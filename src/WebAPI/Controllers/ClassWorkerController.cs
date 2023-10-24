using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Class Module Workers
/// </summary>
[Authorize]
public class ClassModulesWorkerController : BaseApiController
{

    ///// <summary>
    ///// Gets the Worker of a class.
    ///// </summary>
    ///// <param name="classId">The ID of the class.</param>
    ///// <param name="WorkerId">The ID of the Worker.</param>
    ///// <returns>The Worker of the class.</returns>
    //[HttpGet("/classes/{classId}/Worker/{WorkerId}")]
    //public async Task<IActionResult> Get([FromRoute] Guid classId, [FromRoute] Guid WorkerId)
    //        => StatusCode(200, await Mediator.Send(new GetClassModulesWorkerQuery(classId, WorkerId)));



    ///// <summary>
    ///// Gets all class Workers.
    ///// </summary>
    ///// <returns>A status code of 200 and a list of class Workers.</returns>
    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //            => StatusCode(200, await Mediator.Send(new GetAllClassModulesWorkerQuery()));

    ///// <summary>
    ///// Adds an Worker to a class.
    ///// </summary>
    ///// <param name="request">The request containing the class and Worker IDs.</param>
    ///// <returns>A 201 status code if the Worker was added successfully.</returns>
    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesDefaultResponseType]
    //public async Task<IActionResult> Create([FromBody] CreateClassModulesWorkerRequestDto request)
    //            => StatusCode(201, await Mediator.Send(new CreateClassModulesWorkerCommand(request.ClassId, request.WorkerId)));


    ///// <summary>
    ///// Updates the class Worker with the given classId and WorkerId.
    ///// </summary>
    //[HttpPut]
    //public async Task<IActionResult> Update([FromBody] UpdateClassModulesWorkerRequestDto request)
    //                => StatusCode(200, await Mediator.Send(new UpdateClassModulesWorkerCommand(request.ClassId, request.WorkerId)));

    ///// <summary>
    ///// Deletes an Worker from a class.
    ///// </summary>
    ///// <param name="classId">The ID of the class.</param>
    ///// <param name="WorkerId">The ID of the Worker.</param>
    ///// <returns>The status code of the operation.</returns>
    //[HttpDelete("/classes/{classId}/Worker/{WorkerId}")]
    //public async Task<IActionResult> Delete([FromRoute] Guid classId, [FromRoute] Guid WorkerId)
    //          => StatusCode(200, await Mediator.Send(new DeleteClassModulesWorkerCommand(classId, WorkerId)));
}
