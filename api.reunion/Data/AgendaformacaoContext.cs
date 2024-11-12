using api.reunion.Models;
using Microsoft.EntityFrameworkCore;

namespace api.reunion.Data;

/// <summary>
/// Ligação à BD feita no ficheiro Program.cs
/// Contexto da Base de dados, definindo as tabelas presentes na BD 
/// e gerindo as entidades e relações das tabelas.
/// </summary>
public partial class AgendaformacaoContext : DbContext
{
    public AgendaformacaoContext() { }

    public AgendaformacaoContext(DbContextOptions<AgendaformacaoContext> options)
        : base(options) { }

    public virtual DbSet<AdminGroup> AdminGroups { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Local> Locais { get; set; }

    public virtual DbSet<MarcacaoMaterial> MarcacaoMateriais { get; set; }

    public virtual DbSet<Marcacao> Marcacoes { get; set; }

    public virtual DbSet<Material> Materiais { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<SalaMaterial> SalaMateriais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("db_owner");

        modelBuilder.Entity<AdminGroup>(entity =>
        {
            entity.HasKey(e => e.AdminGroupId).HasName("PK__AdminGro__C4AE922724210324");

            entity.ToTable("AdminGroups", "dbo");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E5FD89A519");

            entity.ToTable("Categorias", "dbo");
        });

        modelBuilder.Entity<Local>(entity =>
        {
            entity.HasKey(e => e.LocalId).HasName("PK__Locais__499359BBFDED0AED");

            entity.ToTable("Locais", "dbo");

            entity.HasOne(d => d.AdminGroup).WithMany(p => p.Locais)
                .HasForeignKey(d => d.AdminGroupId)
                .HasConstraintName("FK_Locais_AdminGroup");
        });

        modelBuilder.Entity<MarcacaoMaterial>(entity =>
        {
            entity.HasKey(e => e.MarcacaoMateriaisId).HasName("PK__Marcacao__F6D382869ED3D5FE");

            entity.ToTable("MarcacaoMateriais", "dbo");

            entity.HasOne(d => d.Marcacao).WithMany(p => p.MarcacaoMateriais)
                .HasForeignKey(d => d.MarcacaoId)
                .HasConstraintName("FK_MarcacaoMateriais_Marcacao");

            entity.HasOne(d => d.Material).WithMany(p => p.MarcacaoMateriais)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK_MarcacaosMateriais_Material");
        });

        modelBuilder.Entity<Marcacao>(entity =>
        {
            entity.HasKey(e => e.MarcacaoId).HasName("PK__Marcacoe__EDD06DDCC50C3500");

            entity.ToTable("Marcacoes", "dbo");

            entity.Property(e => e.DataRegisto).HasColumnType("datetime");
            entity.Property(e => e.Estado).HasMaxLength(10);
            entity.Property(e => e.HoraFim).HasColumnName("Hora_Fim");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.ModificadoEm).HasColumnType("datetime");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Marcacoes)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marcacoes_Categoria");

            entity.HasOne(d => d.Local).WithMany(p => p.Marcacoes)
                .HasForeignKey(d => d.LocalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marcacoes_Locais");

            entity.HasOne(d => d.Sala).WithMany(p => p.Marcacoes)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Marcacoes_Sala");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Materiai__C50610F7931FBE14");

            entity.ToTable("Materiais", "dbo");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK__Salas__0428487AF6BD9689");

            entity.ToTable("Salas", "dbo");

            entity.Property(e => e.Cor).HasMaxLength(10);

            entity.HasOne(d => d.Local).WithMany(p => p.Salas)
                .HasForeignKey(d => d.LocalId)
                .HasConstraintName("FK_Salas_Locais");
        });

        modelBuilder.Entity<SalaMaterial>(entity =>
        {
            entity.HasKey(e => e.SalaMateriaisId).HasName("PK__SalaMate__CA1DB67731B4BF2E");

            entity.ToTable("SalaMateriais", "dbo");

            entity.HasOne(d => d.Material).WithMany(p => p.SalaMateriais)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK_SalaMateriais_Materiais");

            entity.HasOne(d => d.Sala).WithMany(p => p.SalaMateriais)
                .HasForeignKey(d => d.SalaId)
                .HasConstraintName("FK_SalaMateriais_Salas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
