using Helper;
using Models;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    [Autenticado]
    [NoEmpresa]
    public class IdiomaController : Controller
    {
        Idioma mIdioma = new Idioma();
        Idiomas mIdiomas = new Idiomas();
        Estado mEstado = new Estado();
        RespuestaServidor mRespuestaAjax;
        public ActionResult Index()
        {
            Session["menuActivo"] = 4;
            return View();
        }
        public PartialViewResult _Listado()
        {
            return PartialView(mIdioma.GetIdiomas(SesionHelper.GetUser()));
        }
        public ActionResult Acciones (int id=0)
        {
            if (id == 0)
            {
                ViewBag.Title = "Nuevo idioma";
                mIdioma.id = 0;
                mIdioma.Usuario_id = SesionHelper.GetUser();
                //Todos los idiomas menos los que ya tiene el usuario
                ViewBag.IdiomasDisponibles = mIdiomas.GetIdiomasDisponibles();
            }
            else
            {
                ViewBag.Title = "Modificar Idioma";
                mIdioma = mIdioma.GetIdioma(id, SesionHelper.GetUser());
            }  
            
            ViewBag.Nivel = mEstado.GetNiveles();
            return View(mIdioma);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Acciones(Idioma modelo)
        {
            bool result = false;
            mRespuestaAjax = new RespuestaServidor();
            if (ModelState.IsValid)
            {
                result = modelo.Guardar();
                if (result)
                {
                    mRespuestaAjax.SetResponse(true, "Guardado");
                    mRespuestaAjax.href = Url.Content("~/admin/idioma");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error de acceso a la base de datos");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Formulario no valido");
            }
            return Json(mRespuestaAjax);
        }
        [HttpPost]
        public JsonResult Eliminar (int[] idioma_id)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool result = true;
            if (idioma_id.Count() > 0)
            {
                for(int i=0; i< idioma_id.Count() && result==true; i++)
                {
                    result = mIdioma.Eliminar(idioma_id[i]);
                }
                if (result)
                {
                    mRespuestaAjax.SetResponse(true);
                    mRespuestaAjax.funcion=("RecargarPagina();");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al acceder a la base de datos");
                }
            }
            return Json(mRespuestaAjax);
        }
    }
}