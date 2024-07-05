using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;
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

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateUserRole([FromRoute] int id, [FromBody] IEnumerable<CreateRoleRequestDto> request)
    {
        await Mediator.Send(new CreateRoleByUserCommand(id, request));
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRequestWorkerDto request)
              => StatusCode(201, await Mediator.Send(new CreateWorkerCommand(request.Name, request.Surname, request.Email, request.Fincode, request.GroupsId)));

    [Authorize]
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles([FromRoute] int id)
        => Ok(await Mediator.Send(new GetRolesByUserQuery(id)));


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    => StatusCode(200, await Mediator.Send(new DeleteWorkerCommand(id)));
}
