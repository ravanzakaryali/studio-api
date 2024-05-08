using Space.Application.DTOs.Program.Request;
using Microsoft.AspNetCore.Http;
using Space.Application.Enums;

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
    public async Task<IActionResult> GetById(int id)
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
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteProgramCommand(id));
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("unmarked-attendances")]
    public async Task<IActionResult> GetUnMarkedAttendancesByPrograms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
         => Ok(await Mediator.Send(new GetUnMarkedAttendancesByProgramsQuery()
         {
             StartDate = startDate,
             EndDate = endDate
         }));

    [Authorize(Roles = "admin")]
    [HttpGet("{id}/unmarked-attendances-classes")]
    public async Task<IActionResult> GetUnmarkedAttedamceClasses([FromRoute] int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        => Ok(await Mediator.Send(new GetUnmarkedAttedanceClassesByProgramQuery()
        {
            Id = id,
            StartDate = startDate,
            EndDate = endDate
        }));


    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet("{id}/modules")]
    public async Task<IActionResult> GetModules([FromRoute] int id)
        => Ok(await Mediator.Send(new GetModulesByProgramQuery()
        {
            Id = id
        }));

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/modules")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateModules([FromRoute] int id, [FromBody] CreateModuleWithProgramRequestDto modules)
        => StatusCode(201, await Mediator.Send(new CreateModuleWithProgramCommand(id, modules.Modules)));

}
