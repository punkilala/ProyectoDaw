using Helper;
using Models;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Controllers
{   
    public class PerfilController : Controller
    {
        Usuario mUsuario = new Usuario();
        Idioma mIdioma = new Idioma();

        public ActionResult Index(int id)
        {
           
            var usuario = mUsuario.GetDatosPersonales(id);

            if (Convert.ToInt16(Session["Rol"]) == 1)
            {
                //Un Candidato solo puede ver su perfil o un perfil de empresa;
                if (id != SesionHelper.GetUser() && usuario.Rol_id==1)
                {
                    return Content("<script>window.close();</script>");
                }
            }

            //Si no estas autenticado solo se puede ver el perfil de las empresas
            if (SesionHelper.GetUser() == 0 && usuario.Rol_id==1)
            {
                return Content("<script>window.close();</script>");
            }
            ViewBag.UsuarioActual = SesionHelper.GetUser();
            return View(usuario);
        }
        public PartialViewResult _MostrarCv(int id)
        {
            return PartialView(mUsuario.GetDatosCv(id));
        }    
        public PartialViewResult _EnviarMensaje()
        {
            return PartialView(new Mensaje());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _EnviarMensaje(Mensaje modelo)
        {
            bool result;
            RespuestaServidor mRespuestaAjax = new RespuestaServidor();
            //el binding me envia el id del usuario al que le envio el msg en el campo id
            //lo paso al campo correcto en el modelo
            modelo.Usuario_id = modelo.id;
            modelo.id = 0;
            if (ModelState.IsValid)
            {
                result = modelo.SetMensaje();
                if (result) mRespuestaAjax.SetResponse(true, "Mensaje enviado");
                else mRespuestaAjax.SetResponse(false, "Error Al enviar el mensaje");
            }
            return Json(mRespuestaAjax);
        }

        public ActionResult ExportaPdf(int id)
        {
            return new Rotativa.MVC.ActionAsPdf("PDF", new { id=id});
        }
        public ActionResult PDF(int id)
        {
            return View(mUsuario.GetDatosPersonales(id));
        }
    }
}