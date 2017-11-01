using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Idiomas")]
    public partial class Idiomas
    {
        public Idiomas()
        {
            Idioma = new HashSet<Idioma>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        public virtual ICollection<Idioma> Idioma { get; set; }

        //LOGICA NEGOCIO
        public List<Idiomas> GetIdiomasDisponibles()
        {
            int usuario_id = SesionHelper.GetUser();
            var lista = new List<Idiomas>();
           
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    lista = (from ii in bbdd.Idiomas
                                   .Except(
                                            from ii in bbdd.Idiomas
                                            join i in bbdd.Idioma
                                            on ii.id equals i.Idiomas_id
                                            where i.Usuario_id == usuario_id
                                            select (ii)
                                          )
                                   select (ii)).ToList();
                    return lista;
                }
            }
            catch (Exception ex)
            {

                return lista;
            }       
        }
     }

 }
