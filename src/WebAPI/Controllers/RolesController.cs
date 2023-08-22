using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class RolesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllRolesQuery()));
}
