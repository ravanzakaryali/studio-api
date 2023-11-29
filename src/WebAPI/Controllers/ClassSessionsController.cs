using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Class sessions controller
/// </summary>
[Authorize(Roles = "admin")]
public class ClassSessionsController : BaseApiController
{

    /// <summary>
    /// Retrieves details of a specific class session based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class session to retrieve details for.</param>
    /// <param name="date">The date of the class session to retrieve details for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) containing details of the specified class session upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve details of a specific class session based on its unique identifier
    /// and date. It returns a 200 status code along with the requested class session details upon successful retrieval.
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("/api/class-sessions/{id}")]
    public async Task<IActionResult> GetClassSessionDetail([FromRoute] Guid id, [FromQuery] DateOnly date)
    {
        return Ok(await Mediator.Send(new GetClassSessionsByDateQuery(id, date)));
    }

    /// <summary>
    /// Extends a class session by a specified number of hours and updates session details.
    /// </summary>
    /// <param name="request">A JSON object containing details for extending the class session.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful extension of the class session.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to extend a class session by a specified number of hours and update
    /// session details by providing a JSON object with the necessary details. It returns a 204 status code upon
    /// successful extension of the class session.
    /// </remarks>
    [HttpPost("/api/session-extensions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> SessionExtension([FromBody] SessionExtensionsRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionExtensionCommand(request.Hours, request.ClassId, request.RoomId, request.Sessions, request.StartDate));
        return NoContent();
    }

    //[HttpPost("bulk-import")]
    //public async Task<IActionResult> Create(IEnumerable<ClassSessionImport> import)
    //{
    //    var classSessions = await SpaceDbContext.ClassSessions.Include(c => c.AttendancesWorkers).Where(c => c.Category == Domain.Enums.ClassSessionCategory.Theoric).ToListAsync();
    //    foreach (ClassSessionImport item in import)
    //    {
    //        var session = classSessions.FirstOrDefault(c => c.Date == item.Date && c.ClassId == item.ClassId);
    //        session?.AttendancesWorkers.Add(new Domain.Entities.AttendanceWorker()
    //        {
    //            ClassSessionId = session.Id,
    //            TotalAttendanceHours = session.TotalHour,
    //            RoleId = new Guid("39489493-d615-49e2-a0ce-507eaf38f234"),
    //            WorkerId = item.WorkerId
    //        });
    //    }
    //    await SpaceDbContext.SaveChangesAsync();
    //    return Ok();
    //}
    //public class ClassSessionImport
    //{
    //    public DateTime Date { get; set; }
    //    public Guid ClassId { get; set; }
    //    public Guid WorkerId { get; set; }
    //    public int TotalHour { get; set; }
    //}
}
