using IntervencoesAPI.Dtos;
using IntervencoesAPI.Dtos.EntidadeDtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntervencoesAPI.Controllers;

/// <summary>
/// Provides endpoints to manage entidades.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntidadeController : ControllerBase
{
	private readonly EntidadeService _entidadeService;

	public EntidadeController(EntidadeService entidadeService)
	{
		_entidadeService = entidadeService;
	}

	/// <summary>
	/// Gets a paginated list of entidades.
	/// </summary>
	/// <param name="pageParameters">Pagination parameters (page number and page size).</param>
	/// <remarks>
	/// Returns a paged result containing the requested page of entidades.
	/// </remarks>
	/// <response code="200">Entidades returned successfully.</response>
	/// <response code="400">The query parameters are invalid.</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<Entidade>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<PagedList<Entidade>>> GetAll([FromQuery] PageParameters pageParameters)
	{
		try
		{
			var pagedEntidades = await _entidadeService.GetAllPagedAsync(pageParameters);
			return Ok(pagedEntidades);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Gets an entidade by identifier.
	/// </summary>
	/// <param name="id">The entidade identifier.</param>
	/// <remarks>
	/// Returns the entidade resource if found.
	/// </remarks>
	/// <response code="200">Entidade returned successfully.</response>
	/// <response code="404">Entidade not found.</response>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(Entidade), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Entidade> GetById(int id)
	{
		try
		{
			var entidade = _entidadeService.GetByIdEntidade(id);
			if (entidade is null)
			{
				return NotFound($"No Entidade exists with the provided ID: {id}.");
			}

			return entidade;
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Creates a new entidade.
	/// </summary>
	/// <param name="dto">The data used to create a new entidade.</param>
	/// <remarks>
	/// Creates a new entidade and returns the created resource with its generated identifier.
	/// </remarks>
	/// <response code="201">Entidade created successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	[HttpPost]
	[ProducesResponseType(typeof(Entidade), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Entidade>> Create([FromBody] CreateEntidade dto)
	{
		try
		{
			var created = await _entidadeService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Updates an existing entidade.
	/// </summary>
	/// <param name="id">The entidade identifier.</param>
	/// <param name="updateEntidade">The data used to update the entidade.</param>
	/// <remarks>
	/// Updates the entidade identified by <paramref name="id"/> and returns the updated resource.
	/// </remarks>
	/// <response code="200">Entidade updated successfully.</response>
	/// <response code="400">The request data is invalid.</response>
	/// <response code="404">Entidade not found.</response>
	[HttpPut("{id:int}")]
	[ProducesResponseType(typeof(Entidade), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Entidade>> Update(int id, [FromBody] UpdateEntidade updateEntidade)
	{
		var updatedEntidade = await _entidadeService.UpdateAsync(id, updateEntidade);

		if (updatedEntidade is null)
		{
			return NotFound($"No Entidade exists with the provided ID: {id}.");
		}

		return Ok(updatedEntidade);
	}

	/// <summary>
	/// Deletes an existing entidade.
	/// </summary>
	/// <param name="id">The entidade identifier.</param>
	/// <remarks>
	/// Deletes the entidade identified by <paramref name="id"/>.
	/// </remarks>
	/// <response code="204">Entidade deleted successfully.</response>
	/// <response code="404">Entidade not found.</response>
	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult Delete(int id)
	{
		try
		{
			var deleted = _entidadeService.Delete(id);
			if (!deleted)
			{
				return NotFound($"No Entidade exists with the provided ID: {id}.");
			}

			return NoContent();
		}
		catch (System.Exception)
		{
			throw;
		}
	}
}
