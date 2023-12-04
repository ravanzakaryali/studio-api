using Space.Application.DTOs.Program.Request;

namespace Space.WebAPI.Controllers;

public class ProgramsController : BaseApiController
{

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllProgramsQuery()));
    }

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetProgramQuery(id)));

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateProgramRequestDto program)
    {
        await Mediator.Send(new CreateProgramCommand(program.Name, program.TotalHours));
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteProgramCommand(id));
        return NoContent();
    }
    [Authorize(Roles = "admin")]
    [HttpGet("unmarked-attendances")]
    public async Task<IActionResult> GetUnMarkedAttendancesByPrograms()
        => Ok(await Mediator.Send(new GetUnMarkedAttendancesByProgramsQuery()));

    //[Authorize(Roles = "admin")]
    //[HttpGet("{id}/unmarked-attendances-classes")]
    //public async Task<IActionResult> GetUnmarkedAttedamceClasses([FromRoute] Guid id)
        //=> Ok(await Mediator.Send());

}
