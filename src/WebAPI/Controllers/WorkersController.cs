using Microsoft.AspNetCore.Authorization;
using Space.Application.DTOs.Worker;
using Space.Domain.Entities;
using Space.Domain.Enums;

namespace Space.WebAPI.Controllers;

public class WorkersController : BaseApiController
{

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRequestWorkerDto request)
           => StatusCode(201, await Mediator.Send(new CreateWorkerCommand(request.Name, request.Surname, request.Email)));

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerQuery(id)));

    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RoleEnum? role)
           => StatusCode(200, await Mediator.Send(new GetAllWorkerQuery(role)));


    [Authorize(Roles = "admin")]
    [HttpGet("with-details")]
    public async Task<IActionResult> GetAllWithDetaiks()
        => StatusCode(200, await Mediator.Send(new GetAllWorkersWithDetailsQuery()));

    [Authorize(Roles = "mentor,ta,muellim,admin")]
    [HttpGet("{id}/worker-class-sessions-by-class")]
    public async Task<IActionResult> GetWorkerClassSessionsByClass([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerClassSessionsByClassQuery(id)));

    [Authorize(Roles = "mentor,ta,muellim,admin")]
    [HttpGet("{id}/get-worker-general-report")]
    public async Task<IActionResult> GetWorkerGeneralReport([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerGeneralReportQuery(id)));


    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new DeleteWorkerCommand(id)));


    [Authorize(Roles = "admin")]
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


    [Authorize(Roles = "admin,muellim,mentor,ta")]
    [HttpGet("{id}/classes")]
    public async Task<IActionResult> GetClassByWorker([FromRoute] int id)
        => Ok(await Mediator.Send(new GetClassesByWorkerQuery(id)));


    [Authorize(Roles = "admin")]
    [HttpGet("{id}/attendance-by-class")]
    public async Task<IActionResult> GetWorkerAttendanceByClassId([FromRoute] int id) =>
        StatusCode(200, await Mediator.Send(new GetWorkerAttendanceByClassQuery(id)));
}
