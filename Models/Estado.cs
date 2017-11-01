namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Estado")]
    public partial class Estado
    {
        public Estado()
        {
            Inscritos = new HashSet<Inscritos>();
            InscritosHistorial = new HashSet<InscritosHistorial>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Relacion { get; set; }

        public ICollection<Inscritos> Inscritos { get; set; }
        public ICollection<InscritosHistorial> InscritosHistorial{ get; set; }

        //LOGICA DE NEGOCIO

        public List<Estado> GetNiveles()
        {
            var lista = new List<Estado>();
            try
            {
                using(var bbdd = new ProyectoContexto())
                {
                    lista = bbdd.Estado.Where(e => e.Relacion.Equals("idiomas")).ToList();
                    return lista;
                }
            }
            catch (Exception)
            {

                return lista;
            }
        }
        public List<Estado> GetPago()
        {
            var lista = new List<Estado>();
            try
            {
                using(var bbdd=new ProyectoContexto())
                {
                    lista = bbdd.Estado.Where(e => e.Relacion == "Pago").ToList();
                }
                return lista;
            }
            catch (Exception)
            {
                return lista;
            }
        }
        public List<Estado> GetModoPago()
        {
            var lista = new List<Estado>();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    lista = bbdd.Estado.Where(e => e.Relacion == "ModoPago").ToList();
                }
                return lista;
            }
            catch (Exception)
            {
                return lista;
            }
        }
        public List<Estado> GetEstadoInscrito()
        {
            var lista = new List<Estado>();
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    lista = bbdd.Estado.Where(e => e.Relacion == "inscrito").ToList();
                }
                return lista;
            }
            catch (Exception)
            {

                return lista;
            }
        }
    }

}
