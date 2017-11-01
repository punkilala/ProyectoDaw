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
    public class AdjuntosController : Controller
    {
        Adjuntos mAdjunto = new Adjuntos();
        RespuestaServidor mRespuestaAjax;
        
        public ActionResult Index()
        {
            Session["menuActivo"] = 5;
            mAdjunto.Usuario_id = SesionHelper.GetUser();
            return View(mAdjunto);
        }
        public PartialViewResult _Listado()
        {
            ViewBag.RutaAdjunta = SubirArchivos.GetRutaAdjuntos();
            return PartialView(mAdjunto.Listado(SesionHelper.GetUser()));
        }
        public JsonResult NuevoAdjunto (HttpPostedFileBase [] Fichero, Adjuntos modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool resultBBDD = false;
            if (ModelState.IsValid)
            {
               foreach( var doc in Fichero)
                {
                    string[] ResultSubida = SubirArchivos.SubirAdjuntos(modelo.Usuario_id, doc);
                    if (ResultSubida[0] == null)
                    {
                        modelo.fichero = ResultSubida[1];
                        resultBBDD = modelo.GuardarDocumento();
                        if (resultBBDD)
                        {
                            mRespuestaAjax.funcion = ("RecargarAdjuntos();");
                            mRespuestaAjax.SetResponse(true, "");
                        }
                        else
                        {
                            mRespuestaAjax.SetResponse(false, "Error de acceso a BBDD pero el documento se ha subido al Server");
                            break;
                        }

                    }
                    else
                    {
                        mRespuestaAjax.SetResponse(false, ResultSubida[0]);
                        break;
                    }
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Debe seleccionar un fichero");
            }
            return Json(mRespuestaAjax);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EliminarAdjunto (int[] id)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool resultBBDD = true;
            if (id!=null)
            {
                for (int i=0; i<id.Length && resultBBDD==true; i++)
                {
                    resultBBDD = mAdjunto.EliminarDocumento(id[i]); 
                }
                if (resultBBDD) mRespuestaAjax.SetResponse(true,"");
                else mRespuestaAjax.SetResponse(false, "Errores al eliminar los adjutos");
                mRespuestaAjax.funcion = "RecargarAdjuntos();";
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Debe seleccionar un registro");
            }
            return Json(mRespuestaAjax);
        }

    }
}