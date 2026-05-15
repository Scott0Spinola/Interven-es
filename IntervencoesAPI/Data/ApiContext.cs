using Microsoft.EntityFrameworkCore;
using IntervencoesAPI.Models;

namespace IntervencoesAPI.Data;

public class IntervencoesAPIContext(DbContextOptions<IntervencoesAPIContext> options) : DbContext(options)
{
    public DbSet<Entidade> Entidades => Set<Entidade>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<ProcessoProjecto> ProcessoProjectos => Set<ProcessoProjecto>();

    public DbSet<Intervencao> Intervencaos => Set<Intervencao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Intervencao>()
            .Property(i => i.PrevisaoEsforco)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Intervencao>()
            .Property(i => i.EsforcoReal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Intervencao>()
            .Property(i => i.EsforcoACobrar)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Intervencao>()
            .Property(i => i.Valor)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProcessoProjecto>()
            .Property(p => p.EsforcoPrevisto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProcessoProjecto>()
            .Property(p => p.EsforcoReal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Entidade)
            .WithMany(e => e.Clientes)
            .HasForeignKey(c => c.IdEntidade);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<ProcessoProjecto>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.ProcessoProjectos)
            .HasForeignKey(p => p.ClienteId);

        modelBuilder.Entity<Intervencao>()
      .HasOne(i => i.ProcessoProjecto)
      .WithMany(p => p.Intervencaos)
      .HasForeignKey(i => i.ProcessoId);
    }
}