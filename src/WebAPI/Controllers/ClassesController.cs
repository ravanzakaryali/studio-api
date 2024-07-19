namespace Space.WebAPI.Controllers.v2;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    [HttpGet("{id}/students")]
    public async Task<IActionResult> GetStudentsByClass([FromRoute] int id, [FromQuery] DateTime date)
            => Ok(await Mediator.Send(new GetAllStudentsByClassQuery(id, date)));

    // GET: api/Classes/999/modules - Returns the modules of the class
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
          => Ok(await Mediator.Send(new GetAllModulesByClassQuery(id, date)));

    // GET: api/Classes/999/held-modules?date=2023-12-12 (keçirilmiş modullar) - Returns the held modules of the class (Tarixə uyğun olaraq )
    [HttpGet("{id}/held-modules/admin")]
    public async Task<IActionResult> GetHeldModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id,
            Date = date
        }));

    [HttpGet("{id}/held-modules")]
    public async Task<IActionResult> GetHeldModulesByClassAdmin([FromRoute] int id)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id,
            Date = DateTime.Now,
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

    // GET: api/Classes/999/sessions-category - Returns the session category of the class by date
    [HttpGet("{id}/sessions-category")]
    public async Task<IActionResult> GetSessionCategoryHoursAdmin([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassCategoryHoursQuery(id, date)));

    // [HttpGet("{id}/sessions-category")]
    // public async Task<IActionResult> GetSessionCategoryHours([FromRoute] int id)
    //     => Ok(await Mediator.Send(new GetClassCategoryHoursQuery(id, new DateTime())));

    // GET: api/Classes/999/class-session - Returns the class session of the class by date 
    // UI link - /admin/app/classes/373/class-sessions/by-day/2023-09-25
    [HttpGet("{id}/class-session")]
    public async Task<IActionResult> GetClassSession([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassSessionByClassQuery(id, date)));

    // GET: api/Classes/999/program - Returns the program of the class
    [HttpGet("{id}/program")]
    public async Task<IActionResult> GetClassByProgram([FromRoute] int id)
    => Ok(await Mediator.Send(new GetClassByProgramQuery(id)));


    // GET: api/Classes/export/excel - Exports the students of the classes to excel
    [HttpGet("export/excel")]
    public async Task ClassesExcelExport(
            [FromQuery] ClassStatus status,
        [FromQuery] List<int>? ClassIds,
        [FromQuery] List<int>? ProgramIds,
        [FromQuery] DateTime? StartDate,
        [FromQuery] DateTime? EndDate)
    {
        _ = await Mediator.Send(new StudentsOfClassesExcelExportCommand()
        {
            ClassStatus = status,
            ClassIds = ClassIds,
            ProgramIds = ProgramIds,
            StartDate = StartDate,
            EndDate = EndDate
        });
    }

    // GET: api/Classes/999/export/excel - Exports the students of the class to excel
    [HttpGet("{id}/export/excel")]
    public async Task ClassExcelExport([FromRoute] int id)
    {
        _ = await Mediator.Send(new StudentsofClassExcelExportCommand()
        {
            ClassId = id
        });
    }

    // POST: api/Classes/999/attendances - Creates the attendances of the class
    [HttpPost("{id}/attendances")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassAttendances([FromRoute] int id, [FromBody] CreateClassAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionAttendanceCommand()
        {
            ClassId = id,
            Date = request.Date,
            Sessions = request.Sessions,
            HeldModules = request.HeldModules,
        });
        return StatusCode(StatusCodes.Status204NoContent);
    }

    // POST: api/Classes/999/sessions - Creates the sessions of the class
    [HttpPost("{id}/sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateClassModuleSessionRequestDto request)
    {
        await Mediator.Send(new CreateClassModuleSessionCommand()
        {
            ClassId = id,
            CreateClassModuleSessionDto = request
        });
        return NoContent();
    }

    // PUT: api/Classes/999/sessions - Updates the sessions of the class
    [HttpPut("{id}/sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassSessionAttendance([FromRoute] int id, [FromBody] UpdateClassSessionByDateRequestDto request)
    {
        await Mediator.Send(new UpdateClassSessionByDateCommand()
        {
            ClassId = id,
            EndDate = request.EndDate,
            Sessions = request.Sessions,
            StartDate = request.StartDate
        });
        return NoContent();
    }

    // POST: api/Classes/999/class-session - Creates the class session of the class
    [HttpPost("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassSession([FromRoute] int id, [FromBody] CreateClassSessionByClassRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionByClassCommand()
        {
            Id = id,
            Session = request
        });
        return StatusCode(StatusCodes.Status201Created);
    }

    // PUT: api/Classes/999/modules-workers - Updates the modules and workers of the class
    [HttpPut("{id}/modules-workers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassModulesWorkers([FromRoute] int id, [FromBody] UpdateClassModuleRequestDto request)
    {
        await Mediator.Send(new UpdateClassModuleCommand()
        {
            Id = id,
            Modules = request.Modules,
            ExtraModules = request.ExtraModules,
            NewExtraModules = request.NewExtraModules
        });
        return NoContent();
    }


    // POST: api/Classes/999/modules-workers - Creates the modules and workers of the class
    [HttpPost("{id}/modules-workers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassModulesWorkers([FromRoute] int id, IEnumerable<CreateClassModuleRequestDto> modules)
    {
        await Mediator.Send(new CreateClassModuleCommand()
        {
            ClassId = id,
            CreateClassModule = modules,
        });
        return NoContent();
    }

    // PUT: api/Classes/999/class-session - Updates the class session of the class
    [HttpPut("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassSession([FromRoute] int id, [FromQuery] DateTime date, [FromBody] IEnumerable<UpdateClassSessionRequestDto> request)
    {
        await Mediator.Send(new UpdateClassSessionCommand(id, date, request));
        return NoContent();
    }


    [HttpPost("{id}/session-cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CancelClassSession([FromRoute] int id)
    {
        await Mediator.Send(new CancelledAttendanceCommand(id, DateTime.Now));
        return NoContent();
    }

    [HttpPost("{id}/session-cancel/admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CancelClassSessionAdmin([FromRoute] int id, [FromQuery] DateTime date)
    {
        await Mediator.Send(new CancelledAttendanceCommand(id, date));
        return NoContent();
    }

    #region Comments Endpoints
    // // POST: api/Classes - Creates a new class
    // [Authorize]
    // [HttpPost]
    // [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<CreateClassRequestDto>))]
    // [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    // [ProducesDefaultResponseType]
    // public async Task<IActionResult> Create([FromBody] CreateClassRequestDto request)
    //           => StatusCode(StatusCodes.Status201Created, await Mediator.Send(new CreateClassCommand(
    //               request.Name,
    //               request.ProgramId,
    //               request.SessionId,
    //               request.RoomId
    //               )));

    // // PUT: api/Classes/999 - Updates the class
    // [Authorize]
    // [HttpPut("{id}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    // [ProducesDefaultResponseType]
    // public async Task<IActionResult> Update(int id, UpdateClassRequestDto request)
    // {
    //     await Mediator.Send(new UpdateClassCommand(id,
    //         request.Name,
    //         request.SessionId,
    //         request.ProgramId,
    //         request.RoomId));
    //     return StatusCode(StatusCodes.Status204NoContent);
    // }

    // // DELETE: api/Classes/999 - Deletes the class
    // [Authorize]
    // [HttpDelete("{id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    // [ProducesDefaultResponseType]
    // public async Task<IActionResult> Delete([FromRoute] int id)
    //     => StatusCode(StatusCodes.Status200OK, await Mediator.Send(new DeleteClassCommand(id)));

    // [Authorize ]
    // [HttpGet("{id}/class-sessions")]
    // public async Task<IActionResult> GetAllClassSessions([FromRoute] int id)
    //     => Ok(await Mediator.Send(new GetAllClassSessionsByClassQuery(id)));
    #endregion
}
