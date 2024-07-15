using Microsoft.AspNetCore.Http;

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

    [HttpGet("{id}/app-modules-access")]
    public async Task<IActionResult> GetPermissionGroupAppModulesAccess([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetPermissionGroupAppModulesAccessQuery { GroupId = id }));
    }

    [HttpPut("{id}/app-modules-access")]
    public async Task<IActionResult> UpdatePermissionGroupAppModulesAccess(int id, IEnumerable<UpdatePermissionAppModuleDto> request)
    {
        await Mediator.Send(new UpdatePermissionGroupAppModulesAccessCommand
        {
            GroupId = id,
            PermissionAccesses = request
        });
        return Ok();
    }

    [HttpGet("{id}/with-users")]
    public async Task<IActionResult> GetPermissionGroupWithUsers(int id)
    {
        return Ok(await Mediator.Send(new GetPermissionGroupWithUsersQuery { Id = id }));
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdatePermissionGroup(int id, UpdatePermissionGroupDto request)
    {
        await Mediator.Send(new UpdatePermissionGroupCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description
        });
        return NoContent();
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