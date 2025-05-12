using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SMI.Shared.Models;

namespace SMI.Server.Data
{
    public partial class SGISDbContext : DbContext
    {
        public SGISDbContext(DbContextOptions<SGISDbContext> options) : base(options) { }

        public DbSet<User> Usuario { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<PersonaDocumento> PersonaDocumentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Persona
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Persona");

                entity.HasKey(p => p.id);

                entity.Property(p => p.id)
                      .ValueGeneratedOnAdd(); // ID generado por la base de datos

                entity.Property(p => p.nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.apellido)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Correo)
                      .HasMaxLength(200);

                entity.HasMany(p => p.Usuarios)
                      .WithOne(u => u.Persona)
                      .HasForeignKey(u => u.Id_Persona)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Documentos)
                      .WithOne(pd => pd.Persona)
                      .HasForeignKey(pd => pd.id_Persona)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Usuario
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Clave).IsRequired();
                entity.Property(u => u.Activo).IsRequired();
            });

            // TipoDocumento
            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.HasKey(td => td.id);

                entity.Property(td => td.nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasMany(td => td.PersonaDocumentos)
                      .WithOne(pd => pd.TipoDocumento)
                      .HasForeignKey(pd => pd.id_TipoDocumento)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PersonaDocumento
            modelBuilder.Entity<PersonaDocumento>().ToTable("PersonaDocumento");

            modelBuilder.Entity<PersonaDocumento>()
                .HasKey(pd => new { pd.id_Persona, pd.id_TipoDocumento });

            modelBuilder.Entity<PersonaDocumento>()
                .HasOne(pd => pd.Persona)
                .WithMany(p => p.Documentos)
                .HasForeignKey(pd => pd.id_Persona);

            modelBuilder.Entity<PersonaDocumento>()
                .HasOne(pd => pd.TipoDocumento)
                .WithMany(td => td.PersonaDocumentos)
                .HasForeignKey(pd => pd.id_TipoDocumento);


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
