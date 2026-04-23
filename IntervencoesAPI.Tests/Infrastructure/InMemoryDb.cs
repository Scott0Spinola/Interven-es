using IntervencoesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntervencoesAPI.Tests.Infrastructure;

public sealed class InMemoryDb : IAsyncDisposable
{
    private readonly string _databaseName = $"IntervencoesAPI.Tests-{Guid.NewGuid()}";
    private readonly InMemoryDatabaseRoot _root = new();

    public IntervencoesAPIContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<IntervencoesAPIContext>()
            .UseInMemoryDatabase(_databaseName, _root)
            .EnableSensitiveDataLogging()
            .Options;

        return new IntervencoesAPIContext(options);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
