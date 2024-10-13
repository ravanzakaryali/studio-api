using Microsoft.EntityFrameworkCore;
using Space.Application.DTOs.Worker;
using Space.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace Space.WebAPI.Controllers;

public class WorkersController : BaseApiController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] RoleEnum? role)
           => StatusCode(200, await Mediator.Send(new GetAllWorkerQuery(role)));


    [HttpGet("with-details")]
    [Authorize]
    public async Task<IActionResult> GetAllWithDetaiks()
        => StatusCode(200, await Mediator.Send(new GetAllWorkersWithDetailsQuery()));

    [HttpGet("FilteredDatas")]
    public async Task<IActionResult> GetAllFilteredDatas()
     => StatusCode(200, await Mediator.Send(new GetAllFilteredQuery()));


    [HttpGet("GetAllExamModule")]
    public async Task<IActionResult> GetAllExamData()
    => StatusCode(200, await Mediator.Send(new GetAllExamQuery()));

    [HttpPost("CreateExamSheet")]
    public async Task<IActionResult> CreatExamSheet(IEnumerable<CreateExamSheetDto> dtos)
        => StatusCode(201, await Mediator.Send(new CreateExamSheetCommand(dtos)));

    [HttpGet("GetAllSurveyModule")]
    public async Task<IActionResult> GetAllSurveyData()
    => StatusCode(200, await Mediator.Send(new GetAllSurveyQuery()));

    [HttpPost("CreateSurveySheet")]
    public async Task<IActionResult> CreatSurveySheet(IEnumerable<CreateSurveySheetDto> dtos)
        => StatusCode(201, await Mediator.Send(new CreateSurveySheetCommand(dtos)));


    [HttpGet("{id}/worker-class-sessions-by-class")]
    [Authorize]

    public async Task<IActionResult> GetWorkerClassSessionsByClass([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerClassSessionsByClassQuery(id)));

    [HttpGet("{id}/get-worker-general-report")]
    [Authorize]

    public async Task<IActionResult> GetWorkerGeneralReport([FromRoute] int id)
            => StatusCode(200, await Mediator.Send(new GetWorkerGeneralReportQuery(id)));


    [HttpGet("login/classes")]
    [Authorize]

    public async Task<IActionResult> GetClassByWorker()
        => Ok(await Mediator.Send(new GetClassesByWorkerQuery()));


    [HttpGet("{id}/attendance-by-class")]
    [Authorize]

    public async Task<IActionResult> GetWorkerAttendanceByClassId([FromRoute] int id) =>
        StatusCode(200, await Mediator.Send(new GetWorkerAttendanceByClassQuery(id)));


    [HttpPost("{id}/set-permission")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [Authorize]

    public async Task<IActionResult> AddPermissionToWorker([FromRoute] int id, [FromBody] IEnumerable<SetAccessToPermissionGroupAndWorkerDto> request)
    {
        await Mediator.Send(new SetPermissionToWorkerCommand()
        {
            WorkerId = id,
            AppModulesAccess = request
        });
        return NoContent();
    }


    [HttpPut("{id}")]
    [Authorize]

    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkerReuqest request)
              => StatusCode(200, await Mediator.Send(new UpdateWorkerCommand()
              {
                  Id = id,
                  Worker = new WorkerDto()
                  {
                      Email = request.Email,
                      Name = request.Name,
                      Surname = request.Surname,
                      Fincode = request.Fincode
                  }
              }));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    [Authorize]

    public async Task<IActionResult> Create([FromBody] CreateRequestWorkerDto request)
              => StatusCode(201, await Mediator.Send(new CreateWorkerCommand(request.Name, request.Surname, request.Email, request.Fincode, request.GroupsId)));

    [HttpGet("{id}/permission-groups")]
    [Authorize]

    public async Task<IActionResult> GetPermissionGroupsByWorker([FromRoute] int id)
        => StatusCode(200, await Mediator.Send(new GetPermissionGroupsByWorkerQuery(id)));

    [HttpPut("{id}/permission-groups")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [Authorize]

    public async Task<IActionResult> UpdatePermissionGroupsByWorker([FromRoute] int id, [FromBody] IEnumerable<PermissionGroupId> request)
    {
        await Mediator.Send(new UpdatePermissionGroupsByWorkerCommand()
        {
            WorkerId = id,
            PermissionGroups = request
        });
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize]

    public async Task<IActionResult> Get([FromRoute] int id)
         => StatusCode(200, await Mediator.Send(new GetWorkerQuery(id)));

    [HttpGet("{id}/app-modules-access")]
    [Authorize]

    public async Task<IActionResult> GetWorkerAppModulesAccess([FromRoute] int id)
    {
        return Ok(await Mediator.Send(new GetWorkerAppModulesAccessQuery(id)));
    }
    [HttpPut("{id}/app-modules-access")]
    [Authorize]

    public async Task<IActionResult> SetAccessToWorkerAppModules([FromRoute] int id, [FromBody] IEnumerable<UpdatePermissionAppModuleDto> request)
    {
        await Mediator.Send(new UpdateWorkerAppModulesAccessCommand()
        {
            WorkerId = id,
            PermissionAccesses = request
        });
        return Ok();
    }
    [HttpGet("excel/export")]
    [Authorize]

    public async Task<IActionResult> ExportWorkersToExcel([FromQuery] WorkerType? workerType)
    {
        return Ok(await Mediator.Send(new ExportWorkersToExcelCommand()
        {
            WorkerType = workerType
        }));
    }
}
