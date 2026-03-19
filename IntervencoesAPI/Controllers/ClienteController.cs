using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;
using IntervencoesAPI.Services;

namespace IntervencoesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClienteController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Gets a paginated list of clientes.
    /// </summary>
    /// <param name="pageParameters">Pagination parameters (page number and page size).</param>
    /// <remarks>
    /// Returns a paged result containing the requested page of clientes.
    /// </remarks>
    /// <response code="200">Clientes returned successfully.</response>
    /// <response code="400">The query parameters are invalid.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<Cliente>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedList<Cliente>>> GetAll([FromQuery] PageParameters pageParameters)
    {
        try
        {
            var pagedIdeas = await _clienteService.GetAllPagedAsync(pageParameters);
            return Ok(pagedIdeas);
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    /// <summary>
    /// Gets a cliente by identifier.
    /// </summary>
    /// <param name="id">The cliente identifier.</param>
    /// <remarks>
    /// Returns the cliente resource if found.
    /// </remarks>
    /// <response code="200">Cliente returned successfully.</response>
    /// <response code="404">Cliente not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Cliente> GetById(int id)
    {
        try
        {
            var cliente = _clienteService.GetByIdCliente(id);
            if (cliente is null)
            {
                return NotFound($"No Cliente exists with the provided ID: {id}.");
            }

            return cliente;
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    /// <summary>
    /// Creates a new cliente.
    /// </summary>
    /// <param name="dto">The data used to create a new cliente.</param>
    /// <remarks>
    /// Creates a new cliente and returns the created resource with its generated identifier.
    /// </remarks>
    /// <response code="201">Cliente created successfully.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cliente>> Create([FromBody] CreateCliente dto)
    {
        try
        {
            var created = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Updates an existing cliente.
    /// </summary>
    /// <param name="id">The cliente identifier.</param>
    /// <param name="updatecliente">The data used to update the cliente.</param>
    /// <remarks>
    /// Updates the cliente identified by <paramref name="id"/> and returns the updated resource.
    /// </remarks>
    /// <response code="200">Cliente updated successfully.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">Cliente not found.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Cliente>> Update(int id, [FromBody] UpdateCliente updatecliente)
    {
        var updatedCliente = await _clienteService.UpdateAsync(id, updatecliente);

        if (updatedCliente is null)
        {
            return NotFound($"No Cliente exists with the provided ID: {id}.");
        }

        return Ok(updatedCliente);
    }


    /// <summary>
    /// Deletes an existing cliente.
    /// </summary>
    /// <param name="id">The cliente identifier.</param>
    /// <remarks>
    /// Deletes the cliente identified by <paramref name="id"/>.
    /// </remarks>
    /// <response code="204">Cliente deleted successfully.</response>
    /// <response code="404">Cliente not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
   public ActionResult Delete(int id)
   {
        try
        {
            var DeleteCliente = _clienteService.Delete(id);
              if (!DeleteCliente)
                {
                    return NotFound($"No Cliente exists with the provided ID: {id}.");
                }
                return NoContent();
        }
        catch (System.Exception)
        {
            
            throw;
        }
   }



}
