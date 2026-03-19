using Microsoft.EntityFrameworkCore;
using IntervencoesAPI.Models;

namespace IntervencoesAPI.Data;

public class IntervencoesAPIContext(DbContextOptions<IntervencoesAPIContext> options ) : DbContext (options)
{
    public DbSet<Entidade> Entidades => Set<Entidade>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<ProcessoProjecto> ProcessoProjectos => Set<ProcessoProjecto>();

    public DbSet<Intervencao> Intervencaos => Set<Intervencao>();

}