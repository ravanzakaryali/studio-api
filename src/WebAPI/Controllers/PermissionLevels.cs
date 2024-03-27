namespace Space.WebAPI.Controllers;

public class PermissionLevelsController : BaseApiController
{
    [Authorize]
    [HttpGet("permission-accesses")]
    public async Task<IActionResult> GetPermissionAccesses()
        => Ok(await Mediator.Send(new GetPermissionAccessesQuery()));


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePermissionAccess(CreatePermissionAccessDto request)
        => Ok(await Mediator.Send());
}