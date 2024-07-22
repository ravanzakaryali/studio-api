using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Space.Application.DTOs.Worker;

public class UsersController : BaseApiController
{
    [HttpGet("login")]
    public async Task<IActionResult> GetUserLogin()
    {
        return Ok(await Mediator.Send(new UserLoginQuery()));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
           => StatusCode(200, await Mediator.Send(new GetWorkerQuery(id)));

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await Mediator.Send(new GetUsersQuery()));
    }

    [Authorize]
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


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    => StatusCode(200, await Mediator.Send(new DeleteWorkerCommand(id)));
}
