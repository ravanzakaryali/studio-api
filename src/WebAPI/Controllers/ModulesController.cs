using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

public class ModulesController : BaseApiController
{

    //todo: added modules
    // [Authorize(Roles = "admin")]
    // [HttpPost]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesDefaultResponseType]
    // public async Task<IActionResult> Create([FromBody] CreateModuleRequestDto module)
    //         => StatusCode(201, await Mediator.Send(new CreateModuleCommand()
    //         {
    //             Module = module
    //         }));

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteModuleCommand(id));
        return StatusCode(204);
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllModuleQuery()));

    //todo: delete endpoint
    // [Authorize(Roles = "admin")]
    // [HttpGet("non-program")]
    // public async Task<IActionResult> GetNonProgramModules()
    //     => Ok(await Mediator.Send(new GetNonProgramModulesQuery()));
}
