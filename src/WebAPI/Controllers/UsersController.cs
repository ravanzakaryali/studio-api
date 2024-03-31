using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;
using Microsoft.Extensions.Configuration;

public class UsersController : BaseApiController
{
    [HttpGet("login")]
    public async Task<IActionResult> GetUserLogin()
    {
        return Ok(await Mediator.Send(new UserLoginQuery()));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
    {
        await Mediator.Send(new CreateUserCommand
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        });
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateUserRole([FromRoute] int id, [FromBody] IEnumerable<CreateRoleRequestDto> request)
    {
        await Mediator.Send(new CreateRoleByUserCommand(id, request));
        return NoContent();
    }


    [Authorize]
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles([FromRoute] int id)
        => Ok(await Mediator.Send(new GetRolesByUserQuery(id)));
}
