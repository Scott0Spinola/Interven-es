using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Services;

/// <summary>
/// Service layer for CRUD and query operations over <see cref="Intervencao"/>.
/// </summary>
/// <remarks>
/// This service encapsulates Entity Framework Core access to <see cref="IntervencoesAPIContext"/>.
/// </remarks>
public class IntervencaoService
{
    private readonly IntervencoesAPIContext _context;

    private readonly ILogger<IntervencaoService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntervencaoService"/> class.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    /// <param name="logger">The logger instance.</param>
    public IntervencaoService(IntervencoesAPIContext context, ILogger<IntervencaoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets all intervenções ordered by identifier.
    /// </summary>
    /// <returns>A list of all <see cref="Intervencao"/> records.</returns>
    public List<Intervencao> GetAll()
    {
        try
        {
            return _context.Intervencaos.OrderBy(i => i.Id).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetAll));
            throw;
        }
    }

    /// <summary>
    /// Gets a paginated list of intervenções ordered by identifier.
    /// </summary>
    /// <param name="pageParameters">The pagination parameters (page number and page size).</param>
    /// <returns>A <see cref="PagedList{T}"/> containing the requested page.</returns>
    /// <remarks>
    /// The query uses <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/> for read-only performance.
    /// </remarks>
    public async Task<PagedList<Intervencao>> GetAllPagedAsync(PageParameters pageParameters)
    {
        try
        {
            var query = _context.Intervencaos
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .AsQueryable();

            return await PagedList<Intervencao>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetAllPagedAsync));
            throw;
        }
    }

    /// <summary>
    /// Gets an intervenção by identifier.
    /// </summary>
    /// <param name="id">The intervenção identifier.</param>
    /// <returns>The matching <see cref="Intervencao"/>, or <see langword="null"/> if not found.</returns>
    public Intervencao? GetByIdIntervencao(int id)
    {
        try
        {
            return _context.Intervencaos.FirstOrDefault(i => i.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByIdIntervencao));
            throw;
        }
    }

    /// <summary>
    /// Gets an intervenção by reference (referência).
    /// </summary>
    /// <param name="referencia">The reference value to search for.</param>
    /// <returns>The matching <see cref="Intervencao"/>, or <see langword="null"/> if not found.</returns>
    public Intervencao? GetByReferencia(string referencia)
    {
        try
        {
            return _context.Intervencaos.FirstOrDefault(r => r.Referencia == referencia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByReferencia));
            throw;
        }
    }

    /// <summary>
    /// Gets the first intervenção with the provided type.
    /// </summary>
    /// <param name="tipo">The type value to search for.</param>
    /// <returns>The first matching <see cref="Intervencao"/>, or <see langword="null"/> if not found.</returns>
    public Intervencao? GetByTipo(int tipo)
    {
        try
        {
            return _context.Intervencaos.FirstOrDefault(t => t.Tipo == tipo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByTipo));
            throw;
        }
    }

    /// <summary>
    /// Gets the first intervenção with the provided state.
    /// </summary>
    /// <param name="estado">The state value to search for.</param>
    /// <returns>The first matching <see cref="Intervencao"/>, or <see langword="null"/> if not found.</returns>
    public Intervencao? GetByEstado(int estado)
    {
        try
        {
            return _context.Intervencaos.FirstOrDefault(e => e.Estado == estado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByEstado));
            throw;
        }
    }

    /// <summary>
    /// Gets intervenções whose creation date is within the provided interval.
    /// </summary>
    /// <param name="start">Interval start (inclusive).</param>
    /// <param name="end">Interval end (inclusive).</param>
    /// <returns>A list of matching <see cref="Intervencao"/> records ordered by identifier.</returns>
    /// <remarks>
    /// This method performs a read-only query (<c>AsNoTracking</c>).
    /// </remarks>
    public async Task<List<Intervencao>> GetByIntervaloDataCriacaoAsync(DateTime start, DateTime end)
    {
        try
        {
            return await _context.Intervencaos
                .AsNoTracking()
                .Where(i => i.DataCriacao >= start && i.DataCriacao <= end)
                .OrderBy(i => i.Id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByIntervaloDataCriacaoAsync));
            throw;
        }
    }

    /// <summary>
    /// Gets intervenções created on the same calendar date as <paramref name="dataCriacao"/>.
    /// </summary>
    /// <param name="dataCriacao">The date whose day range should be queried.</param>
    /// <returns>A list of matching <see cref="Intervencao"/> records.</returns>
    /// <remarks>
    /// Internally computes the day interval <c>[00:00:00, 23:59:59.9999999]</c> for <paramref name="dataCriacao"/>'s date.
    /// Note: method name is preserved as-is to avoid breaking callers.
    /// </remarks>
    public Task<List<Intervencao>> GetByIntervelDataDeCriacao(DateTime dataCriacao)
    {
        try
        {
            var start = dataCriacao.Date;
            var end = start.AddDays(1).AddTicks(-1);
            return GetByIntervaloDataCriacaoAsync(start, end);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByIntervelDataDeCriacao));
            throw;
        }
    }

    /// <summary>
    /// Gets the first intervenção associated with a given processo identifier.
    /// </summary>
    /// <param name="idProcesso">The processo identifier to search for.</param>
    /// <returns>The first matching <see cref="Intervencao"/>, or <see langword="null"/> if none exist.</returns>
    public Intervencao? GetByIdProcesso(int idProcesso)
    {
        try
        {
            return _context.Intervencaos.FirstOrDefault(p => p.ProcessoId == idProcesso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByIdProcesso));
            throw;
        }
    }

    /*
    public Intervencao? GetByIdCliente(int idCliente)
    {
        return _context.Intervencaos.FirstOrDefault(p => p.id ==idCliente);
    }
    */

    /// <summary>
    /// Creates a new intervenção.
    /// </summary>
    /// <param name="dto">The DTO containing creation data.</param>
    /// <returns>The created <see cref="Intervencao"/> (including its generated identifier).</returns>
    public async Task<Intervencao> CreateAsync(CreateIntervencao dto)
    {
        try
        {
            var intervencao = new Intervencao
            {
                ProcessoId = dto.ProcessoId,
                TarefaId = dto.TarefaId,
                Autor = dto.Autor,
                Visibilidade = dto.Visibilidade,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                HistoricoEstados = dto.HistoricoEstados,
                Prioridade = dto.Prioridade,
                DataRegisto = dto.DataRegisto,
                DataLimite = dto.DataLimite,
                DataConclusao = dto.DataConclusao,
                DataConfirmacao = dto.DataConfirmacao,
                DataInstalacao = dto.DataInstalacao,
                Tema = dto.Tema,
                AccaoRealizada = dto.AccaoRealizada,
                PrevisaoEsforco = dto.PrevisaoEsforco,
                EsforcoReal = dto.EsforcoReal,
                Referencia = dto.Referencia,
                Responsavel = dto.Responsavel,
                Coresponsavel = dto.Coresponsavel,
                Descricao = dto.Descricao,
                Notas = dto.Notas,
                Comentarios = dto.Comentarios,
                IntervencaoPaiId = dto.IntervencaoPaiId,
                EsforcoACobrar = dto.EsforcoACobrar,
                Valor = dto.Valor,
                DataCriacao = dto.DataCriacao,
                DataQualidade = dto.DataQualidade,
                UpdateUser = dto.UpdateUser,
                UpdateDate = dto.UpdateDate,
                Email = dto.Email,
                Codigo = dto.Codigo,
                DataInicio = dto.DataInicio,
                TarefaAgendadaId = dto.TarefaAgendadaId,
                DataInicioPrevista = dto.DataInicioPrevista,
                DataFimPrevista = dto.DataFimPrevista,
                Alerta = dto.Alerta,
                MotivoAlerta = dto.MotivoAlerta,
            };

            _context.Intervencaos.Add(intervencao);
            await _context.SaveChangesAsync();
            return intervencao;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "Operation Canceled - {Method}", nameof(CreateAsync));
            throw;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency error in {Method}", nameof(CreateAsync));
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Database update error in {Method}", nameof(CreateAsync));
            throw;
        }
    }

    /// <summary>
    /// Updates an existing intervenção.
    /// </summary>
    /// <param name="id">The intervenção identifier.</param>
    /// <param name="dto">The DTO containing update data.</param>
    /// <returns>
    /// The updated <see cref="Intervencao"/>, or <see langword="null"/> if no record exists with the provided identifier.
    /// </returns>
    public async Task<Intervencao?> UpdateAsync(int id, UpdateIntervencao dto)
    {
        try
        {
            var intervencao = _context.Intervencaos.FirstOrDefault(i => i.Id == id);

            if (intervencao is null)
            {
                return null;
            }

            intervencao.ProcessoId = dto.ProcessoId;
            intervencao.TarefaId = dto.TarefaId;
            intervencao.Autor = dto.Autor;
            intervencao.Visibilidade = dto.Visibilidade;
            intervencao.Tipo = dto.Tipo;
            intervencao.Estado = dto.Estado;
            intervencao.HistoricoEstados = dto.HistoricoEstados;
            intervencao.Prioridade = dto.Prioridade;
            intervencao.DataRegisto = dto.DataRegisto;
            intervencao.DataLimite = dto.DataLimite;
            intervencao.DataConclusao = dto.DataConclusao;
            intervencao.DataConfirmacao = dto.DataConfirmacao;
            intervencao.DataInstalacao = dto.DataInstalacao;
            intervencao.Tema = dto.Tema;
            intervencao.AccaoRealizada = dto.AccaoRealizada;
            intervencao.PrevisaoEsforco = dto.PrevisaoEsforco;
            intervencao.EsforcoReal = dto.EsforcoReal;
            intervencao.Referencia = dto.Referencia;
            intervencao.Responsavel = dto.Responsavel;
            intervencao.Coresponsavel = dto.Coresponsavel;
            intervencao.Descricao = dto.Descricao;
            intervencao.Notas = dto.Notas;
            intervencao.Comentarios = dto.Comentarios;
            intervencao.IntervencaoPaiId = dto.IntervencaoPaiId;
            intervencao.EsforcoACobrar = dto.EsforcoACobrar;
            intervencao.Valor = dto.Valor;
            intervencao.DataCriacao = dto.DataCriacao;
            intervencao.DataQualidade = dto.DataQualidade;
            intervencao.UpdateUser = dto.UpdateUser;
            intervencao.UpdateDate = dto.UpdateDate;
            intervencao.Email = dto.Email;
            intervencao.Codigo = dto.Codigo;
            intervencao.DataInicio = dto.DataInicio;
            intervencao.TarefaAgendadaId = dto.TarefaAgendadaId;
            intervencao.DataInicioPrevista = dto.DataInicioPrevista;
            intervencao.DataFimPrevista = dto.DataFimPrevista;
            intervencao.Alerta = dto.Alerta;
            intervencao.MotivoAlerta = dto.MotivoAlerta;

            await _context.SaveChangesAsync();
            return intervencao;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency error in {Method}", nameof(UpdateAsync));
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Database update error in {Method}", nameof(UpdateAsync));
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method} id={Id}", nameof(UpdateAsync), id);
            throw;
        }
    }

    /// <summary>
    /// Deletes an existing intervenção.
    /// </summary>
    /// <param name="id">The intervenção identifier.</param>
    /// <returns><see langword="true"/> if deleted; otherwise <see langword="false"/> if not found.</returns>
    public bool Delete(int id)
    {
        try
        {
            var intervencao = _context.Intervencaos.FirstOrDefault(i => i.Id == id);
            if (intervencao is null)
            {
                return false;
            }

            _context.Intervencaos.Remove(intervencao);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(Delete));
            throw;
        }
    }
}