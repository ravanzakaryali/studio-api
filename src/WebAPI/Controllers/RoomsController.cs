using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;



[Authorize(Roles = "admin")]
public class RoomsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => StatusCode(200, await Mediator.Send(new GetAllRoomQuery()));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto request)
        => StatusCode(201, await Mediator.Send(new CreateRoomCommand(request.Name, request.Capacity, request.Type)));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequestDto request)
       => StatusCode(200, await Mediator.Send(new UpdateRoomCommand(id, request)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => StatusCode(200, await Mediator.Send(new DeleteRoomCommand(id)));
}
