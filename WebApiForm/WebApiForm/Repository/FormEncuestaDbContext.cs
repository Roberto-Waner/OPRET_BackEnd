using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiForm.Repository.Models;
using WebApiForm.Services.DTO__Data_Transfer_Object_;
using WebApiForm.Services;

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

    public virtual DbSet<Estacion> Estacions { get; set; }

    public virtual DbSet<Formulario> Formularios { get; set; }

    public virtual DbSet<Linea> Lineas { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<RegistroUsuario> RegistroUsuarios { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Sesion> Sesions { get; set; }

    public virtual DbSet<SubPregunta> SubPreguntas { get; set; }

    public DbSet<PreguntaCompleta> PreguntaCompletas { get; set; }

    public DbSet<EstacionPorLinea> EstacionPorLineas { get; set; }

    public DbSet<ObtenerEmpleados> FiltrarUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estacion>(entity =>
        {
            entity.HasKey(e => e.IdEstacion).HasName("PK__Estacion__1F3B45EBAEDC364A");

            entity.Property(e => e.IdEstacion).ValueGeneratedNever();

            entity.HasOne(d => d.IdLineaNavigation).WithMany(p => p.Estacions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Estacion_linea");
        });

        modelBuilder.Entity<Formulario>(entity =>
        {
            entity.HasKey(e => e.IdentifacadorForm).HasName("PK__Formular__6CDA1CA2297646DD");

            entity.HasOne(d => d.IdEstacionNavigation).WithMany(p => p.Formularios).HasConstraintName("fk_Formulario_Estacion");

            entity.HasOne(d => d.IdLineaNavigation).WithMany(p => p.Formularios).HasConstraintName("fk_Formulario_Linea");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Formularios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_User_Form");
        });

        modelBuilder.Entity<Linea>(entity =>
        {
            entity.HasKey(e => e.IdLinea).HasName("PK__Linea__E346BA1903503E96");
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.CodPregunta).HasName("PK__Pregunta__9277FCFEA889D975");

            entity.Property(e => e.CodPregunta).ValueGeneratedNever();
        });

        modelBuilder.Entity<RegistroUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Registro__854B73B3E3501785");
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

        modelBuilder.Entity<PreguntaCompleta>().HasNoKey();
        modelBuilder.Entity<EstacionPorLinea>().HasNoKey();
        modelBuilder.Entity<ObtenerEmpleados>().HasNoKey();

        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public async Task<List<PreguntaCompleta>> GetPreguntasCompleto()
    {
        return await this.PreguntaCompletas.FromSqlRaw("EXEC sp_ObtenerPreguntasCompleto").ToListAsync();
    }

    public async Task<List<EstacionPorLinea>> GetEstacionPorLineas(string idLinea)
    {
        return await this.EstacionPorLineas.FromSqlRaw("EXEC sp_ObternerEstacionesPorLinea @idLinea = {0}", idLinea).ToListAsync();
    }

    public async Task<List<ObtenerEmpleados>> ObtenerEmpleadosAsync()
    {
        return await this.FiltrarUsuarios.FromSqlRaw("EXEC sp_ObtenerEmpleados").ToListAsync();
    }
}
