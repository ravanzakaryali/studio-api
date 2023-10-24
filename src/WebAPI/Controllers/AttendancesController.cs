using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Attendances controller
/// </summary>
public class AttendancesController : BaseApiController
{
    /// <summary>
    /// Used to create attendance records for a specific class session. Only users with 'mentor' 'muellim' and 'admin' roles are allowed to perform this operation.
    /// </summary>
    /// <param name="request">The request object containing the data needed to create attendance records.</param>
    /// <returns>If successful, returns a 204 No Content response.</returns>
    [HttpPost]
    [Authorize(Roles = "mentor,muellim,ta,admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAttendances([FromBody] CreateClassSessionAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionAttendanceCommand()
        {
            ClassId = request.ClassId,
            Date = request.Date,
            ModuleId = request.ModuleId,
            Sessions = request.Sessions
        });
        return NoContent();
    }

}