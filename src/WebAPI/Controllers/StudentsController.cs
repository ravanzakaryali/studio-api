using Space.Application.DTOs.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;
public class StudentsController : BaseApiController
{
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpGet("{id}/attendances")]
    public async Task<IActionResult> GetAttendancesStudentByClass([FromRoute] Guid id, [FromQuery] Guid classId)
    {

        return StatusCode(200, await Mediator.Send(new GetStudentAttendancesByClassQuery(id, classId)));
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        return StatusCode(200, await Mediator.Send(new GetAllStudentsQuery()));
    }


}

