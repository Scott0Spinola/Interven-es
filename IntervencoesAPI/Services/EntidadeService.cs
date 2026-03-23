using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Dtos.EntidadeDtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;


namespace IntervencoesAPI.Services;

/// <summary>
/// Provides data access and business operations for <see cref="Entidade"/>.
/// </summary>
public class EntidadeService
{
    private readonly IntervencoesAPIContext _context;

    private readonly ILogger<EntidadeService> _logger;

    public EntidadeService(IntervencoesAPIContext context, ILogger<EntidadeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets all entidades ordered by identifier.
    /// </summary>
    /// <returns>A list of entidades.</returns>
    public List<Entidade> GetAll()
    {
        try
        {
            return _context.Entidades.OrderBy(i => i.Id).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetAll));
            throw;
        }
    }

    /// <summary>
    /// Gets a paginated list of entidades.
    /// </summary>
    /// <param name="pageParameters">Pagination parameters (page number and page size).</param>
    /// <returns>A paged list containing the requested page of entidades.</returns>
    public async Task<PagedList<Entidade>> GetAllPagedAsync(PageParameters pageParameters)
    {
        try
        {
            var query = _context.Entidades
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .AsQueryable();

            return await PagedList<Entidade>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetAllPagedAsync));
            throw;
        }
    }

    /// <summary>
    /// Gets an entidade by nome social.
    /// </summary>
    /// <param name="name">The nome social to search for.</param>
    /// <returns>The entidade if found; otherwise <see langword="null"/>.</returns>
    public Entidade? GetByNomeSocial(string name)
    {
        try
        {
            return _context.Entidades.FirstOrDefault(n => n.NomeSocial == name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByNomeSocial));
            throw;
        }
    }

    /// <summary>
    /// Gets an entidade by referencia.
    /// </summary>
    /// <param name="referncia">The referencia to search for.</param>
    /// <returns>The entidade if found; otherwise <see langword="null"/>.</returns>
     public Entidade? GetByReferencia(string referncia)
    {
        try
        {
            return _context.Entidades.FirstOrDefault(n => n.Referencia == referncia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByReferencia));
            throw;
        }
    }

    /// <summary>
    /// Gets an entidade by identifier.
    /// </summary>
    /// <param name="id">The entidade identifier.</param>
    /// <returns>The entidade if found; otherwise <see langword="null"/>.</returns>
    public Entidade? GetByIdEntidade(int id)
    {
        try
        {
            return _context.Entidades.FirstOrDefault(i => i.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetByIdEntidade));
            throw;
        }
    }

    /// <summary>
    /// Creates a new entidade.
    /// </summary>
    /// <param name="dto">The data used to create the entidade.</param>
    /// <returns>The created entidade with its generated identifier.</returns>
    public async Task<Entidade> CreateAsync(CreateEntidade dto)
    {
        try
        {
            var entidade = new Entidade
            {
                Referencia = dto.Referencia,
                NomeSocial = dto.NomeSocial,
                Contribuinte = dto.Contribuinte,
                Observacoes = dto.Observacoes,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                Item1 = dto.Item1,
                Item2 = dto.Item2,
                Item3 = dto.Item3,
                DesignacaoComercial = dto.DesignacaoComercial,
                DataActualizacao = DateTime.UtcNow,
            };

            _context.Entidades.Add(entidade);
            await _context.SaveChangesAsync();

            return entidade;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(CreateAsync));
            throw;
        }
    }


    /// <summary>
    /// Updates an existing entidade.
    /// </summary>
    /// <param name="id">The entidade identifier.</param>
    /// <param name="dto">The data used to update the entidade.</param>
    /// <returns>The updated entidade if found; otherwise <see langword="null"/>.</returns>
    public async Task<Entidade?> UpdateAsync(int id, UpdateEntidade dto)
    {
        try
        {
            var entidade = _context.Entidades.FirstOrDefault(i => i.Id == id);

            if (entidade is null)
            {
                return null;
            }
            entidade.Referencia = dto.Referencia;
            entidade.NomeSocial = dto.NomeSocial;
            entidade.Contribuinte = dto.Contribuinte;
            entidade.Observacoes = dto.Observacoes;
            entidade.Tipo = dto.Tipo;
            entidade.Estado = dto.Estado;
            entidade.Item1 = dto.Item1;
            entidade.Item2 = dto.Item2;
            entidade.Item3 = dto.Item3;
            entidade.DesignacaoComercial = dto.DesignacaoComercial;
            entidade.DataActualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return entidade;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(UpdateAsync));
            throw;
        }
    }



    /// <summary>
    /// Deletes an entidade.
    /// </summary>
    /// <param name="id">The entidade identifier.</param>
    /// <returns><see langword="true"/> if deleted; <see langword="false"/> if not found.</returns>
    public bool Delete(int id)
    {
        try
        {
            var entidade = _context.Entidades.FirstOrDefault(i => i.Id == id);
            if (entidade is null)
            {
                return false;
            }
            _context.Entidades.Remove(entidade);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(Delete));
            throw;
        }
    }


    /// <summary>
    /// Gets an aggregated details DTO for an entidade.
    /// </summary>
    /// <param name="entidadeId">The entidade identifier.</param>
    /// <remarks>
    /// Loads related data (clientes, processos, and intervenções) and maps it into <see cref="EntidadeDetailsDto"/>.
    /// </remarks>
    /// <returns>The details DTO if found; otherwise <see langword="null"/>.</returns>
    public async Task<EntidadeDetailsDto?> GetDetailsAsync(int entidadeId)
    {
        try
        {
            var entidade = await _context.Entidades
            .AsNoTrackingWithIdentityResolution()
            .AsSplitQuery()
            .Include(e => e.Clientes)
                .ThenInclude(c => c.ProcessoProjectos)
                    .ThenInclude(p => p.Intervencaos)
            .SingleOrDefaultAsync(e => e.Id == entidadeId);

            if (entidade is null) return null;
            return new EntidadeDetailsDto(
                entidade.Id,
                entidade.Referencia,
                entidade.NomeSocial,
                entidade.Contribuinte,
                entidade.Observacoes,
                entidade.Tipo,
                entidade.Estado,
                entidade.Item1,
                entidade.Item2,
                entidade.Item3,
                entidade.DesignacaoComercial,
                entidade.DataActualizacao,
                entidade.Clientes
                    .OrderBy(c => c.Id)
                    .Select(c => new ClienteDto(
                        c.Id,
                        c.IdEntidade,
                        c.Referencia,
                        c.Observacoes,
                        c.Estado,
                        c.NProcesso,
                        c.ProcessoProjectos
                            .OrderBy(p => p.Id)
                            .Select(p => new ProcessoProjectoDto(
                                p.Id,
                                p.Referencia,
                                p.Estado,
                                p.DataInicio,
                                p.Intervencaos
                                .OrderBy(i => i.Id)
                                 .Select(i => new IntervencaoDto(
                                    i.Id,
                                    i.ProcessoId,
                                    i.Autor,
                                    i.Tipo,
                                    i.Estado,
                                    i.Tema
                                 ))
                                 .ToList()
                            ))
                            .ToList()
                    ))
                    .ToList()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Method}", nameof(GetDetailsAsync));
            throw;
        }

    }


}