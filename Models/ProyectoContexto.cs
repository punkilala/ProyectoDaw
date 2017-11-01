namespace Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProyectoContexto : DbContext
    {
        public ProyectoContexto()
            : base("name=ProyectoContexto")
        {
        }

        public virtual DbSet<Adjuntos> Adjuntos { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Conocimiento> Conocimiento { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Experiencia> Experiencia { get; set; }
        public virtual DbSet<Inscritos> Inscritos { get; set; }
        public virtual DbSet<Mensaje> Mensaje { get; set; }
        public virtual DbSet<OfertaEmpleo> OfertaEmpleo { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Idiomas> Idiomas { get; set; }
        public virtual DbSet<Idioma> Idioma { get; set; }
        public virtual DbSet<InscritosHistorial> InscritosHistorial { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Adjuntos>()
                .Property(e => e.fichero)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.OfertaEmpleo)
                .WithRequired(e => e.Categoria)
                .HasForeignKey(e => e.Categoria_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conocimiento>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Relacion)
                .IsUnicode(false);

            modelBuilder.Entity<Experiencia>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Experiencia>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Experiencia>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);


            modelBuilder.Entity<Mensaje>()
                .Property(e => e.Remitente)
                .IsUnicode(false);

            modelBuilder.Entity<Mensaje>()
                .Property(e => e.Mensaje1)
                .IsUnicode(false);

            modelBuilder.Entity<Mensaje>()
                .Property(e => e.relacion)
                .IsUnicode(false);

            modelBuilder.Entity<OfertaEmpleo>()
                .Property(e => e.RequisitosMin)
                .IsUnicode(false);

            modelBuilder.Entity<OfertaEmpleo>()
                .Property(e => e.ExperienciaMin)
                .IsUnicode(false);

            modelBuilder.Entity<OfertaEmpleo>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<OfertaEmpleo>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<OfertaEmpleo>()
                .HasMany(e => e.Inscritos)
                .WithRequired(e => e.OfertaEmpleo)
                .HasForeignKey(e => e.Oferta_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuario)
                .WithOptional(e => e.Rol)
                .HasForeignKey(e => e.Rol_id);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Dni)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Ciudad)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Cp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Pais)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Telefono)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Movil)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Adjuntos)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Conocimiento)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Experiencia)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Inscritos)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id_D)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Inscritos1)
                .WithRequired(e => e.Usuario1)
                .HasForeignKey(e => e.Usuario_id_E)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Mensaje)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.OfertaEmpleo)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id);


            modelBuilder.Entity<Usuario>()
               .HasMany(e => e.Idioma)
               .WithRequired(e => e.Usuario)
               .HasForeignKey(e => e.Usuario_id);

            modelBuilder.Entity<Idiomas>()
              .HasMany(e => e.Idioma)
              .WithRequired(e => e.Idiomas)
              .HasForeignKey(e => e.Idiomas_id);

           
        }
    }
}
