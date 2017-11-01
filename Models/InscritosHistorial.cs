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
    [Table("InscritosHistorial")]
    public partial class InscritosHistorial
    {
        public InscritosHistorial()
        {
            Fecha = DateTime.Now;
            EstadoId = 30;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Index]
        public int Usuario_id_D { get; set; }
        [Index]
        public int Oferta_id { get; set; }

        public int EstadoId{ get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public virtual Inscritos Inscritos { get; set; }
        public virtual Estado Estado { get; set; }

        //LOGICA DE NEGOCIO
        public void SetHistorial(int usuario_id, int oferta_id, int elEstado)
        {
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var historial = new InscritosHistorial();
                    historial.Usuario_id_D = usuario_id;
                    historial.Oferta_id = oferta_id;
                    historial.EstadoId = elEstado;

                    bbdd.Entry(historial).State = EntityState.Added;
                    bbdd.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<InscritosHistorial> GetHistorial (int oferta_id)
        {
            int usuario_id = SesionHelper.GetUser();
            var lista = new List<InscritosHistorial>();
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    lista = bbdd.InscritosHistorial
                        .Include("Estado")
                        .Include("Inscritos.OfertaEmpleo")
                        .Where(h => h.Oferta_id == oferta_id)
                        .Where(h => h.Usuario_id_D == usuario_id)
                        .OrderByDescending(h=>h.Fecha)
                        .ToList();
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
