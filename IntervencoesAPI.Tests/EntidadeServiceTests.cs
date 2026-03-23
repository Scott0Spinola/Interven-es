using IntervencoesAPI.Dtos;
using IntervencoesAPI.Dtos.EntidadeDtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using IntervencoesAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace IntervencoesAPI.Tests;

public class EntidadeServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsEntidade_AndSetsDataActualizacao()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new EntidadeService(context, NullLogger<EntidadeService>.Instance);

        var dto = new CreateEntidade(
            Referencia: "E-REF",
            NomeSocial: "Nome",
            Contribuinte: "123",
            Observacoes: "Obs",
            Tipo: 1,
            Estado: 2,
            Item1: true,
            Item2: 42,
            Item3: "X",
            DesignacaoComercial: "DC");

        var before = DateTime.UtcNow;
        var created = await service.CreateAsync(dto);
        var after = DateTime.UtcNow;

        Assert.True(created.Id > 0);
        Assert.Equal("E-REF", created.Referencia);
        Assert.InRange(created.DataActualizacao, before.AddSeconds(-1), after.AddSeconds(1));

        var fromDb = await context.Entidades.AsNoTracking().FirstOrDefaultAsync(e => e.Id == created.Id);
        Assert.NotNull(fromDb);
        Assert.Equal("Nome", fromDb!.NomeSocial);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ReturnsNull()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new EntidadeService(context, NullLogger<EntidadeService>.Instance);

        var dto = new UpdateEntidade(
            Referencia: "X",
            NomeSocial: null,
            Contribuinte: null,
            Observacoes: null,
            Tipo: 1,
            Estado: null,
            Item1: false,
            Item2: 0,
            Item3: null,
            DesignacaoComercial: null);

        var updated = await service.UpdateAsync(999, dto);

        Assert.Null(updated);
    }

    [Fact]
    public async Task Delete_WhenMissing_ReturnsFalse()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new EntidadeService(context, NullLogger<EntidadeService>.Instance);

        var deleted = service.Delete(123);

        Assert.False(deleted);
    }

    [Fact]
    public async Task GetAllPagedAsync_ReturnsPageAndTotalCount()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        for (var i = 1; i <= 25; i++)
        {
            context.Entidades.Add(new Entidade
            {
                Referencia = $"E-{i:00}",
                Tipo = 1,
                Item1 = false,
                Item2 = i,
                DataActualizacao = DateTime.UtcNow,
            });
        }
        await context.SaveChangesAsync();

        var service = new EntidadeService(context, NullLogger<EntidadeService>.Instance);

        var page2 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 2, PageSize = 10 });
        Assert.Equal(25, page2.TotalCount);
        Assert.Equal(10, page2.Items.Count);
        Assert.True(page2.HasNextPage);
        Assert.True(page2.HasPreviousPage);

        var page3 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 3, PageSize = 10 });
        Assert.Equal(5, page3.Items.Count);
        Assert.False(page3.HasNextPage);
        Assert.True(page3.HasPreviousPage);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsNestedGraph()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var entidade = new Entidade
        {
            Referencia = "E-1",
            NomeSocial = "Nome",
            Tipo = 1,
            Item1 = true,
            Item2 = 1,
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

        var processo = new ProcessoProjecto
        {
            NumArquivo = "NA",
            Referencia = "P-1",
            Estado = 1,
            DataInicio = DateTime.UtcNow,
            DataPrevistaConclusao = DateTime.UtcNow.AddDays(1),
            EsforcoPrevisto = 1,
            EsforcoReal = 2,
            ProcessoPaiId = 0,
            AvencaId = 0,
            ClienteId = cliente.Id,
            FornecedorId = 0,
            Descricao = "D",
            Fornecedores = "F",
            Responsavel = "R",
            IdProposta = 0,
            IdContracto = 0,
        };
        context.ProcessoProjectos.Add(processo);
        await context.SaveChangesAsync();

        var intervencao = new Intervencao
        {
            ProcessoId = processo.Id,
            TarefaId = 1,
            Autor = "Autor",
            Visibilidade = 1,
            Tipo = 10,
            Estado = 20,
            HistoricoEstados = "H",
            Prioridade = 1,
            DataRegisto = DateTime.UtcNow,
            DataLimite = DateTime.UtcNow,
            DataConclusao = DateTime.UtcNow,
            DataConfirmacao = DateTime.UtcNow,
            DataInstalacao = DateTime.UtcNow,
            Tema = "Tema",
            AccaoRealizada = "Accao",
            PrevisaoEsforco = 1,
            EsforcoReal = 1,
            Referencia = "I-1",
            Responsavel = "Resp",
            Coresponsavel = "Co",
            Descricao = null,
            Notas = "N",
            Comentarios = "C",
            IntervencaoPaiId = 0,
            EsforcoACobrar = 0,
            Valor = 0,
            DataCriacao = DateTime.UtcNow,
            DataQualidade = DateTime.UtcNow,
            UpdateUser = "U",
            UpdateDate = DateTime.UtcNow,
            Email = null,
            Codigo = null,
            DataInicio = null,
            TarefaAgendadaId = 0,
            DataInicioPrevista = DateTime.UtcNow,
            DataFimPrevista = DateTime.UtcNow,
            Alerta = 0,
            MotivoAlerta = "M",
        };
        context.Intervencaos.Add(intervencao);
        await context.SaveChangesAsync();

        var service = new EntidadeService(context, NullLogger<EntidadeService>.Instance);

        var details = await service.GetDetailsAsync(entidade.Id);

        Assert.NotNull(details);
        Assert.Equal(entidade.Id, details!.Id);
        Assert.Single(details.Clientes);
        Assert.Equal(cliente.Id, details.Clientes[0].Id);
        Assert.Single(details.Clientes[0].ProcessoProjectos);
        Assert.Equal(processo.Id, details.Clientes[0].ProcessoProjectos[0].Id);
        Assert.Single(details.Clientes[0].ProcessoProjectos[0].IntervencaoDtos);
        Assert.Equal(intervencao.Id, details.Clientes[0].ProcessoProjectos[0].IntervencaoDtos[0].Id);
    }
}
