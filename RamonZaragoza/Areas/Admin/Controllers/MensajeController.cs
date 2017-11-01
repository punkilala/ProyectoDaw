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
    public class MensajeController : Controller
    {
        Mensaje mMensajes = new Mensaje();

        public ActionResult Index()
        {
            Session["menuActivo"] = 7;
            return View();
        }
        public PartialViewResult _Listado(Filtro filtro, int[] idEliminar, int? pagina, int ? estado_id)
        {
            if (filtro.Eliminar==1)
            {
                int queBorro = Convert.ToInt16(Session["EstadoMensajes"]);
                bool result = true;
                for (int i = 0; i < idEliminar.Count() && result == true; i++)
                {
                    result = mMensajes.EliminarMensaje(idEliminar[i]);  
                    if (result && queBorro == 1) RestarMensajeSinLeer();
                }   
            }

            int numPag = pagina ?? 1;
            int maxReg = 5;
            int estado = estado_id ?? 1;

            //para saber si leo algun mensaje a donde tengo que volver, si a 'sin leer' o a leidos'
            //en las vista LeerMensajes leo esta variable de sesion antes de regresar
            Session["EstadoMensajes"] = estado;

            var mensajes = mMensajes.ListaMensajes(SesionHelper.GetUser(),estado).ToPagedList(numPag, maxReg);

            if (estado == 1) ViewBag.Estado = "Sin Leer";
            else ViewBag.Estado = "Leidos";

            return PartialView(mensajes);
        }

        public ActionResult LeerMensaje(int id)
        {
            mMensajes = mMensajes.LeerMensaje(id);
            // solo leo mis mensajes
            if (mMensajes == null)
            {
                return RedirectToAction("index", "usuario");
            }
            int queLeo = Convert.ToInt16(Session["EstadoMensajes"]);
            //queLeo=1 es mensaje nuevo
            if (queLeo==1) RestarMensajeSinLeer();
            return View(mMensajes);
        }
        private void RestarMensajeSinLeer()
        {
            int mensajesSinLeer = Convert.ToInt16(@Session["MensajesSinLeer"]);            
            mensajesSinLeer--;
            Session["MensajesSinLeer"] = mensajesSinLeer;
        }
        
    }
}