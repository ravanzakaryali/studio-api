namespace Space.WebAPI.Controllers;

public class AttendancesController : BaseApiController
{
    //POST: api/attendance - Create Attendances
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAttendances([FromBody] CreateClassSessionAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionAttendanceCommand()
        {
            ClassId = request.ClassId,
            HeldModules = request.HeldModules,
            Sessions = request.Sessions
        });
        return NoContent();
    }

    //GET: api/attendance/{classId}/held-modules
    [Authorize]
    [HttpGet("{classId}/held-modules")]
    public async Task<IActionResult> GetHeldModules([FromRoute] int classId)
       => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
       {
           Id = classId
       }));

    //GET: api/attendance/{classId}/sessions
    [HttpGet("classes/{classId}/workers")]
    public async Task<IActionResult> GetWorkersByClass([FromRoute] int classId)
      => Ok(await Mediator.Send(new GetAllWorkersByClassQuery(classId)));
}