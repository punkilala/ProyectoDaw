namespace Models
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Conocimiento")]
    public partial class Conocimiento
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required(ErrorMessage ="Campo requerido")]
        [StringLength(70)]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Campo requerido")]
        [Range(1,100,ErrorMessage ="Valor entre 1 y 100")]
        public int Nivel { get; set; }

        public virtual Usuario Usuario { get; set; }


        public List<Conocimiento> ListaConocimientos(int usuario_id, Filtro filtro)
        {
            var lista = new List<Conocimiento>();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var consulta = from h in bbdd.Conocimiento
                                   .Where(h => h.Usuario_id == usuario_id)
                                   select h;
                    if (filtro.porNombre != null)
                        consulta = consulta.Where(h => h.Nombre.Contains(filtro.porNombre));
                    if (filtro.nombreOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(h => h.Nombre);
                    if (filtro.nombreOrderBy == "Asc")
                        consulta = consulta.OrderBy(h => h.Nombre);
                    if (filtro.tituloOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(h => h.Nivel);
                    if (filtro.tituloOrderBy == "Asc")
                        consulta = consulta.OrderBy(h => h.Nivel);

                    foreach (var registro in consulta)
                    {
                        lista.Add(registro);
                    }
                }
            }
            catch (Exception)
            {

                return lista; ;
            }
            return lista;
        }
        public Conocimiento ObtenerConocimiento(int id)
        {
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var Conocimiento = bbdd.Conocimiento.Where(h => h.id == id).SingleOrDefault();
                    return Conocimiento;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool GuardarConocimiento()
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    if (this.id == 0)
                    {
                        bbdd.Entry(this).State = EntityState.Added;
                        bbdd.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        bbdd.Entry(this).State = EntityState.Modified;
                        bbdd.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }
        public bool EliminarConocimiento(int id)
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var Conocimiento = bbdd.Conocimiento.Where(h => h.id == id).SingleOrDefault();
                    bbdd.Entry(Conocimiento).State = EntityState.Deleted;
                    bbdd.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                return result;
            }
            return result;
        }
    }
}
