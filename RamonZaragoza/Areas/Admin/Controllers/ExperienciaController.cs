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
    public class ExperienciaController : Controller
    {

        Experiencia mExperiencia = new Experiencia();
        RespuestaServidor mRespuestaAjax;
        public ActionResult Index(int tipo)
        {
            //seguridad
            if (tipo > 2 || tipo <1) return RedirectToAction("Logout", "Usuario");

            Session["menuActivo"] = tipo == 1 ? 1 : 2;
            ViewBag.Tipo = tipo;
            ViewBag.Title = tipo == 1 ? "Experiencia Profesional" : "Estudios realizados";
            return View();
        }
        public ActionResult _Listado(int tipo, int[] idEliminar, Filtro filtro,
            int? displayNum, int? pagina, int? btnEliminar)
        {
            // ////////////opcion eliminar registros
            if (filtro.Eliminar == 1)
            {
                bool result = true;
                for (int i = 0; i < idEliminar.Count() && result == true; i++)
                {
                    result = mExperiencia.EliminaExperiencia(idEliminar[i]);
                }
                ViewBag.Eliminar = 0;
            }
            // //////////////////paginacion
            int maxPag = displayNum ?? 5;
            int numPag = pagina ?? 1;

            ViewBag.Tipo = tipo;
            ViewBag.Listado = mExperiencia.Listado(tipo, SesionHelper.GetUser(), filtro)
                .ToPagedList(numPag, maxPag);

            return PartialView();
        }
        [ValidateInput(false)]
        public ActionResult Acciones(byte tipo, int id = 0)
        {
            if (id == 0) //nuevo registro
            {
                mExperiencia.Tipo = tipo;
                mExperiencia.Usuario_id = SesionHelper.GetUser();
            }
            else // modificacion
            {
                mExperiencia = mExperiencia.ObtenerExperiencia(id);
            }
            return View(mExperiencia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult Agregar(Experiencia modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            mRespuestaAjax.mensaje = "El campo Descripción es requerido";
            if (ModelState.IsValid)
            {
                int result = DateTime.Compare(modelo.Desde, modelo.Hasta);
                if (result > 0)
                {
                    mRespuestaAjax.SetResponse(false, "Campo 'Hasta' no puede ser <b>MENOR</b> que 'Desde'");
                }
                else
                {
                    bool guardado = modelo.GuardarExperiencia();
                    if (guardado)
                    {
                        mRespuestaAjax.SetResponse(true);
                        mRespuestaAjax.href = Url.Content("~/admin/experiencia?tipo=" + modelo.Tipo);
                    }
                    else
                    {
                        mRespuestaAjax.SetResponse(false, "Error al guardar los datos");
                    }
                }
            }

            return Json(mRespuestaAjax);
        }
    }
}