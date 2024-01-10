namespace Space.WebAPI.Controllers;

public class AttendancesController : BaseApiController
{
    [HttpPost]
    [Authorize(Roles = "mentor,muellim,ta,admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAttendances([FromBody] CreateClassSessionAttendanceRequestDto request)
    {
        await Mediator.Send(new CreateClassSessionAttendanceCommand()
        {
            ClassId = request.ClassId,
            HeldModules = request.HeldModules,
            Sessions = request.Sessions
        });
        return NoContent();
    }

}