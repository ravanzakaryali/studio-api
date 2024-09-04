namespace Space.WebAPI.Controllers;

[Authorize]
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
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoomRequestDto request)
       => StatusCode(200, await Mediator.Send(new UpdateRoomCommand(id, request)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new DeleteRoomCommand(id)));

    [HttpGet("planning")]
    public async Task<IActionResult> GetRoomPlaning([FromQuery] List<int> sessions, [FromQuery] List<int> rooms)
    {
        return StatusCode(200, await Mediator.Send(new GetRoomPlaningQuery()
        {
            SessionIds = sessions,
            RoomIds = rooms,
            Year = DateTime.Now.Year
        }));
    }

}
