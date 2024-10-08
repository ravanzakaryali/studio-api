﻿namespace Space.WebAPI.Controllers;


public class StudentsController : BaseApiController
{
    [Authorize]
    [HttpGet("{id}/attendances")]
    public async Task<IActionResult> GetAttendancesStudentByClass([FromRoute] int id, [FromQuery] int classId)
    {
        return StatusCode(200, await Mediator.Send(new GetStudentAttendancesByClassQuery(id, classId)));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        return StatusCode(200, await Mediator.Send(new GetAllStudentsQuery()));
    }

}

