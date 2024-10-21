using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiForm.Repository.Models;
using WebApiForm.Services;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; // Asegúrate de incluir este espacio de nombres

namespace WebApiForm.Repository;

public partial class FormEncuestaDbContext : DbContext
{
    public FormEncuestaDbContext()
    {
    }

    public FormEncuestaDbContext(DbContextOptions<FormEncuestaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EncuestaOrden> EncuestaOrdens { get; set; }

    public virtual DbSet<Estacion> Estacions { get; set; }

    public virtual DbSet<Formulario> Formularios { get; set; }

    public virtual DbSet<Linea> Lineas { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<RegistroUsuario> RegistroUsuarios { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Sesion> Sesions { get; set; }

    public virtual DbSet<SubPregunta> SubPreguntas { get; set; }

    public DbSet<PreguntaCompleta> PreguntaCompletas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EncuestaOrden>(entity =>
        {
            entity.HasKey(e => e.Year).HasName("PK__Encuesta__D4BD60559D101B9B");
        });

        modelBuilder.Entity<Estacion>(entity =>
        {
            entity.HasKey(e => e.IdEstacion).HasName("PK__Estacion__1F3B45EB77812F5C");

            entity.Property(e => e.IdEstacion).ValueGeneratedNever();
        });

        modelBuilder.Entity<Formulario>(entity =>
        {
            entity.HasKey(e => e.IdentifacadorForm).HasName("PK__Formular__6CDA1CA2297646DD");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Formularios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_User_Form");
        });

        modelBuilder.Entity<Linea>(entity =>
        {
            entity.HasKey(e => e.IdLinea).HasName("PK__Linea__E346BA197EC0FCF2");
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.CodPregunta).HasName("PK__Pregunta__9277FCFEA889D975");

            entity.Property(e => e.CodPregunta).ValueGeneratedNever();
        });

        modelBuilder.Entity<RegistroUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Registro__854B73B3E3501785");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasKey(e => e.IdRespuestas).HasName("PK__Respuest__D875135C8E8B1BE2");

            entity.HasOne(d => d.CodPreguntaNavigation).WithMany(p => p.Respuestas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Respuestas_Pregunta");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Respuestas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Respuestas_User");
        });

        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("PK__Sesion__8D3F9DFE95939BB5");

            entity.HasOne(d => d.CodPreguntaNavigation).WithMany(p => p.Sesions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Sesion_Pregunta");

            entity.HasOne(d => d.CodSubPreguntaNavigation).WithMany(p => p.Sesions).HasConstraintName("fk_Sesion_SubPreguntas");
        });

        modelBuilder.Entity<SubPregunta>(entity =>
        {
            entity.HasKey(e => e.CodSubPregunta).HasName("PK__SubPregu__B4EDE11C216D97A1");
        });

        /*Definir PreguntaCompleta como entidad sin clave, ya que el 
        procedimiento almacenado de PreguntaCompleta no existe como tabla*/
        modelBuilder.Entity<PreguntaCompleta>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    // Método para ejecutar el procedimiento almacenado
    public async Task<List<PreguntaCompleta>> GetPreguntasCompleto()
    {
        return await this.PreguntaCompletas.FromSqlRaw("EXEC sp_ObtenerPreguntasCompleto").ToListAsync();
    }

    //adaptacion de stored procedure para insertar datos a la tabla

    public async Task InsertarFormularioAsync(
        string idUsuarios,
        string cedula,
        string? fecha,
        string? hora,
        int? idEstacion,
        string? idLInea,
        int orderNumber)
    {
        var parameters = new[]
        {
            new SqlParameter("@id_usuarios", idUsuarios),
            new SqlParameter("@cedula", cedula),
            new SqlParameter("@fecha", fecha ?? (object) DBNull.Value),
            new SqlParameter("@hora", hora ?? (object) DBNull.Value),
            new SqlParameter("@id_estacion", idEstacion ??(object) DBNull.Value),
            new SqlParameter("@id_linea", idLInea ??(object) DBNull.Value),
            new SqlParameter("@orderNumber", orderNumber)
        };

        await Database.ExecuteSqlRawAsync("EXEC sp_InsertarFormulario @id_usuarios, @cedula, @fecha, @hora, @id_estacion, @id_linea, @orderNumber", parameters);
    }
}
