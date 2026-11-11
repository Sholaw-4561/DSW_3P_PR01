using DSW_3P_PR01.DTOs;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/misiones")]
public sealed class MisionesController(IMisionService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MisionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MisionResponse>>> ObtenerTodas(
        CancellationToken cancellationToken)
    {
        return Ok(await service.ObtenerTodasAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MisionResponse>> ObtenerPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var mision = await service.ObtenerPorIdAsync(id, cancellationToken);
        return mision is null ? NotFound() : Ok(mision);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MisionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MisionResponse>> Agregar(
        [FromBody] CreateMisionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var creada = await service.AgregarAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(CrearProblemaDeValidacion(exception));
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MisionResponse>> Actualizar(
        int id,
        [FromBody] UpdateMisionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var actualizada = await service.ActualizarAsync(id, request, cancellationToken);
            return actualizada is null ? NotFound() : Ok(actualizada);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(CrearProblemaDeValidacion(exception));
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var eliminada = await service.EliminarAsync(id, cancellationToken);
        return eliminada ? NoContent() : NotFound();
    }

    private ProblemDetails CrearProblemaDeValidacion(ArgumentException exception)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Datos de misión no válidos",
            Detail = exception.Message,
            Instance = HttpContext.Request.Path
        };
    }
}
