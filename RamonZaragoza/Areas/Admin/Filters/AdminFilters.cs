﻿using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RamonZaragoza.Areas.Admin.Filters
{
    // Si no estamos logeado, regresamos al login
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!SesionHelper.ExistUserEnSesion())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Index"
                }));
            }
            else
            {
                if (HttpContext.Current.Session["nombre"] ==null )
                {
                    SesionHelper.DestroyUserSession();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Index"
                    }));
                }
            }
        }
    }

    // Si estamos logeado ya no podemos acceder a la página de Login
    public class NoLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SesionHelper.ExistUserEnSesion())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Usuario",
                    action = "Index"
                }));
            }
        }
    }
    public class NoLoginBackAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
    //Denegar acceso a los controladores para el rol empresa
    public class NoEmpresaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int rol = Convert.ToInt16(HttpContext.Current.Session["rol"]);
            if (rol == 2)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Usuario",
                    action = "Index"
                }));
            }
        }
    }

    //Denegar acceso a los controladores para el rol Usuario
    public class NoUsurioAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int rol = Convert.ToInt16(HttpContext.Current.Session["rol"]);
            if (rol == 1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Usuario",
                    action = "Index"
                }));
            }
        }
    }
}