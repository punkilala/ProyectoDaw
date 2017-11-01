﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

namespace Helper
{
    public class SesionHelper
    {
        public static bool ExistUserEnSesion()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static void DestroyUserSession()
        {
            //destruir cookie
            HttpCookie miCookie = new HttpCookie("ASP.NET_SessionId");
            miCookie.Expires = DateTime.Now.AddDays(-1d);
            HttpContext.Current.Response.Cookies.Add(miCookie);

            //destruir variables de sesion
            HttpContext.Current.Session.Clear();

            // destruir autenticacion
            FormsAuthentication.SignOut();
        }
        public static int GetUser()
        {
            int user_id = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user_id = Convert.ToInt32(ticket.UserData);
                }
            }
            return user_id;
        }
        public static void AddUserASesion(string id, string nombre, string foto, int rol)
        {
            bool persist = true;
            var cookie = FormsAuthentication.GetAuthCookie("usuario", persist);

            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = DateTime.Now.AddHours(1);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, id);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Session.Add("nombre", nombre);
            HttpContext.Current.Session.Add("foto", foto);
            HttpContext.Current.Session.Add("rol", rol);
            HttpContext.Current.Session.Add("EstadoMensajes", "1");
            HttpContext.Current.Session.Add("menuActivo", "0");
        }
    }
}