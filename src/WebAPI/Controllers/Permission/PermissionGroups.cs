namespace Space.WebAPI.Controllers;

[Authorize]
public class PermissionGroupsController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetPermissionGroups()
    {
        return Ok(await Mediator.Send(new GetPermissionGroupsQuery()));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermissionGroup(CreatePermissionGroupDto request)
    {
        await Mediator.Send(new CreatePermissionGroupCommand
        {
            Name = request.Name,
            Description = request.Description,
            WorkersId = request.WorkersId
        });
        return Ok();
    }

    [HttpPost("{id}/add-worker")]
    public async Task<IActionResult> AddWorkerToPermissionGroup(int id, AddWorkerToPermissionGroupDto request)
    {
        await Mediator.Send(new AddWorkerToPermissionGroupCommand
        {
            PermissionGroupId = id,
            WorkerId = request.WorkerId
        });
        return Ok();
    }

    [HttpPost("{id}/set-access")]
    public async Task<IActionResult> SetAccessToPermissionGroup(int id, IEnumerable<SetAccessToPermissionGroupDto> request)
    {
        await Mediator.Send(new SetAccessToPermissionGroupCommand
        {
            PermissionGroupId = id,
            PermissionLevels = request
        });
        return Ok();
    }


}