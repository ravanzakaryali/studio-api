namespace Space.WebAPI.Controllers.v2;

[Route("api/[controller]")]
[ApiController]
public class ClassesController : BaseApiController
{

    // GET: api/Classes/999 - Returns the class details
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassDetail([FromRoute] int id)
            => Ok(await Mediator.Send(new GetClassDetailQuery()
            {
                Id = id,
            }));

    // GET: api/Classes - Returns the classes according to the status of the class (Active, New, Completed) and other filters
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetClassModuleWorkers>))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll(
            [FromQuery] ClassStatus status = ClassStatus.Active,
            [FromQuery] int? programId = null,
            [FromQuery] int? studyCount = null,
            [FromQuery] StudyCountStatus? studyCountStatus = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? teacherId = null,
            [FromQuery] int? mentorId = null,
            [FromQuery] int? startAttendancePercentage = null,
            [FromQuery] int? endAttendancePercentage = null)
                => Ok(await Mediator.Send(new GetAllClassQuery(
                    status,
                    programId,
                    studyCount,
                    studyCountStatus,
                    startDate,
                    endDate,
                    teacherId,
                    mentorId,
                    startAttendancePercentage,
                    endAttendancePercentage)));

    // GET: api/Classes/count - Returns the number according to the status of the class (Active, New, Completed)
    [HttpGet("count")]
    public async Task<IActionResult> GetClassesCount()
        => Ok(await Mediator.Send(new GetClassesCountQuery()));

    // GET: api/Classes/999/students-details - Returns the students of the class
    [HttpGet("{id}/students-details")]
    public async Task<IActionResult> GetStudentsDetailsByClass([FromRoute] int id)
        => Ok(await Mediator.Send(new GetAllStudentsDetailsByClassQuery(id)));

    // GET: api/Classes/999/students - Returns the students of the class
    public async Task<IActionResult> GetStudentsByClass([FromRoute] int id, [FromQuery] DateTime date)
            => Ok(await Mediator.Send(new GetAllStudentsByClassQuery(id, date)));

    // GET: api/Classes/999/modules - Returns the modules of the class
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
          => Ok(await Mediator.Send(new GetAllModulesByClassQuery(id, date)));

    // GET: api/Classes/999/held-modules?date=2023-12-12 (keçirilmiş modullar) - Returns the held modules of the class (Tarixə uyğun olaraq )
    [HttpGet("{id}/held-modules")]
    public async Task<IActionResult> GetHeldModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id,
            Date = date
        }));

    //Todo: Session Id
    //GET: api/Classes/999/modules-workers - Returns the modules and workers of the class (Hansı müəllim hansı modula assign olub)
    [HttpGet("{id}/modules-workers")]
    public async Task<IActionResult> GetClassModulesWorkers([FromRoute] int id, [FromQuery] int? sessionId)
    {
        return Ok(await Mediator.Send(new GetClassWorkersModulesQuery(id, sessionId)));
    }

    // GET: api/Classes/999/workers - Returns the workers of the class by date
    [HttpGet("{id}/workers")]
    public async Task<IActionResult> GetWorkersByClass([FromRoute] int id, [FromQuery] DateTime date)
       => Ok(await Mediator.Send(new GetAllWorkersByClassQuery(id, date)));

    // GET: api/Classes/999/students-absent - Returns the absent students of the class
    [HttpGet("{id}/students-absent")]
    public async Task<IActionResult> GetAllAbsentStudents([FromRoute] int id)
        => Ok(await Mediator.Send(new GetAllAbsentStudentsQuery(id)));

    // GET: api/Classes/999/attendance-rate?month=1&year=2024 - Returns the attendance rate of the class
    [HttpGet("{id}/attendance-rate")]
    public async Task<IActionResult> GetAttendanceRateByClass([FromRoute] int id, [FromQuery] MonthOfYear month, [FromQuery] int year)
        => Ok(await Mediator.Send(new GetAttendanceRateByClassQuery()
        {
            Id = id,
            MonthOfYear = month,
            Year = year
        }));

    // GET: api/Classes/999/unmarked-attendance-day - Returns the days that the class has not been marked
    [HttpGet("{id}/unmarked-attendance-days")]
    public async Task<IActionResult> GetUnnotedAttendanceDays([FromRoute] int id)
            => Ok(await Mediator.Send(new GetUnattendedDaysByClassQuery()
            {
                Id = id
            }));


}