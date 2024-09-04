using Microsoft.EntityFrameworkCore;
using Space.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        return Ok(await Mediator.Send(new GetAllProjectsQuery()));
    }
    [HttpGet("{id}/programs")]  
    public async Task<IActionResult> GetProgramsByProjectId([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetProgramsByProjectIdQuery { ProjectId = id }));
    }
}