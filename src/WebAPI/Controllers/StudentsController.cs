namespace Space.WebAPI.Controllers;


/// <summary>
/// Student controller
/// </summary>
public class StudentsController : BaseApiController
{
    /// <summary>
    /// Retrieves student attendances for a specific session within a class.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve attendances for.</param>
    /// <param name="classId">The unique identifier of the class containing the session.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of student attendances.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with roles "admin," "ta," "mentor," or "muellim" to retrieve attendances
    /// for a specific student within a class session. Users can specify the unique identifier (ID) of both the student
    /// and the class containing the session. It sends a query to the mediator to fetch the student's attendances.
    /// Upon successful retrieval, it returns an HTTP response with a status code 200 (OK) to indicate the success of
    /// the operation.
    /// </remarks>
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpGet("{id}/attendances")]
    public async Task<IActionResult> GetAttendancesStudentByClass([FromRoute] Guid id, [FromQuery] Guid classId)
    {

        return StatusCode(200, await Mediator.Send(new GetStudentAttendancesByClassQuery(id, classId)));
    }

    /// <summary>
    /// Retrieves a list of all students.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of all students.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve a list of all students. It sends a query
    /// to the mediator to fetch all student records. Upon successful retrieval, it returns an HTTP response with a
    /// status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        return StatusCode(200, await Mediator.Send(new GetAllStudentsQuery()));
    }

}

