namespace Space.WebAPI.Controllers;

[Authorize]
public class PermissionGroupsController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetPermissionGroups()
    {
        return Ok(await Mediator.Send(new GetPermissionGroupsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPermissionGroup(int id)
    {
        return Ok(await Mediator.Send(new GetPermissionGroupQuery { Id = id }));
    }

    [HttpGet("{id}/with-users")]
    public async Task<IActionResult> GetPermissionGroupWithUsers(int id)
    {
        return Ok(await Mediator.Send(new GetPermissionGroupWithUsersQuery { Id = id }));
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
            WorkerIds = request.WorkerIds,
        });
        return Ok();
    }
    [HttpPost("{id}/remove-worker")]
    public async Task<IActionResult> RemoveWorkerFromPermissionGroup(int id, RemoveWorkerFromPermissionGroupDto request)
    {
        await Mediator.Send(new RemoveWorkerFromPermissionGroupCommand
        {
            PermissionGroupId = id,
            WorkerId = request.WorkerId
        });
        return Ok();
    }

    [HttpPost("{id}/set-access")]
    public async Task<IActionResult> SetAccessToPermissionGroup(int id, IEnumerable<SetAccessToPermissionGroupAndWorkerDto> request)
    {
        await Mediator.Send(new SetAccessToPermissionGroupCommand
        {
            PermissionGroupId = id,
            AppModulesAccess = request
        });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermissionGroup(int id)
    {
        await Mediator.Send(new DeletePermissionGroupCommand { Id = id });
        return Ok();
    }
}