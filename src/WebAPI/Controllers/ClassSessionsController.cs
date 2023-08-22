using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class ClassSessionsController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateClassSessionRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionCommand()
        {
            ClassId = request.ClassId,
            Sessions = request.Sessions,
        });
        return NoContent();
    }

    [HttpPut("/api/class-sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassSessionAttendance([FromBody] UpdateClassSessionByDateRequestDto request)
    {
        await Mediator.Send(new UpdateClassSessionByDateCommand(request.ClassId, request.OldDate, request.NewDate, request.Sessions));
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("/api/class-sessions/{id}")]
    public async Task<IActionResult> GetClassSessionDetail([FromRoute] Guid id, [FromQuery] DateTime date)
    {
        return Ok(await Mediator.Send(new GetClassSessionsByDateQuery(id, date)));
    }

    [HttpPost("/api/session-extensions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> SessionExtension([FromBody] SessionExtensionsRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionExtensionCommand(request.Hours, request.ClassId, request.RoomId, request.Sessions));
        return NoContent();
    }
}
