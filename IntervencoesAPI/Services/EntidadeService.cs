using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Dtos.EntidadeDtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;


namespace IntervencoesAPI.Services;

public class EntidadeService
{
    private readonly IntervencoesAPIContext _context;

    private readonly ILogger<EntidadeService> _logger;

    public EntidadeService(IntervencoesAPIContext context, ILogger<EntidadeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Entidade> GetAll()
    {
        return _context.Entidades.OrderBy(i => i.Id).ToList();
    }

    public async Task<PagedList<Entidade>> GetAllPagedAsync(PageParameters pageParameters)
    {
        var query = _context.Entidades
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .AsQueryable();

        return await PagedList<Entidade>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
    }

    public Entidade? GetByIdEntidade(int id)
    {
        return _context.Entidades.FirstOrDefault(i => i.Id == id);
    }

    public async Task<Entidade> CreateAsync(CreateEntidade dto)
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


    public async Task<Entidade?> UpdateAsync(int id, UpdateEntidade dto)
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



    public bool Delete(int id)
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
        catch (System.Exception)
        {

            throw;
        }

    }


}