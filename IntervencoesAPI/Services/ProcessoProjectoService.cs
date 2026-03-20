using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Services;

/// <summary>
/// Service layer for CRUD operations over <see cref="ProcessoProjecto"/>.
/// </summary>
/// <remarks>
/// This service encapsulates Entity Framework Core access to <see cref="IntervencoesAPIContext"/>.
/// </remarks>
public class ProcessoProjectoService
{
	private readonly IntervencoesAPIContext _context;

	private readonly ILogger<ProcessoProjectoService> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProcessoProjectoService"/> class.
	/// </summary>
	/// <param name="context">The EF Core database context.</param>
	/// <param name="logger">The logger instance.</param>
	public ProcessoProjectoService(IntervencoesAPIContext context, ILogger<ProcessoProjectoService> logger)
	{
		_context = context;
		_logger = logger;
	}

	/// <summary>
	/// Gets all processos/projectos ordered by identifier.
	/// </summary>
	/// <returns>A list of all <see cref="ProcessoProjecto"/> records.</returns>
	public List<ProcessoProjecto> GetAll()
	{
		return _context.ProcessoProjectos.OrderBy(i => i.Id).ToList();
	}

	/// <summary>
	/// Gets a paginated list of processos/projectos ordered by identifier.
	/// </summary>
	/// <param name="pageParameters">The pagination parameters (page number and page size).</param>
	/// <returns>A <see cref="PagedList{T}"/> containing the requested page.</returns>
	/// <remarks>
	/// The query uses <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/> for read-only performance.
	/// </remarks>
	public async Task<PagedList<ProcessoProjecto>> GetAllPagedAsync(PageParameters pageParameters)
	{
		var query = _context.ProcessoProjectos
			.AsNoTracking()
			.OrderBy(i => i.Id)
			.AsQueryable();

		return await PagedList<ProcessoProjecto>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
	}

	/// <summary>
	/// Gets a processo/projecto by identifier.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <returns>The matching <see cref="ProcessoProjecto"/>, or <see langword="null"/> if not found.</returns>
	public ProcessoProjecto? GetByIdProcessoProjecto(int id)
	{
		return _context.ProcessoProjectos.FirstOrDefault(i => i.Id == id);
	}

	/// <summary>
	/// Gets a processo/projecto by reference (referência).
	/// </summary>
	/// <param name="referencia">The reference value to search for.</param>
	/// <returns>The matching <see cref="ProcessoProjecto"/>, or <see langword="null"/> if not found.</returns>
	public ProcessoProjecto? GetByRefencia(string referencia)
	{
		return _context.ProcessoProjectos.FirstOrDefault(r => r.Referencia == referencia);
	}

	/// <summary>
	/// Gets the first processo/projecto associated with a given client identifier.
	/// </summary>
	/// <param name="IdCliente">The client identifier to search for.</param>
	/// <returns>The first matching <see cref="ProcessoProjecto"/>, or <see langword="null"/> if none exist.</returns>
	public ProcessoProjecto? GetByIdClient(int IdCliente)
	{
		return _context.ProcessoProjectos.FirstOrDefault(c => c.ClienteId == IdCliente);
	}

	/// <summary>
	/// Creates a new processo/projecto.
	/// </summary>
	/// <param name="dto">The DTO containing creation data.</param>
	/// <returns>The created <see cref="ProcessoProjecto"/> (including its generated identifier).</returns>
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

	/// <summary>
	/// Updates an existing processo/projecto.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <param name="dto">The DTO containing update data.</param>
	/// <returns>
	/// The updated <see cref="ProcessoProjecto"/>, or <see langword="null"/> if no record exists with the provided identifier.
	/// </returns>
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

	/// <summary>
	/// Deletes an existing processo/projecto.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <returns><see langword="true"/> if deleted; otherwise <see langword="false"/> if not found.</returns>
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

