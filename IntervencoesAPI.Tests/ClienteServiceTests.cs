using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using IntervencoesAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace IntervencoesAPI.Tests;

public class ClienteServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsCliente_AndSetsTimestamps()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var entidade = new Entidade
        {
            Referencia = "E-1",
            Tipo = 1,
            Item1 = false,
            Item2 = 0,
            DataActualizacao = DateTime.UtcNow,
        };
        context.Entidades.Add(entidade);
        await context.SaveChangesAsync();

        var service = new ClienteService(context, NullLogger<ClienteService>.Instance);

        var dto = new CreateCliente(
            IdEntidade: entidade.Id,
            Referencia: "C-REF",
            Observacoes: "Obs",
            Estado: 1,
            NProcesso: "NP1",
            CliCampo1: 10,
            CliCampo2: 20,
            CliCampo3: "A",
            CliCampo4: "B");

        var before = DateTime.UtcNow;
        var created = await service.CreateAsync(dto);
        var after = DateTime.UtcNow;

        Assert.True(created.Id > 0);
        Assert.Equal(entidade.Id, created.IdEntidade);
        Assert.InRange(created.DataDeInicio, before.AddSeconds(-1), after.AddSeconds(1));
        Assert.InRange(created.DataActualizacao, before.AddSeconds(-1), after.AddSeconds(1));

        var fromDb = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == created.Id);
        Assert.NotNull(fromDb);
        Assert.Equal("C-REF", fromDb!.Referencia);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ReturnsNull()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new ClienteService(context, NullLogger<ClienteService>.Instance);

        var dto = new UpdateCliente(
            IdEntidade: 1,
            Referencia: "X",
            Observacoes: "Y",
            Estado: 1,
            NProcesso: "NP1",
            CliCampo1: 1,
            CliCampo2: 2,
            CliCampo3: "A",
            CliCampo4: "B");

        var updated = await service.UpdateAsync(999, dto);

        Assert.Null(updated);
    }

    [Fact]
    public async Task Delete_WhenMissing_ReturnsFalse()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new ClienteService(context, NullLogger<ClienteService>.Instance);

        var deleted = service.Delete(123);

        Assert.False(deleted);
    }

    [Fact]
    public async Task GetAllPagedAsync_ReturnsPageAndTotalCount()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var entidade = new Entidade
        {
            Referencia = "E-1",
            Tipo = 1,
            Item1 = false,
            Item2 = 0,
            DataActualizacao = DateTime.UtcNow,
        };
        context.Entidades.Add(entidade);
        await context.SaveChangesAsync();

        for (var i = 1; i <= 25; i++)
        {
            context.Clientes.Add(new Cliente
            {
                IdEntidade = entidade.Id,
                Referencia = $"C-{i:00}",
                Observacoes = "Obs",
                Estado = 1,
                NProcesso = "NP",
                DataDeInicio = DateTime.UtcNow,
                DataActualizacao = DateTime.UtcNow,
                CliCampo1 = 1,
                CliCampo2 = 2,
                CliCampo3 = "A",
                CliCampo4 = "B",
            });
        }
        await context.SaveChangesAsync();

        var service = new ClienteService(context, NullLogger<ClienteService>.Instance);

        var page2 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 2, PageSize = 10 });
        Assert.Equal(25, page2.TotalCount);
        Assert.Equal(10, page2.Items.Count);
        Assert.Equal(2, page2.Page);
        Assert.Equal(10, page2.PageSize);
        Assert.True(page2.HasNextPage);
        Assert.True(page2.HasPreviousPage);

        var page3 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 3, PageSize = 10 });
        Assert.Equal(25, page3.TotalCount);
        Assert.Equal(5, page3.Items.Count);
        Assert.False(page3.HasNextPage);
        Assert.True(page3.HasPreviousPage);
    }
}
