namespace Space.WebAPI.Controllers;

public class SessionsController : BaseApiController
{
    
    [Authorize ]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateSessionRequestDto request)
                => StatusCode(201, await Mediator.Send(new CreateSessionCommand(request.Name)));

   
    [Authorize ]
    [HttpPost("{id}/details")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateDetails([FromRoute] int id, [FromBody] CreateSessionDetailRequestDto request)
        => StatusCode(201, await Mediator.Send(new CreateSessionDetailCommand(id, request)));

   
    [HttpGet]
    [Authorize ]
    public async Task<IActionResult> GetAll()
        => StatusCode(200, await Mediator.Send(new GetAllSessionQuery()));

   
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new GetSessionQuery(id)));

    [Authorize ]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
           => StatusCode(200, await Mediator.Send(new DeleteSessionCommand(id)));

    [Authorize ]
    [HttpDelete("{sessionId}/details/{sessionDetailId}")]
    public async Task<IActionResult> DeleteSessionDetail([FromRoute] int sessionId, [FromRoute] int sessionDetailId)
        => StatusCode(200, await Mediator.Send(new DeleteSessionDetailCommand(sessionId, sessionDetailId)));
}
