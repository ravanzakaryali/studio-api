using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Space.Application.DTOs.Enums;
using Space.Application.Enums;
using Space.Domain.Enums;

namespace Space.WebAPI.Controllers;

public class ClassesController : BaseApiController
{

    [Authorize(Roles = "admin")]
    [HttpGet]
    [Produces("application/json")]
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

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassDetail([FromRoute] int id, [FromQuery] int? sessionId)
        => Ok(await Mediator.Send(new GetClassDetailQuery()
        {
            Id = id,
            SessionId = sessionId
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("count")]
    public async Task<IActionResult> GetClassesCount()
        => Ok(await Mediator.Send(new GetClassesCountQuery()));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/sessions")]
    public async Task<IActionResult> GetSessionByClass([FromRoute] int id)
        => Ok(await Mediator.Send(new GetSessionByClassQuery(id)));


    //[Authorize(Roles = "admin")]
    //[HttpPost("updateIsNew")]
    //public async Task<IActionResult> ClassIsNew([FromBody] UpdateIsNewInClassRequestDto request)
    //    => Ok(await Mediator.Send(new UpdateIsNewInClassCommand(request.Id)));

    [Authorize(Roles = "admin")]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<CreateClassRequestDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateClassRequestDto request)
          => StatusCode(StatusCodes.Status201Created, await Mediator.Send(new CreateClassCommand(
              request.Name,
              request.ProgramId,
              request.SessionId,
              request.RoomId
              )));

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdateClassRequestDto request)
    {
        await Mediator.Send(new UpdateClassCommand(id,
            request.Name,
            request.SessionId,
            request.ProgramId,
            request.RoomId));
        return StatusCode(StatusCodes.Status204NoContent);
    }


    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => StatusCode(StatusCodes.Status200OK, await Mediator.Send(new DeleteClassCommand(id)));

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
      => Ok(await Mediator.Send(new GetAllModulesByClassQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/held-modules")]
    public async Task<IActionResult> GetHeldModules([FromRoute] int id)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/held-modules/admin")]
    public async Task<IActionResult> GetHeldModulesByClass([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id,
            Date = date
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/modules-workers")]
    public async Task<IActionResult> GetClassModulesWorkers([FromRoute] int id, [FromQuery] int sessionId)
    {
        return Ok(await Mediator.Send(new GetClassWorkersModulesQuery(id, sessionId)));
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/modules-workers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassModulesWorkers([FromRoute] int id, IEnumerable<CreateClassModuleRequestDto> modules)
    {
        await Mediator.Send(new CreateClassModuleCommand()
        {
            ClassId = id,
            CreateClassModule = modules
        });
        return NoContent();
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students")]
    public async Task<IActionResult> GetStudentsByClass([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllStudentsByClassQuery(id, date)));

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/students")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateStudentsByClass([FromRoute] int id, [FromBody] IEnumerable<CreateStudentsDto> students)
    {
        await Mediator.Send(new CreateStudentComand()
        {
            ClassId = id,
            Students = students
        });
        return NoContent();
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students-details")]
    public async Task<IActionResult> GetStudentsDetailsByClass([FromRoute] int id)
       => Ok(await Mediator.Send(new GetAllStudentsDetailsByClassQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students-absent")]
    public async Task<IActionResult> GetAllAbsentStudents([FromRoute] int id)
     => Ok(await Mediator.Send(new GetAllAbsentStudentsQuery(id)));

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/workers")]
    public async Task<IActionResult> GetWorkersByClass([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllWorkersByClassQuery(id, date)));


    [Authorize(Roles = "admin")]
    [HttpGet("{id}/workers/{workerId}")]
    public async Task<IActionResult> GetWorkerByClass(
        [FromRoute] int id,
        [FromRoute] int workerId,
        [FromQuery] DateOnly date,
        [FromQuery] int roleId)
    {
        return Ok(await Mediator.Send(new GetWorkerByClassQuery()
        {
            ClassId = id,
            WorkerId = workerId,
            Date = date,
            RoleId = roleId,
        }));
    }


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/counter")]
    public async Task<IActionResult> GetClassCounterHour([FromRoute] int id)
        => Ok(await Mediator.Send(new GetClassCounterHourQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/sessions-category")]
    public async Task<IActionResult> GetSessionCategoryHours([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassCategoryHoursQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-sessions")]
    public async Task<IActionResult> GetAllClassSessions([FromRoute] int id)
        => Ok(await Mediator.Send(new GetAllClassSessionsByClassQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-session")]
    public async Task<IActionResult> GetClassSession([FromRoute] int id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassSessionByClassQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpPut("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassSession([FromRoute] int id, [FromQuery] DateTime date, [FromBody] IEnumerable<UpdateClassSessionRequestDto> request)
    {
        await Mediator.Send(new UpdateClassSessionCommand(id, date, request));
        return NoContent();
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/program")]
    public async Task<IActionResult> GetClassByProgram([FromRoute] int id)
    => Ok(await Mediator.Send(new GetClassByProgramQuery(id)));


    [Authorize(Roles = "admin")]
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


    [Authorize(Roles = "admin")]
    [HttpPost("{id}/attendances")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassAttendances([FromRoute] int id, [FromBody] CreateClassAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassAttendanceCommand()
        {
            ClassId = id,
            Date = request.Date,
            Sessions = request.Sessions,
            HeldModules = request.HeldModules,
        });
        return StatusCode(StatusCodes.Status204NoContent);
    }


    [Authorize(Roles = "admin")]
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

    [Authorize(Roles = "admin")]
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
    [Authorize(Roles = "admin")]
    [HttpGet("{id}/attendance-rate")]
    public async Task<IActionResult> GetAttendanceRateByClass([FromRoute] int id, [FromQuery] MonthOfYear month, [FromQuery] int year)
        => Ok(await Mediator.Send(new GetAttendanceRateByClassQuery()
        {
            Id = id,
            MonthOfYear = month,
            Year = year
        }));
    [Authorize]
    [HttpGet("{id}/unmarked-attendance-days")]
    public async Task<IActionResult> GetUnnotedAttendanceDays([FromRoute] int id)
        => Ok(await Mediator.Send(new GetUnattendedDaysByClassQuery()
        {
            Id = id
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/export/excel")]
    public async Task ClassExcelExport([FromRoute] int id)
    {
        _ = await Mediator.Send(new StudentsofClassExcelExportCommand()
        {
            ClassId = id
        });

    }
    [Authorize(Roles = "admin")]
    [HttpGet("export/excel")]
    public async Task ClassesExcelExport()
    {

    }
}
