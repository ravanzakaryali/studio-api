using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

public class SessionsController : BaseApiController
{


    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateSessionRequestDto request)
                => StatusCode(201, await Mediator.Send(new CreateSessionCommand(request.Name)));


    [Authorize(Roles = "admin")]
    [HttpPost("{id}/details")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateDetails([FromRoute] Guid id, [FromBody] CreateSessionDetailRequestDto request)
        => StatusCode(201, await Mediator.Send(new CreateSessionDetailCommand(id, request)));

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
        => StatusCode(200, await Mediator.Send(new GetAllSessionQuery()));

    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
        => StatusCode(200, await Mediator.Send(new GetSessionQuery(id)));

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
           => StatusCode(200, await Mediator.Send(new DeleteSessionCommand(id)));

    [Authorize(Roles = "admin")]
    [HttpDelete("{sessionId}/details/{sessionDetailId}")]
    public async Task<IActionResult> DeleteSessionDetail([FromRoute] Guid sessionId, [FromRoute] Guid sessionDetailId)
        => StatusCode(200, await Mediator.Send(new DeleteSessionDetailCommand(sessionId, sessionDetailId)));


}
