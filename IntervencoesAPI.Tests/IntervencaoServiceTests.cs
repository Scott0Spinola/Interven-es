using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using IntervencoesAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace IntervencoesAPI.Tests;

public class IntervencaoServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsIntervencao_AndCanQueryByIdAndReferencia()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var (processoId, _) = await SeedProcessoAsync(context);

        var service = new IntervencaoService(context, NullLogger<IntervencaoService>.Instance);

        var dto = CreateDto(processoId, referencia: "I-REF", tipo: 7, estado: 3, dataCriacao: new DateTime(2026, 3, 20, 10, 0, 0, DateTimeKind.Utc));
        var created = await service.CreateAsync(dto);

        Assert.True(created.Id > 0);
        Assert.Equal("I-REF", created.Referencia);
        Assert.Equal(processoId, created.ProcessoId);

        var byId = service.GetByIdIntervencao(created.Id);
        Assert.NotNull(byId);

        var byRef = service.GetByReferencia("I-REF");
        Assert.NotNull(byRef);
        Assert.Equal(created.Id, byRef!.Id);

        var byTipo = service.GetByTipo(7);
        Assert.NotNull(byTipo);
        Assert.Equal(created.Id, byTipo!.Id);

        var byEstado = service.GetByEstado(3);
        Assert.NotNull(byEstado);
        Assert.Equal(created.Id, byEstado!.Id);

        var byProcesso = service.GetByIdProcesso(processoId);
        Assert.NotNull(byProcesso);
        Assert.Equal(created.Id, byProcesso!.Id);

        var fromDb = await context.Intervencaos.AsNoTracking().FirstOrDefaultAsync(i => i.Id == created.Id);
        Assert.NotNull(fromDb);
        Assert.Equal("Tema", fromDb!.Tema);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ReturnsNull()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var (processoId, _) = await SeedProcessoAsync(context);

        var service = new IntervencaoService(context, NullLogger<IntervencaoService>.Instance);

        var dto = new UpdateIntervencao(
            ProcessoId: processoId,
            TarefaId: 1,
            Autor: "Autor",
            Visibilidade: 1,
            Tipo: 1,
            Estado: 1,
            HistoricoEstados: "H",
            Prioridade: 1,
            DataRegisto: DateTime.UtcNow,
            DataLimite: DateTime.UtcNow,
            DataConclusao: DateTime.UtcNow,
            DataConfirmacao: DateTime.UtcNow,
            DataInstalacao: DateTime.UtcNow,
            Tema: "Tema",
            AccaoRealizada: "Accao",
            PrevisaoEsforco: 1,
            EsforcoReal: 1,
            Referencia: "Ref",
            Responsavel: "Resp",
            Coresponsavel: "Co",
            Descricao: null,
            Notas: "N",
            Comentarios: "C",
            IntervencaoPaiId: 0,
            EsforcoACobrar: 0,
            Valor: 0,
            DataCriacao: DateTime.UtcNow,
            DataQualidade: DateTime.UtcNow,
            UpdateUser: "U",
            UpdateDate: DateTime.UtcNow,
            Email: null,
            Codigo: null,
            DataInicio: null,
            TarefaAgendadaId: 0,
            DataInicioPrevista: DateTime.UtcNow,
            DataFimPrevista: DateTime.UtcNow,
            Alerta: 0,
            MotivoAlerta: "M");

        var updated = await service.UpdateAsync(999, dto);

        Assert.Null(updated);
    }

    [Fact]
    public async Task Delete_WhenMissing_ReturnsFalse()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var service = new IntervencaoService(context, NullLogger<IntervencaoService>.Instance);

        var deleted = service.Delete(123);

        Assert.False(deleted);
    }

    [Fact]
    public async Task GetByIntervaloDataCriacaoAsync_FiltersByDateRange()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var (processoId, _) = await SeedProcessoAsync(context);

        var service = new IntervencaoService(context, NullLogger<IntervencaoService>.Instance);

        var inside = await service.CreateAsync(CreateDto(processoId, "I-IN", 1, 1, new DateTime(2026, 3, 20, 12, 0, 0, DateTimeKind.Utc)));
        _ = await service.CreateAsync(CreateDto(processoId, "I-OUT", 1, 1, new DateTime(2026, 3, 10, 12, 0, 0, DateTimeKind.Utc)));

        var results = await service.GetByIntervaloDataCriacaoAsync(
            start: new DateTime(2026, 3, 19, 0, 0, 0, DateTimeKind.Utc),
            end: new DateTime(2026, 3, 21, 0, 0, 0, DateTimeKind.Utc));

        Assert.Single(results);
        Assert.Equal(inside.Id, results[0].Id);

        var sameDay = await service.GetByIntervelDataDeCriacao(new DateTime(2026, 3, 20, 8, 0, 0, DateTimeKind.Utc));
        Assert.Single(sameDay);
        Assert.Equal(inside.Id, sameDay[0].Id);
    }

    [Fact]
    public async Task GetAllPagedAsync_ReturnsPageAndTotalCount()
    {
        await using var db = new SqliteInMemoryDb();
        await using var context = db.CreateContext();

        var (processoId, _) = await SeedProcessoAsync(context);

        for (var i = 1; i <= 25; i++)
        {
            context.Intervencaos.Add(new Intervencao
            {
                ProcessoId = processoId,
                TarefaId = 1,
                Autor = "Autor",
                Visibilidade = 1,
                Tipo = 1,
                Estado = 1,
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
                Referencia = $"I-{i:00}",
                Responsavel = "Resp",
                Coresponsavel = "Co",
                Descricao = null,
                Notas = "N",
                Comentarios = "C",
                IntervencaoPaiId = 0,
                EsforcoACobrar = 0,
                Valor = 0,
                DataCriacao = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(i),
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
            });
        }
        await context.SaveChangesAsync();

        var service = new IntervencaoService(context, NullLogger<IntervencaoService>.Instance);

        var page2 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 2, PageSize = 10 });
        Assert.Equal(25, page2.TotalCount);
        Assert.Equal(10, page2.Items.Count);

        var page3 = await service.GetAllPagedAsync(new PageParameters { PageNumber = 3, PageSize = 10 });
        Assert.Equal(5, page3.Items.Count);
    }

    private static CreateIntervencao CreateDto(int processoId, string referencia, int tipo, int estado, DateTime dataCriacao)
    {
        var now = DateTime.UtcNow;

        return new CreateIntervencao(
            ProcessoId: processoId,
            TarefaId: 1,
            Autor: "Autor",
            Visibilidade: 1,
            Tipo: tipo,
            Estado: estado,
            HistoricoEstados: "H",
            Prioridade: 1,
            DataRegisto: now,
            DataLimite: now,
            DataConclusao: now,
            DataConfirmacao: now,
            DataInstalacao: now,
            Tema: "Tema",
            AccaoRealizada: "Accao",
            PrevisaoEsforco: 1,
            EsforcoReal: 1,
            Referencia: referencia,
            Responsavel: "Resp",
            Coresponsavel: "Co",
            Descricao: null,
            Notas: "N",
            Comentarios: "C",
            IntervencaoPaiId: 0,
            EsforcoACobrar: 0,
            Valor: 0,
            DataCriacao: dataCriacao,
            DataQualidade: now,
            UpdateUser: "U",
            UpdateDate: now,
            Email: null,
            Codigo: null,
            DataInicio: null,
            TarefaAgendadaId: 0,
            DataInicioPrevista: now,
            DataFimPrevista: now,
            Alerta: 0,
            MotivoAlerta: "M");
    }

    private static async Task<(int processoId, int clienteId)> SeedProcessoAsync(IntervencoesAPI.Data.IntervencoesAPIContext context)
    {
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

        var processo = new ProcessoProjecto
        {
            NumArquivo = "NA",
            Referencia = "P-1",
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
        };
        context.ProcessoProjectos.Add(processo);
        await context.SaveChangesAsync();

        return (processo.Id, cliente.Id);
    }
}
