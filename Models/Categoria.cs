namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Categoria")]
    public partial class Categoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categoria()
        {
            OfertaEmpleo = new HashSet<OfertaEmpleo>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfertaEmpleo> OfertaEmpleo { get; set; }

        // LOGICA DE NEGOCIO
        public List<Categoria> GetListado()
        {
            var lista = new List<Categoria>();
            try
            {
                using(var bbdd = new ProyectoContexto())
                {
                    lista = bbdd.Categoria.ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {

                return lista;
            }
        }
    }
}
