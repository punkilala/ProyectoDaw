
using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Controllers
{
    public class HomeController : Controller
    {
        RespuestaServidor mRespuestaAjax;
        OfertaEmpleo mUltimasOfertas = new OfertaEmpleo();

        public ActionResult Index()
        {
            return View(mUltimasOfertas.GetUltimasOfertas(4));
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Contacto";

            return View();
        }
        public ActionResult Sumario()
        {
            return View();
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _Registro(Usuario modelo)
        {
            
            mRespuestaAjax = new RespuestaServidor();
            //retirar de la validacion, ya que no se envian en el post
            ModelState.Remove("Password");
            ModelState.Remove("PassActual");
            if (ModelState.IsValid)
            {
                int result = modelo.SetNuevoUser();
                if (result == 1)
                {
                    mRespuestaAjax.SetResponse(true, "Usuario dado de alta correctamente");
                } else if (result == 0)
                {
                    mRespuestaAjax.SetResponse(false, "No se ha dado de alta en el sistema... Motivo:<b> Email existente</b>");
                }
            }
            return Json(mRespuestaAjax);
        }
    }
}