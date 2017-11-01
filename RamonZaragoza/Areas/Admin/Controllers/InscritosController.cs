using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RamonZaragoza.Areas.Admin.Filters;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    [Autenticado]
    [NoUsurio]
    public class InscritosController : Controller
    {
        OfertaEmpleo mOferta = new OfertaEmpleo();
        Inscritos mInscritos = new Inscritos();
        Estado mEstado = new Estado();
        InscritosHistorial mHistorial = new InscritosHistorial();
        public ActionResult Oferta(int id=0)
        {
            // Recordar a que oferta tengo que volver
            Session["NumOferta"] = id;


            var oferta = mOferta.GetOferta(id);
            //solo ver mis ofertas
            if (oferta == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Estado = oferta.Abierta;
            ViewBag.Nombre = oferta.Nombre;
            //pasar valor si voy a vista DatosUsuario
            TempData["Nombre"] = oferta.Nombre;
            return View();
        }
        public ActionResult _Listado(int id, int? pagina, int? displayNum, int? estado_id )
        {
            int numPag = pagina ?? 1; 
            int numReg = displayNum ?? 10;
            int estado = estado_id ?? 4;

            // recordar en que estado estoy filtrando para cuando vuelva.. lo leo en vista DatosUsuario 
            // para componer la url de retorno
            Session["EstadoFiltro"] = estado;

            ViewBag.LEstado = mEstado.GetEstadoInscrito();
            var lista = mInscritos.GetInscritos(id, estado);
            ViewBag.Cuenta = lista.Count();
            return PartialView(lista.ToPagedList(numPag,numReg));
        }
        public ActionResult DatosUsuario(int id, Int64 inscrip, int estado)
        {
            // inscripcion que aun no he leido
            if (estado==4)
            {
                // marcar leido
                mInscritos.SetModifiEstado(inscrip, id, 5);
                mHistorial.SetHistorial(id, Convert.ToInt32(Session["NumOferta"]), 32);
            }
            
            
            return View (mInscritos.GetInscripcion(inscrip));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DatosUsuario (Inscritos modelo)
        {

            string respuestaAjax;
            
            mInscritos.SetModifiEstado(modelo.NumInscripcion, modelo.Usuario_id_D, modelo.estado_id);
            if (modelo.estado_id == 6)
            {
                respuestaAjax = "Estado: <span class='badge' style='background:#343A40; color:white;'>Rechazado</span>";
                //actualizar historial
                mHistorial.SetHistorial(modelo.Usuario_id_D, modelo.Oferta_id, 34);
            } else
            {
                respuestaAjax = "Estado: <span class='badge' style='background:#28A745; color:white;'>Pre-Selec</span>";
                //actualizar historial
                mHistorial.SetHistorial(modelo.Usuario_id_D, modelo.Oferta_id, 33);
            }
            return Json(respuestaAjax);

        }
    }
}