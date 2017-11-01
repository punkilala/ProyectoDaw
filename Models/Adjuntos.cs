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

    public partial class Adjuntos
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required]
        [StringLength(100)]
        public string fichero { get; set; }

        public virtual Usuario Usuario { get; set; }

        // //////   lOGICA DE NEGOCIO
        public List<Adjuntos> Listado(int usuario_id)
        {
            var lista = new List<Adjuntos>();
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                   lista = bbdd.Adjuntos.Where(a => a.Usuario_id == usuario_id).ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {

                return lista;
            }
        }

        public bool GuardarDocumento()
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    bbdd.Entry(this).State = EntityState.Added;
                    bbdd.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }

        public bool EliminarDocumento(int id)
        {
            bool result = false;
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                     var adjunto = bbdd.Adjuntos.Where(a => a.id == id).SingleOrDefault();
                     string nombre = adjunto.fichero;
                     int usuario_id = adjunto.Usuario_id;
                     bbdd.Entry(adjunto).State = EntityState.Deleted;
                     bbdd.SaveChanges();
                     result = true;
                     SubirArchivos.BorrarAdjunto(usuario_id, nombre);
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }
    }
}
