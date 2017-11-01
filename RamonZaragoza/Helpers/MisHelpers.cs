using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Helpers
{
    public static class MisHelpers
    {
        public static string CortarTexto(this HtmlHelper helper, string cadena, int longitud)
        {
            cadena = Regex.Replace(cadena, @"<[^>]+>|&nbsp;|\n|\r", String.Empty).Trim();
            if (cadena.Length > longitud)
            {
                cadena = cadena.Substring(0, longitud)+"...";
            }
            return cadena;
        }
    }
}