namespace Space.WebAPI.Controllers;

public class PermissionLevelsController : BaseApiController
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPermissionLevels()
        => Ok(await Mediator.Send(new GetAllPermissionLevelQuery()));

    [Authorize]
    [HttpGet("permission-accesses")]
    public async Task<IActionResult> GetPermissionAccesses()
        => Ok(await Mediator.Send(new GetPermissionAccessesQuery()));


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePermissionLevel(CreatePermissionLevelDto request)
    {
        await Mediator.Send(new CreatePermissionLevelCommand
        {
            Name = request.Name,
            Accesses = request.Accesses
        });
        return Ok();
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermissionLevel([FromRoute] int id)
    {
        await Mediator.Send(new DeletePermissionLevelCommand
        {
            Id = id
        });
        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdatePermissionLevel([FromBody] IEnumerable<UpdatePermissionLevelDto> request)
    {
        await Mediator.Send(new UpdatePermissionLevelCommand
        {
            PermissionAccesses = request
        });
        return Ok();
    }
}