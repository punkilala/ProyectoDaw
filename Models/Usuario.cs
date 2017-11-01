namespace Models
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.IO;
    using System.Linq;
    using System.Web;

    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Adjuntos = new HashSet<Adjuntos>();
            Conocimiento = new HashSet<Conocimiento>();
            Experiencia = new HashSet<Experiencia>();
            Inscritos = new HashSet<Inscritos>();
            Inscritos1 = new HashSet<Inscritos>();
            Mensaje = new HashSet<Mensaje>();
            OfertaEmpleo = new HashSet<OfertaEmpleo>();
            Idioma = new HashSet<Idioma>();

            FNacimiento = new DateTime(2001,01,01);
            Foto = "noFoto.jpg";
        }

        public int id { get; set; }

        [StringLength(9)]
        public string Dni { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage ="Campo es requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FNacimiento { get; set; }

        [Column(TypeName = "text")]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Ciudad { get; set; }

        [StringLength(5)]
        public string Cp { get; set; }

        [StringLength(50)]
        public string Pais { get; set; }

        [StringLength(12)]
        public string Telefono { get; set; }

        [StringLength(12)]
        public string Movil { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage ="Dirección de correo no valida")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Foto { get; set; }

        public byte? Rol_id { get; set; }

        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Web { get; set; }

        [Column(TypeName = "text")]
        public string InfoGeneral { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 32 caracteres")]
        public string PassActual { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 32 caracteres")]
        public string PassNuevo { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 10 caracteres")]
        [Compare("PassNuevo", ErrorMessage = "Nueva Contraseña y Repetir Contraseña no coinciden")]
        public string PassRepetir { get; set; }

        public virtual ICollection<Adjuntos> Adjuntos { get; set; }
        public virtual ICollection<Conocimiento> Conocimiento { get; set; }
        public virtual ICollection<Experiencia> Experiencia { get; set; }
        public virtual ICollection<Inscritos> Inscritos { get; set; }
        public virtual ICollection<Inscritos> Inscritos1 { get; set; }
        public virtual ICollection<Mensaje> Mensaje { get; set; }
        public virtual ICollection<OfertaEmpleo> OfertaEmpleo { get; set; }
        public virtual ICollection<Idioma> Idioma { get; set; }

        public virtual Rol Rol { get; set; }



        // ///////LOGICA DE NEGOCIO
        public Usuario Acceder(string email, string password)
        {

            Usuario usuario = (Usuario)null;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    //convertir password a md5
                    password = HashHelper.MD5(password);
                    usuario = bbdd.Usuario.Where(u => u.Email == email)
                       .Where(u => u.Password == password).SingleOrDefault();
                    if (usuario != null)
                    {
                        SesionHelper.AddUserASesion(usuario.id.ToString(),
                            usuario.Nombre, usuario.Foto, Convert.ToInt16(usuario.Rol_id));

                        Mensaje alerta = new Mensaje();
                        HttpContext.Current.Session.Add("MensajesSinLeer", alerta.GetSinLeer(usuario.id));
                    }
                }
            }
            catch (Exception ex)
            {

                return usuario;
            }
            return usuario;
        }
        public Usuario UsuarioActivo()
        {
            Usuario activo = null;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    int id = SesionHelper.GetUser();
                    activo = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                return activo;
            }
            return activo;
        }
        public bool GuardarUsuario()
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {

                    var usuario = bbdd.Entry(this);
                    usuario.State = EntityState.Modified;

                    //El campo password decimos que no se modifica ya que no se lo pasamos.
                    usuario.Property(u => u.Password).IsModified = false;
                    usuario.Property(u => u.Rol_id).IsModified = false;

                    //Para que no de error al grabar puesto que no pasamos Password
                    bbdd.Configuration.ValidateOnSaveEnabled = false;
                    bbdd.SaveChanges();

                    HttpContext.Current.Session["nombre"] = this.Nombre;
                    result = true;
                }
            }
            catch (Exception)
            {

                return result;
            }

            return result;
        }
        public bool GuardarFoto(int id, string nuevaFoto)
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var usuario = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();

                    string fotoAntigua = usuario.Foto;
                    usuario.Foto = nuevaFoto;

                    bbdd.Entry(usuario).State = EntityState.Modified;
                    bbdd.Configuration.ValidateOnSaveEnabled = false;
                    bbdd.SaveChanges();
                    result = true;
                    SubirArchivos.BorrarFotoAntigua(id, fotoAntigua);
                    HttpContext.Current.Session["foto"] = nuevaFoto;
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }
        public int CambiarPassword(int id, string actualPass, string nuevoPass)
        {
            int result = -1;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var usuario = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();
                    actualPass = HashHelper.MD5(actualPass);
                    if (actualPass == usuario.Password)
                    {
                        usuario.Password = HashHelper.MD5(nuevoPass);
                        bbdd.Configuration.ValidateOnSaveEnabled = false;
                        bbdd.SaveChanges();
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
        public Usuario GetDatosPersonales (int id)
        {
            Usuario usuario=null;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    usuario = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();
                }
                return usuario;
            }
            catch (Exception ex)
            {

                return usuario;
            }
        }
        public Usuario GetDatosCv (int id)
        {
            Usuario usuario=null;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    usuario = bbdd.Usuario
                        .Include("Experiencia")
                        .Include("Conocimiento")
                        .Include("Adjuntos")
                        .Include("Idioma")
                        .Include("Idioma.Idiomas")
                        .Where(u => u.id == id).SingleOrDefault();
                    return usuario;
                }
            }
            catch (Exception ex)
            {

                return usuario;
            }
        }
        public int SetNuevoUser()
        {       
            int result = -1;

            try
            {         
                using (var bbdd = new ProyectoContexto())
                {
                    // Email no puede existir en la bbdd
                    var usuario = bbdd.Usuario.Where(u => u.Email == this.Email);
                    if (usuario.Count() > 0)
                    {
                        result = 0;
                    }
                    else
                    {
                        //dar de alta usuario nuevo
                        this.Password = HashHelper.MD5(this.PassNuevo);
                        this.PassActual = this.Password; //para que no de error de validacion
                        bbdd.Entry(this).State = EntityState.Added; 
                        bbdd.SaveChanges();
                        result = 1;
                    }
                }
                return result;
               
            }
            catch (Exception)
            {
                return result;
            }
        }
    }
}
