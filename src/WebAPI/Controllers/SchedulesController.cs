using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;


[Authorize(Roles = "admin")]
public class SchedulesController : BaseApiController
{
    [HttpGet("rooms")]
    public async Task<IActionResult> GetSchedulesRoomsAsync([FromQuery] int? year)
        => Ok(await Mediator.Send(new GetSchedulesRoomsQuery(year)));

    [HttpGet("workers")]
    public async Task<IActionResult> GetSchedulesWorkersAsync()
        => Ok(await Mediator.Send(new GetSchedulesWorkersQuery()));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ScheduleGenerate()
    {
        await Mediator.Send(new CreateRoomScheduleByClassCommand());
        return StatusCode(StatusCodes.Status201Created);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedules-of-days")]
    public async Task<IActionResult> GetScheduleByNow()
    {
        return StatusCode(200, await Mediator.Send(new GetScheduleByDayQuery()));
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-rooms-byweeks")]
    public async Task<IActionResult> GetScheduleOfRoomsByWeeks()
    {
        return StatusCode(200, await Mediator.Send(new GetRoomScheduleSessionsQuery()));
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-classes-byweeks")]
    public async Task<IActionResult> GetScheduleOfClassesByWeeks()
    {
        return StatusCode(200, await Mediator.Send(new GetClassSchedulesQuery()));
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-workers")]
    public async Task<IActionResult> GetScheduleOfWorkers()
    {
        return StatusCode(200, await Mediator.Send(new GetWorkerClassSchedulesQuery()));
    }
}

