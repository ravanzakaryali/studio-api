using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class ModulesController : BaseApiController
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequestDto module)
        => StatusCode(201, await Mediator.Send(new CreateModuleCommand()
        {
            Module = module
        }));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteModuleCommand(id));
        return StatusCode(204);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllModuleQuery()));
}
