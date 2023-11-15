using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Space.Application.DTOs.Enums;
using Space.Domain.Enums;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Class controller
/// </summary>
public class ClassesController : BaseApiController
{
    /// <summary>
    /// Retrieves a list of class modules for workers, accessible only to users with the "admin" role.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a JSON array of class modules for workers upon successful retrieval.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetClassModuleWorkers>))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll([FromQuery] ClassStatus status = ClassStatus.Active)
            => Ok(await Mediator.Send(new GetAllClassQuery(status)));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassDetail([FromRoute] Guid id, [FromQuery] Guid? sessionId)
        => Ok(await Mediator.Send(new GetClassDetailQuery()
        {
            Id = id,
            SessionId = sessionId
        }));


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "admin")]
    [HttpGet("count")]
    public async Task<IActionResult> GetClassesCount()
        => Ok(await Mediator.Send(new GetClassesCountQuery()));
    /// <summary>
    /// Retrieves information about a specific class module based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class module to retrieve.</param>
    /// <returns>
    /// An HTTP response containing information about the requested class module upon successful retrieval.
    /// </returns>


    /// <summary>
    /// Retrieves sessions related to a specific class module based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class module for which to retrieve sessions.</param>
    /// <returns>
    /// An HTTP response containing sessions related to the specified class module upon successful retrieval.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpGet("{id}/sessions")]
    public async Task<IActionResult> GetSessionByClass([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetSessionByClassQuery(id)));

    /// <summary>
    /// Updates the "IsNew" status of a class module, accessible only to users with the "admin" role.
    /// </summary>
    /// <param name="request">A JSON object containing the unique identifier of the class module.</param>
    /// <returns>
    /// An HTTP response indicating the success of the "IsNew" status update.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpPost("updateIsNew")]
    public async Task<IActionResult> ClassIsNew([FromBody] UpdateIsNewInClassRequestDto request)
        => Ok(await Mediator.Send(new UpdateIsNewInClassCommand(request.Id)));

    /// <summary>
    /// Creates a new class, accessible only to users with the "admin" role.
    /// </summary>
    /// <param name="request">A JSON object containing details for creating a class.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) and the created class details upon successful creation.
    /// </returns>
    /// <remarks>
    /// This endpoint expects a JSON object in the request body with the following properties:
    /// - Name: The name of the class.
    /// - ProgramId: The unique identifier of the program to which the class belongs.
    /// - SessionId: The unique identifier of the session associated with the class.
    /// - RoomId: The unique identifier of the room where the class is held.
    /// </remarks>
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

    /// <summary>
    /// Updates an existing class, accessible only to users with the "admin" role.
    /// </summary>
    /// <param name="id">The unique identifier of the class to update.</param>
    /// <param name="request">A JSON object containing details for updating the class.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful update of the class.
    /// </returns>
    /// <remarks>
    /// This endpoint expects a JSON object in the request body with the following properties:
    /// - Name: The updated name of the class.
    /// - ProgramId: The updated unique identifier of the program to which the class belongs.
    /// - SessionId: The updated unique identifier of the session associated with the class.
    /// - RoomId: The updated unique identifier of the room where the class is held.
    /// </remarks>
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

    /// <summary>
    /// Deletes an existing class, accessible only to users with the "admin" role.
    /// </summary>
    /// <param name="id">The unique identifier of the class to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful deletion of the class.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => StatusCode(StatusCodes.Status200OK, await Mediator.Send(new DeleteClassCommand(id)));

    /// <summary>
    /// Retrieves modules related to a specific class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve modules.</param>
    /// <param name="date">The date for filtering modules (optional).</param>
    /// <returns>
    /// An HTTP response containing modules related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve modules
    /// associated with a class based on the class's unique identifier. The optional 'date' parameter can
    /// be used to filter modules by date.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModulesByClass([FromRoute] Guid id, [FromQuery] DateTime date)
      => Ok(await Mediator.Send(new GetAllModulesByClassQuery(id, date)));


    [Authorize(Roles = "admin")]
    [HttpGet("{id}/modules-workers")]
    public async Task<IActionResult> GetClassModulesWorkers([FromRoute] Guid id, [FromQuery] Guid sessionId)
    {
        return Ok(await Mediator.Send(new GetClassWorkersModulesQuery(id, sessionId)));
    }

    /// <summary>
    /// Retrieves students related to a specific class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve students.</param>
    /// <param name="date">The date for filtering students (optional).</param>
    /// <returns>
    /// An HTTP response containing students related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve students
    /// associated with a class based on the class's unique identifier. The optional 'date' parameter can
    /// be used to filter students by date.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students")]
    public async Task<IActionResult> GetStudentsByClass([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllStudentsByClassQuery(id, date)));

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

    /// <summary>
    /// Retrieves a list of absent students related to a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve absent students.</param>
    /// <returns>
    /// An HTTP response containing a list of absent students related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve a list of absent students
    /// associated with a class based on the class's unique identifier.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/students-absent")]
    public async Task<IActionResult> GetAllAbsentStudents([FromRoute] Guid id)
     => Ok(await Mediator.Send(new GetAllAbsentStudentsQuery(id)));

    /// <summary>
    /// Retrieves workers related to a specific class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve workers.</param>
    /// <param name="date">The date for filtering workers (optional).</param>
    /// <returns>
    /// An HTTP response containing workers related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve workers
    /// associated with a class based on the class's unique identifier. The optional 'date' parameter can
    /// be used to filter workers by date.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/workers")]
    public async Task<IActionResult> GetWorkersByClass([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetAllWorkersByClassQuery(id, date)));

    /// <summary>
    /// Retrieves detailed information about a worker related to a specific class based on class and worker IDs,
    /// date, and role ID, accessible only to users with the "admin" role.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve the worker.</param>
    /// <param name="workerId">The unique identifier of the worker to retrieve.</param>
    /// <param name="date">The date for filtering the worker's information (optional).</param>
    /// <param name="roleId">The unique identifier of the role associated with the worker (optional).</param>
    /// <returns>
    /// An HTTP response containing detailed information about the worker related to the specified class upon successful retrieval.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpGet("{id}/workers/{workerId}")]
    public async Task<IActionResult> GetWorkerByClass(
        [FromRoute] Guid id,
        [FromRoute] Guid workerId,
        [FromQuery] DateTime date,
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

    /// <summary>
    /// Retrieves the class counter hour information for a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve the counter hour information.</param>
    /// <returns>
    /// An HTTP response containing the class counter hour information upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve the counter hour
    /// information for a class based on the class's unique identifier.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/counter")]
    public async Task<IActionResult> GetClassCounterHour([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetClassCounterHourQuery(id)));

    /// <summary>
    /// Retrieves session category hours for a specific class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve session category hours.</param>
    /// <param name="date">The date for filtering session category hours (optional).</param>
    /// <returns>
    /// An HTTP response containing session category hours related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve session category hours
    /// for a class based on the class's unique identifier. The optional 'date' parameter can be used to filter category
    /// hours by date.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/sessions-category")]
    public async Task<IActionResult> GetSessionCategoryHours([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassCategoryHoursQuery(id, date)));

    /// <summary>
    /// Retrieves all class sessions related to a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve class sessions.</param>
    /// <returns>
    /// An HTTP response containing all class sessions related to the specified class upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve all class sessions
    /// associated with a class based on the class's unique identifier.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-sessions")]
    public async Task<IActionResult> GetAllClassSessions([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetAllClassSessionsByClassQuery(id)));

    /// <summary>
    /// Retrieves a specific class session for a class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to retrieve the class session.</param>
    /// <param name="date">The date of the class session to retrieve.</param>
    /// <returns>
    /// An HTTP response containing the specified class session for the specified class and date upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve a specific class session
    /// associated with a class based on the class's unique identifier and date.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-session")]
    public async Task<IActionResult> GetClassSession([FromRoute] Guid id, [FromQuery] DateTime date)
        => Ok(await Mediator.Send(new GetClassSessionByClassQuery(id, date)));

    /// <summary>
    /// Updates a class session for a specific class based on its unique identifier and date.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to update the class session.</param>
    /// <param name="date">The date of the class session to update.</param>
    /// <param name="request">A collection of JSON objects containing details for updating the class session.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful update of the class session.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to update a class session
    /// associated with a class based on the class's unique identifier and date. The request should include a
    /// collection of JSON objects with details for updating the class session.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpPut("{id}/class-session")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateClassSession([FromRoute] Guid id, [FromQuery] DateTime date, [FromBody] IEnumerable<UpdateClassSessionRequestDto> request)
    {
        await Mediator.Send(new UpdateClassSessionCommand(id, date, request));
        return NoContent();
    }

    /// <summary>
    /// Retrieves classes associated with a specific program based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the program for which to retrieve classes.</param>
    /// <returns>
    /// An HTTP response containing classes associated with the specified program upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with roles "admin," "mentor," "ta," and "muellim" to retrieve classes
    /// associated with a program based on the program's unique identifier.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/class-by-program")]
    public async Task<IActionResult> GetClassByProgram([FromRoute] Guid id)
    => Ok(await Mediator.Send(new GetClassByProgramQuery(id)));

    /// <summary>
    /// Creates a class session for a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to create a class session.</param>
    /// <param name="request">A JSON object containing details for creating the class session.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the class session.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with the "admin" role to create a class session associated with a class
    /// based on the class's unique identifier. The request should include a JSON object with details for
    /// creating the class session.
    /// </remarks>
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

    /// <summary>
    /// Creates class attendances for a specific class based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class for which to create class attendances.</param>
    /// <param name="request">A JSON object containing details for creating class attendances.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful creation of class attendances.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users with the "admin" role to create class attendances associated with a class
    /// based on the class's unique identifier. The request should include a JSON object with details for
    /// creating the class attendances.
    /// </remarks>
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
            ModuleId = request.ModuleId,
        });
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Müəyyən tarix aralığında update etmək
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Class students export
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
