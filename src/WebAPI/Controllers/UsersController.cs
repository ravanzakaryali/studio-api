using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet("login")]
    public async Task<IActionResult> GetUserLogin()
        => Ok(await Mediator.Send(new UserLoginQuery()));


    [Authorize(Roles = "admin")]
    [HttpPost("{id}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateUserRole([FromRoute] Guid id, [FromBody] IEnumerable<CreateRoleRequestDto> request)
    {
        await Mediator.Send(new CreateRoleByUserCommand(id, request));
        return NoContent();
    }

    [Authorize]
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetRolesByUserQuery(id)));
}
