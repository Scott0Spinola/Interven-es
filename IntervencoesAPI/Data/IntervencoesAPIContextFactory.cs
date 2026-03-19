using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IntervencoesAPI.Data;

public sealed class IntervencoesAPIContextFactory : IDesignTimeDbContextFactory<IntervencoesAPIContext>
{
    public IntervencoesAPIContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Missing ConnectionStrings:DefaultConnection. Configure it in appsettings.json or via environment variables.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<IntervencoesAPIContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new IntervencoesAPIContext(optionsBuilder.Options);
    }
}
