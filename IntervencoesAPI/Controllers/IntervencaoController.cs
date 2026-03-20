using Microsoft.AspNetCore.Mvc;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IntervencoesAPI.Controllers;

/// <summary>
/// Provides endpoints to manage intervencoes.
/// </summary>
/// <remarks>
/// Base route: <c>/api/Intervencao</c>.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class IntervencaoController : ControllerBase
{
	private readonly IntervencaoService _intervencaoService;
	private readonly ILogger<IntervencaoController> _logger;

	public IntervencaoController(IntervencaoService intervencaoService, ILogger<IntervencaoController> logger)
	{
		_intervencaoService = intervencaoService;
		_logger = logger;
	}

	/// <summary>
	/// Gets a paginated list of intervencoes.
	/// </summary>
	/// <param name="pageParameters">Pagination parameters (page number and page size).</param>
	/// <remarks>
	/// Returns a paged result containing the requested page of intervencoes.
	/// </remarks>
	/// <response code="200">Intervencoes returned successfully.</response>
	/// <response code="400">The query parameters are invalid.</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<Intervencao>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<PagedList<Intervencao>>> GetAll([FromQuery] PageParameters pageParameters)
	{
		try
		{
			_logger.LogInformation(
				"CRUD {CrudOperation} {Resource} pageNumber={PageNumber} pageSize={PageSize}",
				"Read",
				"Intervencao",
				pageParameters.PageNumber,
				pageParameters.PageSize);

			var paged = await _intervencaoService.GetAllPagedAsync(pageParameters);
			return Ok(paged);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Gets an intervencao by identifier.
	/// </summary>
	/// <param name="id">The intervencao identifier.</param>
	/// <remarks>
	/// Returns the intervencao resource if found.
	/// </remarks>
	/// <response code="200">Intervencao returned successfully.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Intervencao> GetById(int id)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Read", "Intervencao", id);

			var intervencao = _intervencaoService.GetByIdIntervencao(id);
			if (intervencao is null)
			{
				return NotFound($"No Intervencao exists with the provided ID: {id}.");
			}

			return intervencao;
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Gets an intervencao by reference (referência).
	/// </summary>
	/// <param name="referencia">The reference value to search for. Read from query string parameter <c>referencia</c>.</param>
	/// <remarks>
	/// The provided value is matched against the <c>Referencia</c> field.
	/// </remarks>
	/// <response code="200">Intervencao returned successfully.</response>
	/// <response code="400">The <c>referencia</c> query parameter is missing or invalid.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpGet("Referencia")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Intervencao> GetByReferencia([FromQuery] string referencia)
	{
		if (string.IsNullOrWhiteSpace(referencia))
		{
			return BadRequest("Query parameter 'referencia' is required.");
		}

		_logger.LogInformation("CRUD {CrudOperation} {Resource} referencia={Referencia}", "Read", "Intervencao", referencia);

		var intervencao = _intervencaoService.GetByReferencia(referencia);
		if (intervencao is null)
		{
			return NotFound($"No Intervencao exists with the provided Referencia: {referencia}.");
		}

		return Ok(intervencao);
	}

	/// <summary>
	/// Gets an intervencao by tipo.
	/// </summary>
	/// <param name="tipo">The tipo value to search for. Read from query string parameter <c>tipo</c>.</param>
	/// <remarks>
	/// Returns the first intervencao whose <c>Tipo</c> matches the provided value.
	/// </remarks>
	/// <response code="200">Intervencao returned successfully.</response>
	/// <response code="400">The <c>tipo</c> query parameter is invalid.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpGet("Tipo")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Intervencao> GetByTipo([FromQuery] int tipo)
	{
		if (tipo < 0)
		{
			return BadRequest("Query parameter 'tipo' must be a non-negative integer.");
		}

		_logger.LogInformation("CRUD {CrudOperation} {Resource} tipo={Tipo}", "Read", "Intervencao", tipo);

		var intervencao = _intervencaoService.GetByTipo(tipo);
		if (intervencao is null)
		{
			return NotFound($"No Intervencao exists with the provided Tipo: {tipo}.");
		}

		return Ok(intervencao);
	}

	/// <summary>
	/// Gets an intervencao by estado.
	/// </summary>
	/// <param name="estado">The estado value to search for. Read from query string parameter <c>estado</c>.</param>
	/// <remarks>
	/// Returns the first intervencao whose <c>Estado</c> matches the provided value.
	/// </remarks>
	/// <response code="200">Intervencao returned successfully.</response>
	/// <response code="400">The <c>estado</c> query parameter is invalid.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpGet("Estado")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Intervencao> GetByEstado([FromQuery] int estado)
	{
		if (estado < 0)
		{
			return BadRequest("Query parameter 'estado' must be a non-negative integer.");
		}

		_logger.LogInformation("CRUD {CrudOperation} {Resource} estado={Estado}", "Read", "Intervencao", estado);

		var intervencao = _intervencaoService.GetByEstado(estado);
		if (intervencao is null)
		{
			return NotFound($"No Intervencao exists with the provided Estado: {estado}.");
		}

		return Ok(intervencao);
	}

	/// <summary>
	/// Gets intervencoes created on a specific calendar date.
	/// </summary>
	/// <param name="dataCriacao">The date to search for. Read from query string parameter <c>dataCriacao</c>.</param>
	/// <remarks>
	/// Internally queries the full day interval for the provided date.
	/// Example: GET /api/Intervencao/por-data-criacao-dia?dataCriacao=2026-03-20
	/// </remarks>
	/// <response code="200">Intervencoes returned successfully.</response>
	/// <response code="400">The <c>dataCriacao</c> query parameter is missing or invalid.</response>
	[HttpGet("por-data-criacao-dia")]
	[ProducesResponseType(typeof(List<Intervencao>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<Intervencao>>> GetByDataCriacao([FromQuery] DateTime dataCriacao)
	{
		if (dataCriacao == default)
		{
			return BadRequest("Query parameter 'dataCriacao' is required.");
		}

		_logger.LogInformation("CRUD {CrudOperation} {Resource} dataCriacao={DataCriacao}", "Read", "Intervencao", dataCriacao);

		var intervencoes = await _intervencaoService.GetByIntervelDataDeCriacao(dataCriacao);
		return Ok(intervencoes);
	}

	/// <summary>
	/// Gets an intervencao by processo identifier.
	/// </summary>
	/// <param name="idProcesso">The processo identifier to search for. Read from query string parameter <c>idProcesso</c>.</param>
	/// <remarks>
	/// Returns the first intervencao whose <c>ProcessoId</c> matches the provided identifier.
	/// </remarks>
	/// <response code="200">Intervencao returned successfully.</response>
	/// <response code="400">The <c>idProcesso</c> query parameter is invalid.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpGet("IdProcesso")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Intervencao> GetByIdProcesso([FromQuery] int idProcesso)
	{
		if (idProcesso <= 0)
		{
			return BadRequest("Query parameter 'idProcesso' must be a positive integer.");
		}

		_logger.LogInformation("CRUD {CrudOperation} {Resource} idProcesso={IdProcesso}", "Read", "Intervencao", idProcesso);

		var intervencao = _intervencaoService.GetByIdProcesso(idProcesso);
		if (intervencao is null)
		{
			return NotFound($"No Intervencao exists with the provided IdProcesso: {idProcesso}.");
		}

		return Ok(intervencao);
	}

	/// <summary>
	/// Gets intervencoes whose DataCriacao is within the provided interval.
	/// </summary>
	/// <param name="start">Interval start (inclusive). Read from query string parameter <c>start</c>.</param>
	/// <param name="end">Interval end (inclusive). Read from query string parameter <c>end</c>.</param>
	/// <remarks>
	/// Example: GET /api/Intervencao/por-data-criacao?start=2026-03-01T00:00:00&amp;end=2026-03-31T23:59:59
	/// </remarks>
	/// <response code="200">Intervencoes returned successfully.</response>
	/// <response code="400">The query parameters are invalid (for example, <c>start</c> is greater than <c>end</c>).</response>
	[HttpGet("por-data-criacao")]
	[ProducesResponseType(typeof(List<Intervencao>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<Intervencao>>> GetByIntervaloDataCriacao([FromQuery] DateTime start, [FromQuery] DateTime end)
	{
		if (start > end)
		{
			return BadRequest("'start' must be less than or equal to 'end'.");
		}

		_logger.LogInformation(
			"CRUD {CrudOperation} {Resource} start={Start} end={End}",
			"Read",
			"Intervencao",
			start,
			end);

		var intervencoes = await _intervencaoService.GetByIntervaloDataCriacaoAsync(start, end);
		return Ok(intervencoes);
	}

	/// <summary>
	/// Creates a new intervencao.
	/// </summary>
	/// <param name="dto">The data used to create a new intervencao.</param>
	/// <remarks>
	/// Creates a new intervencao and returns the created resource with its generated identifier.
	/// </remarks>
	/// <response code="201">Intervencao created successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	[HttpPost]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Intervencao>> Create([FromBody] CreateIntervencao dto)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource}", "Create", "Intervencao");

			var created = await _intervencaoService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Updates an existing intervencao.
	/// </summary>
	/// <param name="id">The intervencao identifier.</param>
	/// <param name="updateIntervencao">The data used to update the intervencao.</param>
	/// <remarks>
	/// Updates the intervencao identified by <paramref name="id"/> and returns the updated resource.
	/// </remarks>
	/// <response code="200">Intervencao updated successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpPut("{id:int}")]
	[ProducesResponseType(typeof(Intervencao), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Intervencao>> Update(int id, [FromBody] UpdateIntervencao updateIntervencao)
	{
		_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Update", "Intervencao", id);

		var updated = await _intervencaoService.UpdateAsync(id, updateIntervencao);

		if (updated is null)
		{
			return NotFound($"No Intervencao exists with the provided ID: {id}.");
		}

		return Ok(updated);
	}

	/// <summary>
	/// Deletes an existing intervencao.
	/// </summary>
	/// <param name="id">The intervencao identifier.</param>
	/// <remarks>
	/// Deletes the intervencao identified by <paramref name="id"/>.
	/// </remarks>
	/// <response code="204">Intervencao deleted successfully.</response>
	/// <response code="404">Intervencao not found.</response>
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Delete(int id)
	{
		try
		{
			_logger.LogInformation("CRUD {CrudOperation} {Resource} id={Id}", "Delete", "Intervencao", id);

			var deleted = _intervencaoService.Delete(id);
			if (!deleted)
			{
				return NotFound($"No Intervencao exists with the provided ID: {id}.");
			}
			return NoContent();
		}
		catch (System.Exception)
		{
			throw;
		}
	}
}
