namespace Space.WebAPI.Controllers;

public class PermissionGroupsController : BaseApiController
{
    [Authorize]
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

}