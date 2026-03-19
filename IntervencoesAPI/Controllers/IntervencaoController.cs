using Microsoft.AspNetCore.Mvc;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using Microsoft.AspNetCore.Http;

namespace IntervencoesAPI.Controllers;

/// <summary>
/// Provides endpoints to manage intervencoes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IntervencaoController : ControllerBase
{
	private readonly IntervencaoService _intervencaoService;

	public IntervencaoController(IntervencaoService intervencaoService)
	{
		_intervencaoService = intervencaoService;
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
