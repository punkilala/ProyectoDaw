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
    using System.Web;

    [Table("Mensaje")]
    public partial class Mensaje
    {
        public Mensaje()
        {
            Fecha = DateTime.Now;
            Estado_id = 1;
            relacion = "mensajes";
        }
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Asunto { get; set; }

        [Required]
        [StringLength(50)]
        public string Remitente { get; set; }

        [Column("Mensaje", TypeName = "text")]
        [Required]
        public string Mensaje1 { get; set; }

        public DateTime Fecha { get; set; }

        public int Estado_id { get; set; }

        [StringLength(50)]
        public string relacion { get; set; }

        public virtual Usuario Usuario { get; set; }

        //LOGICA DE NEGOCIO

        public List<Mensaje> ListaMensajes(int usuario_id, int estado_id)
        {
            var lista = new List<Mensaje>();
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var consulta = from m in bbdd.Mensaje.Where(m => m.Usuario_id == usuario_id)
                                   .Where(m => m.Estado_id == estado_id)
                                   .OrderByDescending(m=>m.Fecha)
                                   select m;

                    foreach (var registro in consulta)
                    {
                        lista.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {

                return lista;
            }
            return lista;
        }
        public Mensaje LeerMensaje(int id)
        {
            Mensaje mensaje = null;
            int usuario_id = SesionHelper.GetUser();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    mensaje = bbdd.Mensaje
                        .Where(m => m.id == id)
                        .Where(m => m.Usuario_id == usuario_id)
                        .SingleOrDefault();
                    // si mensaje es null, salta un excepcion y devuelve null  y es evaluada por el controlador
                    //Origen.... Intento de leer mensajes de otros usuarios.
                    if (mensaje.Estado_id == 1)
                    {
                        //marcar como leido
                        mensaje.Estado_id = 2;
                        bbdd.Entry(mensaje).Property(m => m.Estado_id).IsModified = true;
                        bbdd.SaveChanges();
                    }

                    return mensaje;
                }
            }
            catch (Exception)
            {

                return mensaje;
            }
        }
        public bool EliminarMensaje(int id)
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var mensaje = bbdd.Mensaje.Where(m => m.id == id).SingleOrDefault();
                    bbdd.Entry(mensaje).State = EntityState.Deleted;
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
        public int GetSinLeer(int usuario_id)
        {
            int result = 0;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    result = bbdd.Mensaje.Where(m => m.Usuario_id == usuario_id)
                         .Where(m => m.Estado_id == 1).Count();
                }
            }
            catch (Exception)
            {

                return result;
            }
            return result;
        }
        public bool SetMensaje()
        {
            bool result = false;
            using (var bbdd = new ProyectoContexto())
            {
                try
                {
                    bbdd.Entry(this).State = EntityState.Added;
                    bbdd.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {

                    return result;
                }
            }
            return result;
        }
    }
}
