namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class ModulesController : BaseApiController
{

    //post edim

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
