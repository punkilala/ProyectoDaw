using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Helper;
using RamonZaragoza.Areas.Admin.Filters;

namespace RamonZaragoza.Areas.Admin.Controllers.Empresa
{
    [ValidateInput(false)]
    [Autenticado]
    [NoUsurio]
    public class OfertaEmpleoController : Controller
    {
        OfertaEmpleo mOferta = new OfertaEmpleo();
        Categoria mCategoria = new Categoria();
        Estado mEstado = new Estado();
        RespuestaServidor mRespuestaAjax;
        public ActionResult Index()
        {
            Session["menuActivo"] = 1;
            return View();
        }
        public ActionResult _Listado(Filtro filtro, int? displayNum, int? pagina)
        {
            int numPag = pagina ?? 1;
            int maxReg = displayNum ?? 8;

            if (filtro.CambiarEstado > 0)
            {
                mOferta.CambiarEstado(filtro.CambiarEstado);
            }

            return PartialView(mOferta.GetLista(filtro).ToPagedList(numPag,maxReg));
        } 
        public ActionResult Acciones (int id = 0)
        {
            if (id == 0)
            {
                mOferta.id = 0;
                mOferta.Usuario_id = SesionHelper.GetUser();
                ViewBag.Title = "Nueva Oferta de empleo";
            }
            else
            {
                ViewBag.Title = "Modificar Oferta";
                mOferta = mOferta.GetOferta(id);
                // Solo puedo leer mis ofertas
                if (mOferta == null) return RedirectToAction("Index");
            }
            ViewBag.LPago = mEstado.GetPago();
            ViewBag.LModoPago = mEstado.GetModoPago();
            ViewBag.LCategoria= mCategoria.GetListado();
            return View(mOferta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Acciones (OfertaEmpleo modelo)
        {
            mRespuestaAjax = new RespuestaServidor();    
            if(ModelState.IsValid)
            {
                bool result = false;
                result = modelo.SetOferta();
                if (result)
                {
                    mRespuestaAjax.SetResponse(true, "Ok");
                    mRespuestaAjax.href = Url.Action("");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error de acceso a la Base de Datos");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Los campos marcados con * son obligatorios");
            }
            return Json(mRespuestaAjax);
        }
    }
}