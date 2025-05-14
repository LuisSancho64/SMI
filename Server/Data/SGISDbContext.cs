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
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<PersonaLugarResidencia> PersonasLugaresResidencia { get; set; }
        public DbSet<EstadoCivil> EstadosCiviles { get; set; }
        public DbSet<PersonaEstadoCivil> PersonasEstadosCiviles { get; set; }


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

            //Provincia
            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.ToTable("Provincia");
                entity.HasKey(p => p.id);
                entity.Property(p => p.nombre)
                       .HasMaxLength(100)
                       .IsRequired();
            });

            //Ciudad
            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.ToTable("Ciudad");
                entity.HasKey(c => c.id);
                entity.Property(c => c.nombre)
                       .HasMaxLength(100)
                       .IsRequired();

                entity.HasOne(c => c.Provincia)
                      .WithMany(p => p.Ciudades)
                      .HasForeignKey(c => c.id_Provincia)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // PersonaLugarResidencia
            modelBuilder.Entity<PersonaLugarResidencia>(entity =>
            {
                entity.ToTable("PersonaLugarResidencia");
                entity.HasKey(plr => new { plr.id_Persona, plr.id_Ciudad });

                entity.HasOne(plr => plr.Persona)
                      .WithMany(p => p.LugaresResidencia)
                      .HasForeignKey(plr => plr.id_Persona)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(plr => plr.Ciudad)
                      .WithMany()
                      .HasForeignKey(plr => plr.id_Ciudad)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //EsatadoCivil
            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.ToTable("EstadoCivil");
                entity.HasKey(ec => ec.id);
                entity.Property(ec => ec.nombre)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            //PersonaEstadoCivil
            modelBuilder.Entity<PersonaEstadoCivil>()
                .ToTable("PersonaEstadoCivil")
                .HasKey(pec => new { pec.id_Persona, pec.id_EstadoCivil });

            modelBuilder.Entity<PersonaEstadoCivil>()
                .HasOne(pec => pec.Persona)
                .WithMany(p => p.EstadosCiviles)
                .HasForeignKey(pec => pec.id_Persona);

            modelBuilder.Entity<PersonaEstadoCivil>()
                .HasOne(pec => pec.EstadoCivil)
                .WithMany()
                .HasForeignKey(pec => pec.id_EstadoCivil);


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
