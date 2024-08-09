namespace Space.WebAPI.Controllers;

public class AttendancesController : BaseApiController
{

    [HttpPost("start")]
    [Authorize]
    public async Task<IActionResult> StartAttendance([FromBody] StartAttendanceRequestDto request)
    {
        return Ok(await Mediator.Send(new StartAttendanceCommand()
        {

            ClassId = request.ClassId,
            SessionCategory = request.SessionCategory,
            HeldModulesIds = request.HeldModulesIds,
            WorkerId = request.WorkerId
        }));
    }


    //POST: api/attendance - Create Attendances
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAttendances([FromBody] CreateAttendanceRequestDto request)
    {
        await Mediator.Send(new InsertAttendanceCommand()
        {
            ClassTimeSheetId = request.ClassTimeSheetsId,
            Attendances = request.Attendances
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