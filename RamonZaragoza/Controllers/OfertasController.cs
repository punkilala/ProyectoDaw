using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace RamonZaragoza.Controllers
{
    public class OfertasController : Controller
    {
        OfertaEmpleo mOferta = new OfertaEmpleo();
        Categoria mCategoria = new Categoria();
        public ActionResult Index()
        {
            ViewBag.Ciudades = mOferta.GetCiudadConOferta();
            ViewBag.Categorias = mCategoria.GetListado();
            return View();
        }
        public PartialViewResult _Listado()
        {
            var listadoOfertas = mOferta.GetOfertasAbiertas();
            ViewBag.TotalOfertas = listadoOfertas.Count();
            return PartialView(listadoOfertas.ToPagedList(1, 10));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _Listado(Filtro filtro, int ? pagina)
        {
            int numPag = pagina ?? 1;
            var listaOfertas = mOferta.GetOfertasAbiertas(filtro);
            ViewBag.TotalOfertas = listaOfertas.Count();
            return PartialView(listaOfertas.ToPagedList(numPag, 10));
        }

        public ActionResult Detalle (int id)
        {
            var oferta = mOferta.GetOfertaDetalle(id);
            // solo ofertas abiertas
            if (oferta.Abierta == false) return RedirectToAction("Index", "Home");

            ViewBag.UsuarioActivo = SesionHelper.GetUser();
            ViewBag.OfertasRelacionadas = mOferta.GetOfertasRelacionadas(3, oferta.id, oferta.Categoria_id);
            return View(oferta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult  Detalle (Inscritos modelo)
        {
            bool result;
            RespuestaServidor mRespuestaAjax = new RespuestaServidor();
            mRespuestaAjax.SetResponse(true, "<span style='color:#449D44; float:right;'>Usted está inscrito</span>");
            result = modelo.SetMiInscripcion();
            if (result)
            {
                InscritosHistorial historial = new InscritosHistorial();
                historial.SetHistorial(modelo.Usuario_id_D, modelo.Oferta_id, 30);
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "<span style='color:#9C3334; float:right;'>Error en inscripción</span>");
            }
            return Json(mRespuestaAjax);
        }
    }
}