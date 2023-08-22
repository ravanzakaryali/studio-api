using Microsoft.AspNetCore.Authorization;
using Space.Application.Handlers;

namespace Space.WebAPI.Controllers;


[Authorize(Roles = "admin")]
public class ModulesController : BaseApiController
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<GetModuleDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequestDto modules)
        => StatusCode(201, await Mediator.Send(new CreateModuleCommand(modules.ProgramId, modules.Modules)));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteModuleCommand(id));
        return StatusCode(204);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllModuleQuery()));
}
