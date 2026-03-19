using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Services;

public class ProcessoProjectoService
{
	private readonly IntervencoesAPIContext _context;

	private readonly ILogger<ProcessoProjectoService> _logger;

	public ProcessoProjectoService(IntervencoesAPIContext context, ILogger<ProcessoProjectoService> logger)
	{
		_context = context;
		_logger = logger;
	}

	public List<ProcessoProjecto> GetAll()
	{
		return _context.ProcessoProjectos.OrderBy(i => i.Id).ToList();
	}

	public async Task<PagedList<ProcessoProjecto>> GetAllPagedAsync(PageParameters pageParameters)
	{
		var query = _context.ProcessoProjectos
			.AsNoTracking()
			.OrderBy(i => i.Id)
			.AsQueryable();

		return await PagedList<ProcessoProjecto>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
	}

	public ProcessoProjecto? GetByIdProcessoProjecto(int id)
	{
		return _context.ProcessoProjectos.FirstOrDefault(i => i.Id == id);
	}

	public async Task<ProcessoProjecto> CreateAsync(CreateProcessoProjecto dto)
	{
		var processoProjecto = new ProcessoProjecto
		{
			NumArquivo = dto.NumArquivo,
			Referencia = dto.Referencia,
			Estado = dto.Estado,
			DataInicio = dto.DataInicio,
			DataPrevistaConclusao = dto.DataPrevistaConclusao,
			EsforcoPrevisto = dto.EsforcoPrevisto,
			EsforcoReal = dto.EsforcoReal,
			ProcessoPaiId = dto.ProcessoPaiId,
			AvencaId = dto.AvencaId,
			ClienteId = dto.ClienteId,
			FornecedorId = dto.FornecedorId,
			Descricao = dto.Descricao,
			Fornecedores = dto.Fornecedores,
			Responsavel = dto.Responsavel,
			IdProposta = dto.IdProposta,
			IdContracto = dto.IdContracto,
		};

		_context.ProcessoProjectos.Add(processoProjecto);
		await _context.SaveChangesAsync();
		return processoProjecto;
	}

	public async Task<ProcessoProjecto?> UpdateAsync(int id, UpdateProcessoProjecto dto)
	{
		var processoProjecto = _context.ProcessoProjectos.FirstOrDefault(i => i.Id == id);

		if (processoProjecto is null)
		{
			return null;
		}

		processoProjecto.NumArquivo = dto.NumArquivo;
		processoProjecto.Referencia = dto.Referencia;
		processoProjecto.Estado = dto.Estado;
		processoProjecto.DataInicio = dto.DataInicio;
		processoProjecto.DataPrevistaConclusao = dto.DataPrevistaConclusao;
		processoProjecto.DataConclusao = dto.DataConclusao;
		processoProjecto.EsforcoPrevisto = dto.EsforcoPrevisto;
		processoProjecto.EsforcoReal = dto.EsforcoReal;
		processoProjecto.ProcessoPaiId = dto.ProcessoPaiId;
		processoProjecto.AvencaId = dto.AvencaId;
		processoProjecto.ClienteId = dto.ClienteId;
		processoProjecto.FornecedorId = dto.FornecedorId;
		processoProjecto.Descricao = dto.Descricao;
		processoProjecto.Fornecedores = dto.Fornecedores;
		processoProjecto.Responsavel = dto.Responsavel;
		processoProjecto.IdProposta = dto.IdProposta;
		processoProjecto.IdContracto = dto.IdContracto;

		await _context.SaveChangesAsync();
		return processoProjecto;
	}

	public bool Delete(int id)
	{
		var processoProjecto = _context.ProcessoProjectos.FirstOrDefault(i => i.Id == id);
		if (processoProjecto is null)
		{
			return false;
		}

		_context.ProcessoProjectos.Remove(processoProjecto);
		_context.SaveChanges();
		return true;
	}
}

