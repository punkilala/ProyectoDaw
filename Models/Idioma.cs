using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Idioma")]
    public partial class Idioma
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int Usuario_id { get; set; }
        public int Idiomas_id { get; set; }
        [Required]
        [StringLength(20)]
        public string Hablado { get; set; }
        [Required]
        [StringLength(20)]
        public string Escrito { get; set; }

        public virtual Idiomas Idiomas { get; set; }
        public virtual Usuario Usuario { get; set; }

        //LOGICA DE NEGOCIO
        public List<Idioma> GetIdiomas(int usuario_id)
        {
            var idiomas = new List<Idioma>();
            try
            {
                using (var bbdd=new ProyectoContexto())
                {
                    idiomas = bbdd.Idioma.Include("Idiomas").Where(i => i.Usuario_id == usuario_id).ToList();
                }
            }
            catch (Exception)
            {

                return idiomas;
            }
            return idiomas;
        }
        public Idioma GetIdioma (int id, int usuario_id)
        {
            Idioma idioma = null;
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    idioma = bbdd.Idioma.Include("Idiomas").Where(i => i.id == id).Where(i=>i.Usuario_id==usuario_id).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                return idioma;
            }
            return idioma;
        }

        public bool Guardar()
        {
            bool result = false;
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    if (this.id == 0)
                    {
                        bbdd.Entry(this).State = EntityState.Added;  
                    }
                    else
                    {
                        bbdd.Entry(this).State = EntityState.Modified;
                    }
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
        public bool Eliminar(int id)
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var idioma= bbdd.Idioma.Where(i => i.id == id).SingleOrDefault();
                    bbdd.Entry(idioma).State = EntityState.Deleted;
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
