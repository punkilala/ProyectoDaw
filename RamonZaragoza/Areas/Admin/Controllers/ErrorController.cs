using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.titulo = "Ocurrio un error inesperado";
                    ViewBag.Descripcion = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.titulo = "Página no encontrada";
                    ViewBag.Descripcion = "La URL que está intentando ingresar no existe";
                    break;

                default:
                    ViewBag.titulo = "Página no encontrada";
                    ViewBag.Descripcion = "No Puede realizar esta acción";
                    break;
            }
            ViewBag.Error = error;
            ViewBag.Volver = Url.Content("~/Admin/Usuario");

            return View();
        }
    }
}
