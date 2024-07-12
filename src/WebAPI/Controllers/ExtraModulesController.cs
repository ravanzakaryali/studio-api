namespace Space.WebAPI.Controllers;

public class ExtraModulesController : BaseApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateExtraModuleRequestDto extraModule)
            => StatusCode(201, await Mediator.Send(new CreateExtraModuleCommand()
            {
                Name = extraModule.Name,
                Hours = extraModule.Hours,
                Version = extraModule.Version,
                ProgramId = extraModule.ProgramId
            }));
}