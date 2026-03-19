using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntervencoesAPI.Controllers;

/// <summary>
/// Provides endpoints to manage processos/projectos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProcessoProjectoController : ControllerBase
{
	private readonly ProcessoProjectoService _processoProjectoService;
	private readonly ILogger<ProcessoProjectoController> _logger;

	public ProcessoProjectoController(ProcessoProjectoService processoProjectoService, ILogger<ProcessoProjectoController> logger)
	{
		_processoProjectoService = processoProjectoService;
		_logger = logger;
	}

	/// <summary>
	/// Gets a paginated list of processos/projectos.
	/// </summary>
	/// <param name="pageParameters">Pagination parameters (page number and page size).</param>
	/// <remarks>
	/// Returns a paged result containing the requested page of processos/projectos.
	/// </remarks>
	/// <response code="200">Processos/Projectos returned successfully.</response>
	/// <response code="400">The query parameters are invalid.</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<ProcessoProjecto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<PagedList<ProcessoProjecto>>> GetAll([FromQuery] PageParameters pageParameters)
	{
		try
		{
			_logger.LogInformation(
				"CRUD {CrudOperation} {Resource} pageNumber={PageNumber} pageSize={PageSize}",
				"Read",
				"ProcessoProjecto",
				pageParameters.PageNumber,
				pageParameters.PageSize);

			var paged = await _processoProjectoService.GetAllPagedAsync(pageParameters);
			return Ok(paged);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Gets a processo/projecto by identifier.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <remarks>
	/// Returns the processo/projecto resource if found.
	/// </remarks>
	/// <response code="200">Processo/Projecto returned successfully.</response>
	/// <response code="404">Processo/Projecto not found.</response>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(ProcessoProjecto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<ProcessoProjecto> GetById(int id)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Read", "ProcessoProjecto", id);

			var processoProjecto = _processoProjectoService.GetByIdProcessoProjecto(id);
			if (processoProjecto is null)
			{
				return NotFound($"No ProcessoProjecto exists with the provided ID: {id}.");
			}

			return processoProjecto;
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Creates a new processo/projecto.
	/// </summary>
	/// <param name="dto">The data used to create a new processo/projecto.</param>
	/// <remarks>
	/// Creates a new processo/projecto and returns the created resource with its generated identifier.
	/// </remarks>
	/// <response code="201">Processo/Projecto created successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	[HttpPost]
	[ProducesResponseType(typeof(ProcessoProjecto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ProcessoProjecto>> Create([FromBody] CreateProcessoProjecto dto)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource}", "Create", "ProcessoProjecto");

			var created = await _processoProjectoService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Updates an existing processo/projecto.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <param name="updateProcessoProjecto">The data used to update the processo/projecto.</param>
	/// <remarks>
	/// Updates the processo/projecto identified by <paramref name="id"/> and returns the updated resource.
	/// </remarks>
	/// <response code="200">Processo/Projecto updated successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	/// <response code="404">Processo/Projecto not found.</response>
	[HttpPut("{id:int}")]
	[ProducesResponseType(typeof(ProcessoProjecto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ProcessoProjecto>> Update(int id, [FromBody] UpdateProcessoProjecto updateProcessoProjecto)
	{
		_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Update", "ProcessoProjecto", id);

		var updated = await _processoProjectoService.UpdateAsync(id, updateProcessoProjecto);

		if (updated is null)
		{
			return NotFound($"No ProcessoProjecto exists with the provided ID: {id}.");
		}

		return Ok(updated);
	}

	/// <summary>
	/// Deletes an existing processo/projecto.
	/// </summary>
	/// <param name="id">The processo/projecto identifier.</param>
	/// <remarks>
	/// Deletes the processo/projecto identified by <paramref name="id"/>.
	/// </remarks>
	/// <response code="204">Processo/Projecto deleted successfully.</response>
	/// <response code="404">Processo/Projecto not found.</response>
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Delete(int id)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Delete", "ProcessoProjecto", id);

			var deleted = _processoProjectoService.Delete(id);
			if (!deleted)
			{
				return NotFound($"No ProcessoProjecto exists with the provided ID: {id}.");
			}

			return NoContent();
		}
		catch (System.Exception)
		{
			throw;
		}
	}
}
