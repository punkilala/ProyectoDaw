using Helper;
using Models;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private RespuestaServidor mRespuestaAjax;
        private Usuario mUsuario = new Usuario();

        public ActionResult Index()
        {
            Session["menuActivo"] = 0;
            return View(mUsuario.UsuarioActivo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string Email, string Password)
        {
            mRespuestaAjax = new RespuestaServidor();

            Usuario usuario;
            usuario = mUsuario.Acceder(Email, Password);
            if (usuario != null)
            {
                mRespuestaAjax.SetResponse(true, "Redirigiendo... espere por favor");
                mRespuestaAjax.href = Url.Content("~/admin/usuario");
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Correo o contraseña incorrecta");
            }
            return Json(mRespuestaAjax);

        }

        public ActionResult Logout()
        {
            SesionHelper.DestroyUserSession();
            return Redirect("~/");
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarUsuario(Usuario modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            //Eliminamos la validación del camo Password puesto que no se la pasamos...
            //por motivos de seguridad y asi poder validar el resto de campos sin que de error
            ModelState.Remove("Password");
            //Solo se utilizan para cambiar contraseña
            ModelState.Remove("PassActual");
            ModelState.Remove("PassNuevo");
            ModelState.Remove("PassRepetir");

            if (ModelState.IsValid)
            {
                bool guardado = modelo.GuardarUsuario();
                if (guardado)
                {
                    mRespuestaAjax.SetResponse(true, "Modificaciones realizadas correctmente");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al intentar modificar los datos");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "¡¡Error!! Verifique  los errores de validación");
            }
            return Json(mRespuestaAjax);
        }

        public ActionResult FotoPerfil()
        {
            return View();
        }
        public PartialViewResult _CambiarFoto()
        {
            var usuarioActivo = mUsuario.UsuarioActivo();
            ViewBag.Foto = usuarioActivo.Foto;
            if (ViewBag.Foto != "noFoto.jpg")
            {
                ViewBag.Foto = SubirArchivos.GetRutaFotoPerfil() + usuarioActivo.Foto;
            }
            return PartialView(usuarioActivo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _CambiarFoto(HttpPostedFileBase Foto, Usuario modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool resultBBDD = false;
            if (Foto == null)
            {
                mRespuestaAjax.SetResponse(false, "debe seleccionar un archivo");
            }
            else
            { 
                string[] ResultSubida = SubirArchivos.SubirFotoPerfil(modelo.id, Foto);
                if (ResultSubida[0] == null)
                {
                    resultBBDD = mUsuario.GuardarFoto(modelo.id, ResultSubida[1]);
                    if (resultBBDD)
                    {
                        mRespuestaAjax.funcion = "RecargarFoto();";
                        mRespuestaAjax.SetResponse(true,"");
                    }
                    else
                    {
                        mRespuestaAjax.SetResponse(false, "Error de acceso a BBDD pero la foto se ha subido al Server");
                    }
                    
                 }
                 else
                 {
                    mRespuestaAjax.SetResponse(false, ResultSubida[0]); 
                 }
            }
               
           return Json(mRespuestaAjax);
        }

        public ActionResult CambioDePass()
        {
            // var usuarioActual = mUsuario.UsuarioActivo();
            ViewBag.ID = SesionHelper.GetUser();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CambioDePass(int id, string PassActual, string PassNuevo)
        {
            int result;
            mRespuestaAjax = new RespuestaServidor();
            if (ModelState.IsValid)
            {
                result = mUsuario.CambiarPassword(id, PassActual, PassNuevo);
                if (result == 0)
                {
                    mRespuestaAjax.SetResponse(false, "Contraseña Actual no coincide");
                }
                else if (result == 1)
                {
                    mRespuestaAjax.SetResponse(true, "Cambio realizado OK");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al grabar Password");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Error inesplicable");
            }
            return Json(mRespuestaAjax);

        }
    }
}