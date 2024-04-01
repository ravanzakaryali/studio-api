using Microsoft.AspNetCore.Authorization;
using Space.Application.DTOs.Worker;
using Space.Domain.Entities;
using Space.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

[Authorize]
public class WorkersController : BaseApiController
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRequestWorkerDto request)
           => StatusCode(201, await Mediator.Send(new CreateWorkerCommand(request.Name, request.Surname, request.Email, request.GroupsId)));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerQuery(id)));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RoleEnum? role)
           => StatusCode(200, await Mediator.Send(new GetAllWorkerQuery(role)));


    [HttpGet("with-details")]
    public async Task<IActionResult> GetAllWithDetaiks()
        => StatusCode(200, await Mediator.Send(new GetAllWorkersWithDetailsQuery()));

    [HttpGet("{id}/worker-class-sessions-by-class")]
    public async Task<IActionResult> GetWorkerClassSessionsByClass([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerClassSessionsByClassQuery(id)));

    [HttpGet("{id}/get-worker-general-report")]
    public async Task<IActionResult> GetWorkerGeneralReport([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerGeneralReportQuery(id)));


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new DeleteWorkerCommand(id)));


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkerReuqest request)
          => StatusCode(200, await Mediator.Send(new UpdateWorkerCommand()
          {
              Id = id,
              Worker = new WorkerDto()
              {
                  Email = request.Email,
                  Name = request.Name,
                  Surname = request.Surname
              }
          }));


    [HttpGet("{id}/classes")]
    public async Task<IActionResult> GetClassByWorker([FromRoute] int id)
        => Ok(await Mediator.Send(new GetClassesByWorkerQuery(id)));


    [HttpGet("{id}/attendance-by-class")]
    public async Task<IActionResult> GetWorkerAttendanceByClassId([FromRoute] int id) =>
        StatusCode(200, await Mediator.Send(new GetWorkerAttendanceByClassQuery(id)));


    [HttpPost("{id}/set-permission")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddPermissionToWorker([FromRoute] int id, [FromBody] IEnumerable<SetAccessToPermissionGroupAndWorkerDto> request)
    {
        await Mediator.Send(new SetPermissionToWorkerCommand()
        {
            WorkerId = id,
            AppModulesAccess = request
        });
        return NoContent();
    }

    [HttpGet("{id}/permission-groups")]
    public async Task<IActionResult> GetPermissionGroupsByWorker([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new GetPermissionGroupsByWorkerQuery(id)));

    [HttpPut("{id}/permission-groups")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdatePermissionGroupsByWorker([FromRoute] int id, [FromBody] IEnumerable<PermissionGroupId> request)
    {
        await Mediator.Send(new UpdatePermissionGroupsByWorkerCommand()
        {
            WorkerId = id,
            PermissionGroups = request
        });
        return NoContent();
    }
}
