using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

public class AttendancesController : BaseApiController
{
    [HttpPost]
    [Authorize(Roles = "mentor,muellim,ta,admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAttendances([FromBody] UpdateClassSessionAttendanceRequestDto request)
    {
        await Mediator.Send(new UpdateClassSessionAttendanceCommand()
        {
            ClassId = request.ClassId,
            Date = request.Date,
            ModuleId = request.ModuleId,
            Sessions = request.Sessions
        });
        return NoContent();
    }
}