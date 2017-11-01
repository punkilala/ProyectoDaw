using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Filtro
    {
        public string porNombre { get; set; }
        public string porTitulo { get; set; }
        public int porFecha { get; set; }
        public string porCiudad { get; set; }
        public int porCategoria { get; set; }
        public int Salario { get; set; }
        public string nombreOrderBy { get; set; }
        public string tituloOrderBy { get; set; }
        public string desdeOrderBy { get; set; }
        public string hastaOrderBy { get; set; }
        public string numeroOrderBy { get; set; }
        public string inscritosOrderBy { get; set; }
        public int Eliminar { get; set; }
        public int idUsuario { get; set; }
        public string Estado { get; set; }
        public int CambiarEstado { get; set; }
    }
}
