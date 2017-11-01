using Helper;
using Models;
using PagedList;
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
    public class ConocimientoController : Controller
    {
        Conocimiento mConocimiento = new Conocimiento();
        RespuestaServidor mRespuestaAjax;
        public ActionResult Index()
        {
            Session["menuActivo"] = 3;
            return View();
        }
        public ActionResult _Listado(Filtro filtro, int[] idEliminar, int? displayNum, int? pagina)
        {
            if (filtro.Eliminar == 1)
            {
                bool result = true;
                for (int i = 0; i < idEliminar.Count() && result == true; i++)
                {
                    result = mConocimiento.EliminarConocimiento(idEliminar[i]);
                }
            }
            int maxPag = displayNum ?? 5;
            int numPag = pagina ?? 1;

            ViewBag.Listado = mConocimiento.ListaConocimientos(SesionHelper.GetUser(), filtro)
                .ToPagedList(numPag, maxPag);
            return PartialView();
        }
        public ActionResult Acciones(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Title = "Nuevo Conocimiento";
                mConocimiento.id = id;
                mConocimiento.Usuario_id = SesionHelper.GetUser();
            }
            else
            {
                ViewBag.Title = "Editar Conocimiento";
                mConocimiento = mConocimiento.ObtenerConocimiento(id);

            }
            return View(mConocimiento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Acciones(Conocimiento modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool result = false;
            if (ModelState.IsValid)
            {
                result = modelo.GuardarConocimiento();
                if (result)
                {
                    mRespuestaAjax.SetResponse(true, "Datos guardados");
                    mRespuestaAjax.href = Url.Content("~/admin/Conocimiento");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al guardar los datos");
                }

            }
            return Json(mRespuestaAjax);
        }
    }
}