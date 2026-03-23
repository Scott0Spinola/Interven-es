using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using IntervencoesAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace IntervencoesAPI.Tests;

public class ProcessoProjectoServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsProcessoProjecto_AndCanQueryByIdAndReferencia()
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

        var cliente = new Cliente
        {
            IdEntidade = entidade.Id,
            Referencia = "C-1",
            Observacoes = "Obs",
            Estado = 1,
            NProcesso = "NP",
            DataDeInicio = DateTime.UtcNow,
            DataActualizacao = DateTime.UtcNow,
            CliCampo1 = 1,
            CliCampo2 = 2,
            CliCampo3 = "A",
            CliCampo4 = "B",
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var service = new ProcessoProjectoService(context, NullLogger<ProcessoProjectoService>.Instance);

        var dto = new CreateProcessoProjecto(
            NumArquivo: "NA",
            Referencia: "P-REF",
            Estado: 1,
            DataInicio: new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            DataPrevistaConclusao: new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
            EsforcoPrevisto: 10,
            EsforcoReal: 0,
            ProcessoPaiId: 0,
            AvencaId: 0,
            ClienteId: cliente.Id,
            FornecedorId: 0,
            Descricao: "Desc",
            Fornecedores: "Forn",
            Responsavel: "Resp",
            IdProposta: 0,
            IdContracto: 0);

        var created = await service.CreateAsync(dto);

        Assert.True(created.Id > 0);
        Assert.Equal("P-REF", created.Referencia);
        Assert.Equal(cliente.Id, created.ClienteId);

        var byId = service.GetByIdProcessoProjecto(created.Id);
        Assert.NotNull(byId);
        Assert.Equal(created.Id, byId!.Id);

        var byRef = service.GetByRefencia("P-REF");
        Assert.NotNull(byRef);
        Assert.Equal(created.Id, byRef!.Id);

        var byClient = service.GetByIdClient(cliente.Id);
        Assert.NotNull(byClient);
        Assert.Equal(created.Id, byClient!.Id);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ReturnsNull()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new ProcessoProjectoService(context, NullLogger<ProcessoProjectoService>.Instance);

        var dto = new UpdateProcessoProjecto(
            NumArquivo: "NA",
            Referencia: "P-REF",
            Estado: 1,
            DataInicio: DateTime.UtcNow,
            DataPrevistaConclusao: DateTime.UtcNow,
            DataConclusao: DateTime.UtcNow,
            EsforcoPrevisto: 1,
            EsforcoReal: 1,
            ProcessoPaiId: 0,
            AvencaId: 0,
            ClienteId: 1,
            FornecedorId: 0,
            Descricao: "D",
            Fornecedores: "F",
            Responsavel: "R",
            IdProposta: 0,
            IdContracto: 0);

        var updated = await service.UpdateAsync(999, dto);

        Assert.Null(updated);
    }

    [Fact]
    public async Task Delete_WhenMissing_ReturnsFalse()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new ProcessoProjectoService(context, NullLogger<ProcessoProjectoService>.Instance);

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

        var cliente = new Cliente
        {
            IdEntidade = entidade.Id,
            Referencia = "C-1",
            Observacoes = "Obs",
            Estado = 1,
            NProcesso = "NP",
            DataDeInicio = DateTime.UtcNow,
            DataActualizacao = DateTime.UtcNow,
            CliCampo1 = 1,
            CliCampo2 = 2,
            CliCampo3 = "A",
            CliCampo4 = "B",
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        for (var i = 1; i <= 25; i++)
        {
            context.ProcessoProjectos.Add(new ProcessoProjecto
            {
                NumArquivo = "NA",
                Referencia = $"P-{i:00}",
                Estado = 1,
                DataInicio = DateTime.UtcNow,
                DataPrevistaConclusao = DateTime.UtcNow,
                EsforcoPrevisto = 1,
                EsforcoReal = 1,
                ProcessoPaiId = 0,
                AvencaId = 0,
                ClienteId = cliente.Id,
                FornecedorId = 0,
                Descricao = "D",
                Fornecedores = "F",
                Responsavel = "R",
                IdProposta = 0,
                IdContracto = 0,
            });
        }
        await context.SaveChangesAsync();

        var service = new ProcessoProjectoService(context, NullLogger<ProcessoProjectoService>.Instance);

        var page2 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 2, PageSize = 10 });
        Assert.Equal(25, page2.TotalCount);
        Assert.Equal(10, page2.Items.Count);

        var page3 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 3, PageSize = 10 });
        Assert.Equal(5, page3.Items.Count);
    }
}
