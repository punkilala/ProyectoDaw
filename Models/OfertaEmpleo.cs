namespace Models
{
    using Anonimas;
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("OfertaEmpleo")]
    public partial class OfertaEmpleo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OfertaEmpleo()
        {
            Inscritos = new HashSet<Inscritos>();
        }

        public int id { get; set; }

        [Required]
        public int Usuario_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string RequisitosMin { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ExperienciaMin { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Descripcion { get; set; }

        public int Categoria_id { get; set; }

        [Required]
        public int Vacantes { get; set; }

        public int Salario { get; set; }
        public string  Pago { get; set; }
        public string ModoPago { get; set; }


        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public bool  Abierta { get; set; }
        [Required]
        [StringLength(30)]
        public string Localidad { get; set; }

        public virtual Categoria Categoria { get; set; }

        public DateTime? FechaGestion { get; set; }
        public virtual ICollection<Inscritos> Inscritos { get; set; }

        public virtual Usuario Usuario { get; set; }

        // LOGICA DE NEGOCIO BACK-END

        public List<OfertaEmpleo> GetLista(Filtro filtro)
        {
            int usuario_id = SesionHelper.GetUser();
            var lista = new List<OfertaEmpleo>();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    lista = bbdd.OfertaEmpleo.Include("Inscritos").Where(oe => oe.Usuario_id == usuario_id)
                        .OrderByDescending(oe=>oe.Fecha).ToList();

                    if (filtro.numeroOrderBy=="Desc") lista = lista.OrderByDescending(x => x.id).ToList();
                    if (filtro.numeroOrderBy == "Asc") lista = lista.OrderBy(x => x.id).ToList();
                    if (filtro.nombreOrderBy=="Desc") lista = lista.OrderByDescending(x => x.Nombre).ToList();
                    if (filtro.nombreOrderBy == "Asc") lista = lista.OrderBy(x => x.Nombre).ToList();
                    if (filtro.desdeOrderBy == "Desc") lista = lista.OrderByDescending(x => x.Fecha).ToList();
                    if (filtro.desdeOrderBy == "Asc") lista = lista.OrderBy(x => x.Fecha).ToList();
                    if (filtro.inscritosOrderBy == "Desc") lista = lista.OrderByDescending(x => x.Inscritos.Count()).ToList();
                    if (filtro.inscritosOrderBy == "Asc") lista = lista.OrderBy(x => x.Inscritos.Count()).ToList();
                    if (filtro.Estado=="false")
                        lista = lista.Where(x => x.Abierta == false).ToList();
                    else
                        lista = lista.Where(x => x.Abierta == true).ToList();
                    if (filtro.porNombre!= null)
                        lista = lista.Where(x => x.Nombre.ToLower().Contains(filtro.porNombre.ToLower().Trim())).ToList();
                    if (filtro.porTitulo != null)
                    {
                        int num;
                        int.TryParse(filtro.porTitulo, out num);
                        lista = lista.Where(x => x.id.Equals(num)).ToList();

                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return lista;
            }
        }
        public OfertaEmpleo GetOferta (int id)
        {
            int usuario_id = SesionHelper.GetUser();
            var oferta = new OfertaEmpleo();
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    oferta = bbdd.OfertaEmpleo.Where(oe => oe.id == id).Where(oe => oe.Usuario_id == usuario_id).SingleOrDefault();
                    //registrar la ultma vez que se entra a ver la oferta por parte de la empresa
                    if (oferta != null)
                    {
                        oferta.FechaGestion = DateTime.Now;
                        bbdd.Entry(oferta).State = EntityState.Modified;
                        bbdd.SaveChanges();
                    }
                }
                return oferta;
            }
            catch (Exception)
            {

                return oferta;
            }
        }       
        public bool SetOferta()
        {
            bool result = false;
            try
            {
                using(var bbdd=new ProyectoContexto())
                {
                    if (this.id == 0)
                    {
                        bbdd.Entry(this).State = EntityState.Added;
                    }
                    else
                    {
                        var oe = bbdd.Entry(this);
                        bbdd.Entry(this).State = EntityState.Modified;
                        
                    }
                    bbdd.SaveChanges();
                    result = true;
                }
                return result;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {

                return false;
            }
        }
        public void CambiarEstado(int id)
        {
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var oferta = bbdd.OfertaEmpleo.Where(o => o.id == id).SingleOrDefault();
                    oferta.Abierta= oferta.Abierta ? false : true;
                    bbdd.Entry(oferta).Property(o => o.Abierta).IsModified = true;
                    bbdd.SaveChanges();

                    //actualizar InscritosHistorial
                    int estado = oferta.Abierta ? 35 : 31;
                    var lista = new List<Inscritos>();
                    Inscritos inscritos = new Inscritos();
                    lista = inscritos.GetInscritos(id);
                    InscritosHistorial historial = new InscritosHistorial();
                    foreach (var item in lista)
                    {
                        historial.SetHistorial(item.Usuario_id_D, item.Oferta_id, estado);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // LOGICA DE NEGOCIO FRONT-END

        public List<OfertaEmpleo> GetOfertasAbiertas(Filtro filtro = null)
        {
            var lista = new List<OfertaEmpleo>();
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    lista = bbdd.OfertaEmpleo
                        .Include("Usuario")
                        .Include("Inscritos")
                        .Include("Categoria")
                        .Where(oe=>oe.Abierta == true)
                        .OrderByDescending(oe=>oe.Fecha)
                        .ToList();

                    //filtros 
                    if (filtro.porTitulo != null) lista = lista.Where(oe => oe.Nombre.ToLower().Contains(filtro.porTitulo.ToLower().Trim())).ToList();
                    if (filtro.porFecha > 0) lista = lista.Where(oe => oe.Fecha >= DateTime.Today.AddDays(-filtro.porFecha)).ToList();
                    if (filtro.porCiudad != null) lista = lista.Where(oe => oe.Localidad == filtro.porCiudad).ToList();
                    if (filtro.porCategoria > 0) lista = lista.Where(oe => oe.Categoria_id == filtro.porCategoria).ToList();
                    if (filtro.Salario > 0)
                    {
                        lista = lista.Where(oe => oe.Salario >= filtro.Salario).ToList();
                        lista = lista.Where(oe => oe.Pago == filtro.porNombre).ToList();
                    }
                    return lista;
                }
            }
            catch (Exception)
            {

                return lista;
            }
        }

        /// <summary>
        /// Funcion utilizada por el front-end para mostra el detalle de una oferta
        /// </summary>
        /// <param name="id">requiere el id de la oferta</param>
        /// <returns>Devuelve el detalle de la oferta</returns>
        public OfertaEmpleo GetOfertaDetalle(int id)
        {
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var detalle = bbdd.OfertaEmpleo
                        .Include("Usuario")
                        .Include("Inscritos")
                        .Include("Categoria")
                        .Where(oe => oe.id == id)
                        .SingleOrDefault();
                    return detalle;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<OfertaEmpleo> GetUltimasOfertas(int cuantas)
        {
            var lista = new List<OfertaEmpleo>();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {       
                    lista = bbdd.OfertaEmpleo
                        .Include("Usuario")
                        .Where(oe => oe.Abierta == true)  
                        .OrderByDescending(oe=>oe.Fecha)
                        .Take(cuantas)
                        .ToList();
                }
                return lista;
            }
            catch (Exception)
            {

                return lista;
            }
        }
        /// <summary>
        /// Devolver ofertas relacionadas mediante su categoria en base a una oferta en concreto
        /// </summary>
        /// <param name="cuantas">Cuantas ofertas quieres que se devuelvan</param>
        /// <param name="oferta_id"> La oferta en la que se basa la relacion, esta no se mostrara nunca</param>
        /// <param name="categoria_id">La categoria en la que estan relacionadas</param>
        /// <returns>Lista de ofertas</returns>
        public List<OfertaEmpleo> GetOfertasRelacionadas(int cuantas, int oferta_id, int categoria_id)
        {
            var lista = new List<OfertaEmpleo>();
            try
            {
                using (var bbdd= new ProyectoContexto())
                {
                    lista = bbdd.OfertaEmpleo
                        .Include("Usuario")
                        .Where(oe=>oe.id != oferta_id)
                        .Where(oe=>oe.Categoria_id==categoria_id)
                        .Where(oe=>oe.Abierta==true)
                        .OrderByDescending(oe=>Guid.NewGuid())
                        .Take(cuantas)
                        .ToList();
                    return lista;
                }
            }
            catch (Exception)
            {

                return lista;
            }
        }
        public List<CiudadesConOferta> GetCiudadConOferta()
        {
            try
            {
                using (var bbdd =new ProyectoContexto())
                {
                   var  lista= (from ciudades in bbdd.OfertaEmpleo
                                where ciudades.Abierta == true
                                group ciudades by ciudades.Localidad
                                into grupo
                                select new CiudadesConOferta()
                                {
                                    Ciudad = grupo.Key,
                                    Cuenta = grupo.Select(x => x.Localidad).Count()
                                }).ToList();
                    return lista;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
