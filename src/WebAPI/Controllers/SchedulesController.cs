using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Schedule controller
/// </summary>
[Authorize(Roles = "admin")]
public class SchedulesController : BaseApiController
{
    /// <summary>
    /// Retrieves a list of scheduled rooms based on an optional year filter.
    /// </summary>
    /// <param name="year">An optional parameter to filter scheduled rooms by year.</param>
    /// <returns>
    /// An HTTP response containing a list of scheduled rooms upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to retrieve a list of scheduled rooms. It accepts an optional parameter, "year," to
    /// filter scheduled rooms based on the specified year. If no year is provided, it retrieves all scheduled rooms.
    /// It returns a list of scheduled rooms as an HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet("rooms")]
    public async Task<IActionResult> GetSchedulesRoomsAsync([FromQuery] int? year)
        => Ok(await Mediator.Send(new GetSchedulesRoomsQuery(year)));

    /// <summary>
    /// Retrieves a list of scheduled workers.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a list of scheduled workers upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to retrieve a list of scheduled workers. It returns a list of scheduled workers as an
    /// HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet("workers")]
    public async Task<IActionResult> GetSchedulesWorkersAsync()
        => Ok(await Mediator.Send(new GetSchedulesWorkersQuery()));

    /// <summary>
    /// Generates room schedules for classes and sessions.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful generation of room schedules.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to generate room schedules for classes and sessions. It triggers the creation of
    /// room schedules by sending a command to the mediator. Upon successful generation, it returns an HTTP response
    /// with a status code 201 (Created) to indicate the success of the operation.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ScheduleGenerate()
    {
        await Mediator.Send(new CreateRoomScheduleByClassCommand());
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Retrieves room schedules for the current day.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of room schedules for the current day.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to retrieve room schedules for the current day. It sends a query to the mediator
    /// to fetch room schedules. Upon successful retrieval, it returns an HTTP response with a status code 200 (OK)
    /// to indicate the success of the operation.
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedules-of-days")]
    public async Task<IActionResult> GetScheduleByNow()
    {
        return StatusCode(200, await Mediator.Send(new GetScheduleByDayQuery()));
    }

    /// <summary>
    /// Retrieves room schedules for multiple weeks.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of room schedules for multiple weeks.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to retrieve room schedules for multiple weeks. It sends a query to the mediator to
    /// fetch room schedules. Upon successful retrieval, it returns an HTTP response with a status code 200 (OK) to
    /// indicate the success of the operation.
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-rooms-byweeks")]
    public async Task<IActionResult> GetScheduleOfRoomsByWeeks()
    {
        return StatusCode(200, await Mediator.Send(new GetRoomScheduleSessionsQuery()));
    }

    /// <summary>
    /// Retrieves class schedules for multiple weeks.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of class schedules for multiple weeks.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to retrieve class schedules for multiple weeks. It sends a query to the mediator to
    /// fetch class schedules. Upon successful retrieval, it returns an HTTP response with a status code 200 (OK) to
    /// indicate the success of the operation.
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-classes-byweeks")]
    public async Task<IActionResult> GetScheduleOfClassesByWeeks()
    {
        return StatusCode(200, await Mediator.Send(new GetClassSchedulesQuery()));
    }

    /// <summary>
/// Retrieves schedules of workers.
/// </summary>
/// <returns>
/// An HTTP response with a status code 200 (OK) upon successful retrieval of schedules of workers.
/// </returns>
/// <remarks>
/// This endpoint allows users to retrieve schedules of workers. It sends a query to the mediator to fetch
/// schedules of workers. Upon successful retrieval, it returns an HTTP response with a status code 200 (OK)
/// to indicate the success of the operation.
/// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("schedule-of-workers")]
    public async Task<IActionResult> GetScheduleOfWorkers()
    {
        return StatusCode(200, await Mediator.Send(new GetWorkerClassSchedulesQuery()));
    }
}

