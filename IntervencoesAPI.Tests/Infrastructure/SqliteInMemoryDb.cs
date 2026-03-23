using IntervencoesAPI.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Tests.Infrastructure;

public sealed class SqliteInMemoryDb : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteInMemoryDb()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<IntervencoesAPIContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new IntervencoesAPIContext(options);
        context.Database.EnsureCreated();
    }

    public IntervencoesAPIContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<IntervencoesAPIContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        return new IntervencoesAPIContext(options);
    }

    public ValueTask DisposeAsync()
    {
        _connection.Dispose();
        return ValueTask.CompletedTask;
    }
}
