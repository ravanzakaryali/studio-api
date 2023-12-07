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
    public async Task<IActionResult> GetAll([FromQuery] ClassStatus status = ClassStatus.Active)
            => Ok(await Mediator.Send(new GetAllClassQuery(status)));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassDetail([FromRoute] Guid id, [FromQuery] Guid? sessionId)
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
    public async Task<IActionResult> GetSessionByClass([FromRoute] Guid id)
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
    public async Task<IActionResult> Update(Guid id, UpdateClassRequestDto request)
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
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => StatusCode(StatusCodes.Status200OK, await Mediator.Send(new DeleteClassCommand(id)));

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModulesByClass([FromRoute] Guid id, [FromQuery] DateTime date)
      => Ok(await Mediator.Send(new GetAllModulesByClassQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/held-modules")]
    public async Task<IActionResult> GetHeldModules([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetHeldModulesByClassQuery()
        {
            Id = id
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/modules-workers")]
    public async Task<IActionResult> GetClassModulesWorkers([FromRoute] Guid id, [FromQuery] Guid sessionId)
    {
        return Ok(await Mediator.Send(new GetClassWorkersModulesQuery(id, sessionId)));
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/modules-workers")]
    public async Task<IActionResult> UpdateClassModulesWorkers([FromRoute] Guid id, IEnumerable<CreateClassModuleRequestDto> modules)
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
    public async Task<IActionResult> GetStudentsByClass([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllStudentsByClassQuery(id, date)));

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/students")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateStudentsByClass([FromRoute] Guid id, [FromBody] IEnumerable<CreateStudentsDto> students)
    {
        await Mediator.Send(new CreateStudentComand()
        {
            ClassId = id,
            Students = students
        });
        return NoContent();
    }

    /// <summary>
    /// Retrieves detailed information about students related to a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve student details.</param>
    /// <returns>
    /// An HTTP response containing detailed information about students related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve detailed information
    /// about students associated with a class based on the class's unique identifier.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students-details")]
    public async Task<IActionResult> GetStudentsDetailsByClass([FromRoute] Guid id)
       => Ok(await Mediator.Send(new GetAllStudentsDetailsByClassQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students-absent")]
    public async Task<IActionResult> GetAllAbsentStudents([FromRoute] Guid id)
     => Ok(await Mediator.Send(new GetAllAbsentStudentsQuery(id)));

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/workers")]
    public async Task<IActionResult> GetWorkersByClass([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllWorkersByClassQuery(id, date)));


    [Authorize(Roles = "admin")]
    [HttpGet("{id}/workers/{workerId}")]
    public async Task<IActionResult> GetWorkerByClass(
        [FromRoute] Guid id,
        [FromRoute] Guid workerId,
        [FromQuery] DateOnly date,
        [FromQuery] Guid roleId)
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
    public async Task<IActionResult> GetClassCounterHour([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetClassCounterHourQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/sessions-category")]
    public async Task<IActionResult> GetSessionCategoryHours([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassCategoryHoursQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-sessions")]
    public async Task<IActionResult> GetAllClassSessions([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetAllClassSessionsByClassQuery(id)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-session")]
    public async Task<IActionResult> GetClassSession([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassSessionByClassQuery(id, date)));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpPut("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassSession([FromRoute] Guid id, [FromQuery] DateTime date, [FromBody] IEnumerable<UpdateClassSessionRequestDto> request)
    {
        await Mediator.Send(new UpdateClassSessionCommand(id, date, request));
        return NoContent();
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/program")]
    public async Task<IActionResult> GetClassByProgram([FromRoute] Guid id)
    => Ok(await Mediator.Send(new GetClassByProgramQuery(id)));


    [Authorize(Roles = "admin")]
    [HttpPost("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateClassSession([FromRoute] Guid id, [FromBody] CreateClassSessionByClassRequestDto request)
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
    public async Task<IActionResult> CreateClassAttendances([FromRoute] Guid id, [FromBody] CreateClassAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassAttendanceCommand()
        {
            ClassId = id,
            Date = request.Date,
            Sessions = request.Sessions,
        });
        return StatusCode(StatusCodes.Status204NoContent);
    }


    [Authorize(Roles = "admin")]
    [HttpPost("{id}/sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateClassModuleSessionRequestDto request)
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
    public async Task<IActionResult> CreateClassSessionAttendance([FromRoute] Guid id, [FromBody] UpdateClassSessionByDateRequestDto request)
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
    public async Task<IActionResult> GetAttendanceRateByClass([FromRoute] Guid id, [FromQuery] MonthOfYear month, [FromQuery] int year)
        => Ok(await Mediator.Send(new GetAttendanceRateByClassQuery()
        {
            Id = id,
            MonthOfYear = month,
            Year = year
        }));
    [Authorize]
    [HttpGet("{id}/unmarked-attendance-days")]
    public async Task<IActionResult> GetUnnotedAttendanceDays([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetUnattendedDaysByClassQuery()
        {
            Id = id
        }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/export/excel")]
    public async Task ClassExcelExport([FromRoute] Guid id)
    {
        _ = await Mediator.Send(new StudentsofClassExcelExportCommand()
        {
            ClassId = id
        });

    }

}
