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
    public class CandidaturasController : Controller
    {
        Inscritos mInscritos = new Inscritos();
        InscritosHistorial mHistorial = new InscritosHistorial();
        Estado mEstado = new Estado();
        public ActionResult Index()
        {
            Session["menuActivo"] = 6;
            return View();
        }
        public ActionResult _Listado(int? pagina)
        {
            int numPag = pagina ?? 1;
            return PartialView(mInscritos.GetMisCandidaturas().ToPagedList(numPag,6));
        }
        public ActionResult Historial (int id)
        {
            var historial = mHistorial.GetHistorial(id);
            // solo ver el historial de mis candidaturas
            if (historial.Count()==0)
            {
                return RedirectToAction("", "Usuario", null);
            }
            return View(historial);
        }
    }
}