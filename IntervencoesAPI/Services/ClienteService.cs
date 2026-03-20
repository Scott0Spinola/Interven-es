using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Services;


public class ClienteService
{
    private readonly IntervencoesAPIContext _context;

    private readonly ILogger<ClienteService> _logger;

    public ClienteService(IntervencoesAPIContext context, ILogger<ClienteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Cliente> GetAll()
    {
        return _context.Clientes.OrderBy(i => i.Id).ToList();
    }

    /// <summary>
    /// Gets a paginated list of clientes.
    /// </summary>
    /// <param name="pageParameters">Pagination parameters (page number and page size).</param>
    /// <returns>A paged list containing the requested page of clientes.</returns>
    public async Task<PagedList<Cliente>> GetAllPagedAsync(PageParameters pageParameters)
    {
        var query = _context.Clientes
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .AsQueryable();

        return await PagedList<Cliente>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
    }

    public Cliente? GetByIdCliente(int id)
    {
        return _context.Clientes.FirstOrDefault(i => i.Id == id);
    }


    public Cliente? GetByReferencia(string referencia)
    {
        return _context.Clientes.FirstOrDefault(r => r.Referencia == referencia);
    }

    /// <summary>
    /// Gets the first cliente that matches the given entidade identifier.
    /// </summary>
    /// <param name="idEntidade">The entidade identifier.</param>
    /// <returns>The first matching cliente, or <see langword="null"/> if none exists.</returns>
    public Cliente? GetByIdEntidade(int idEntidade)
    {
        return _context.Clientes.FirstOrDefault(r => r.IdEntidade == idEntidade);
    }

    /// <summary>
    /// Gets clientes that match the given entidade identifier, returned as a paged result.
    /// </summary>
    /// <param name="idEntidade">The entidade identifier to filter clientes by.</param>
    /// <param name="pageParameters">Pagination parameters (page number and page size).</param>
    /// <returns>A paged list containing clientes associated with the provided entidade identifier.</returns>
    public async Task<PagedList<Cliente>> GetByIdEntidadePagedAsync(int idEntidade, PageParameters pageParameters)
    {
        var query = _context.Clientes
            .AsNoTracking()
            .Where(c => c.IdEntidade == idEntidade)
            .OrderBy(c => c.Id)
            .AsQueryable();

        return await PagedList<Cliente>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
    }
    public async Task<Cliente> CreateAsync(CreateCliente dto)
    {
        var cliente = new Cliente
        {
            IdEntidade = dto.IdEntidade,
            Referencia = dto.Referencia,
            Observacoes = dto.Observacoes,
            Estado = dto.Estado,
            NProcesso = dto.NProcesso,
            DataDeInicio = DateTime.UtcNow,
            DataActualizacao = DateTime.UtcNow,
            CliCampo1 = dto.CliCampo1,
            CliCampo2 = dto.CliCampo2,
            CliCampo3 = dto.CliCampo3,
            CliCampo4 = dto.CliCampo4,
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<Cliente?> UpdateAsync(int id, UpdateCliente dto)
    {
        var cliente = _context.Clientes.FirstOrDefault(i => i.Id == id);

        if (cliente is null)
        {
            return null;
        }
        cliente.IdEntidade = dto.IdEntidade;
        cliente.Referencia = dto.Referencia;
        cliente.Observacoes = dto.Observacoes;
        cliente.Estado = dto.Estado;
        cliente.NProcesso = dto.NProcesso;
        cliente.DataActualizacao = DateTime.UtcNow;
        cliente.CliCampo1 = dto.CliCampo1;
        cliente.CliCampo2 = dto.CliCampo2;
        cliente.CliCampo3 = dto.CliCampo3;
        cliente.CliCampo4 = dto.CliCampo4;




        await _context.SaveChangesAsync();
        return cliente;
    }

    public bool Delete(int id)
    {
        var cliente = _context.Clientes.FirstOrDefault(i => i.Id == id);
        if (cliente is null)
        {
            return false;
        }
        _context.Clientes.Remove(cliente);
        _context.SaveChanges();
        return true;
    }
}