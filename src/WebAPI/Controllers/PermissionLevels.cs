namespace Space.WebAPI.Controllers;

public class PermissionLevelsController : BaseApiController
{
    [Authorize]
    [HttpGet("permission-accesses")]
    public async Task<IActionResult> GetPermissionAccesses()
        => Ok(await Mediator.Send(new GetPermissionAccessesQuery()));


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePermissionLevel(CreatePermissionLevelDto request)
        => Ok(await Mediator.Send(new CreatePermissionLevelCommand()
        {
            Name = request.Name,
            Accesses = request.Accesses
        }));
}