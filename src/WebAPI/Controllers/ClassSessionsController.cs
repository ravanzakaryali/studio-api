using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class ClassSessionsController : BaseApiController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet("/api/class-sessions/{id}")]
    public async Task<IActionResult> GetClassSessionDetail([FromRoute] int id, [FromQuery] DateOnly date)
    {
        return Ok(await Mediator.Send(new GetClassSessionsByDateQuery(id, date)));
    }

    [HttpPost("/api/session-extensions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> SessionExtension([FromBody] SessionExtensionsRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionExtensionCommand(request.Hours, request.ClassId, request.RoomId, request.Sessions, request.StartDate));
        return NoContent();
    }

    [HttpPut("/api/class-sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassSessionAttendance([FromBody] UpdateClassSessionOneDayRequestDto request)
    {
        await Mediator.Send(new UpdateClassSessionOneDayCommand(request.ClassId, request.OldDate, request.NewDate, request.Sessions));
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
    //            RoleId = new int("39489493-d615-49e2-a0ce-507eaf38f234"),
    //            WorkerId = item.WorkerId
    //        });
    //    }
    //    await SpaceDbContext.SaveChangesAsync();
    //    return Ok();
    //}
    //public class ClassSessionImport
    //{
    //    public DateTime Date { get; set; }
    //    public int ClassId { get; set; }
    //    public int WorkerId { get; set; }
    //    public int TotalHour { get; set; }
    //}
}
