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