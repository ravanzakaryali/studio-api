using Microsoft.AspNetCore.Authorization;
using Space.Application.DTOs.Worker;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Worker controller
/// </summary>
public class WorkersController : BaseApiController
{
    /// <summary>
    /// Creates a new worker with the specified details.
    /// </summary>
    /// <param name="request">The request data containing worker details (name, surname, email).</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of a new worker.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the roles "admin," "mentor," "ta," or "muellim" to create a new worker
    /// by providing the required worker details, including name, surname, and email. It sends a command to the mediator to
    /// create a new worker with the provided details. Upon successful creation, it returns an HTTP response with a status
    /// code 201 (Created) to indicate the success of the operation and includes the newly created worker's information
    /// in the response.
    /// </remarks>
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRequestWorkerDto request)
           => StatusCode(201, await Mediator.Send(new CreateWorkerCommand(request.Name, request.Surname, request.Email)));

    /// <summary>
    /// Retrieves details of a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to retrieve.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the worker's details.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve details of a worker by specifying the worker's unique identifier
    /// (ID). It sends a query to the mediator to fetch the worker's details based on the provided ID. Upon successful
    /// retrieval, it returns an HTTP response with a status code 200 (OK) to indicate the success of the operation and
    /// includes the worker's details in the response body.
    /// </remarks>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
            => StatusCode(200, await Mediator.Send(new GetWorkerQuery(id)));

    /// <summary>
    /// Retrieves a list of all workers.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the list of workers.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve a list of all workers. It sends a query to
    /// the mediator to fetch the list of workers. Upon successful retrieval, it returns an HTTP response with a status
    /// code 200 (OK) to indicate the success of the operation and includes the list of workers in the response body.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
           => StatusCode(200, await Mediator.Send(new GetAllWorkerQuery()));

    /// <summary>
    /// Retrieves a list of all workers with additional details.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the list of workers with details.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve a list of all workers along with their
    /// additional details. It sends a query to the mediator to fetch the list of workers with details. Upon successful
    /// retrieval, it returns an HTTP response with a status code 200 (OK) to indicate the success of the operation and
    /// includes the list of workers with details in the response body.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet("with-details")]
    public async Task<IActionResult> GetAllWithDetaiks()
        => StatusCode(200, await Mediator.Send(new GetAllWorkersWithDetailsQuery()));

    /// <summary>
    /// Retrieves class sessions associated with a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to retrieve class sessions for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of class sessions associated with the worker.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the roles "mentor," "ta," "muellim," or "admin" to retrieve class
    /// sessions associated with a specific worker by providing their unique identifier (ID). It sends a query to the mediator
    /// to fetch the class sessions for the worker based on the provided ID. Upon successful retrieval, it returns an HTTP
    /// response with a status code 200 (OK) to indicate the success of the operation and includes the list of class sessions
    /// associated with the worker in the response body.
    /// </remarks>
    [Authorize(Roles = "mentor,ta,muellim,admin")]
    [HttpGet("{id}/worker-class-sessions-by-class")]
    public async Task<IActionResult> GetWorkerClassSessionsByClass([FromRoute] Guid id)
            => StatusCode(200, await Mediator.Send(new GetWorkerClassSessionsByClassQuery(id)));

    /// <summary>
    /// Retrieves a general report for a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to retrieve the general report for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the general report for the worker.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the roles "mentor," "ta," "muellim," or "admin" to retrieve a general
    /// report for a specific worker by providing their unique identifier (ID). It sends a query to the mediator to fetch the
    /// general report for the worker based on the provided ID. Upon successful retrieval, it returns an HTTP response with
    /// a status code 200 (OK) to indicate the success of the operation and includes the general report for the worker in
    /// the response body.
    /// </remarks>
    [Authorize(Roles = "mentor,ta,muellim,admin")]
    [HttpGet("{id}/get-worker-general-report")]
    public async Task<IActionResult> GetWorkerGeneralReport([FromRoute] Guid id)
            => StatusCode(200, await Mediator.Send(new GetWorkerGeneralReportQuery(id)));

    /// <summary>
    /// Deletes a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to be deleted.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful deletion of the worker.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to delete a worker by specifying their unique
    /// identifier (ID). It sends a command to the mediator to delete the worker based on the provided ID. Upon successful
    /// deletion, it returns an HTTP response with a status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => StatusCode(200, await Mediator.Send(new DeleteWorkerCommand(id)));

    /// <summary>
    /// Updates the information of a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to be updated.</param>
    /// <param name="request">The updated information for the worker.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful update of the worker's information.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to update the information of a worker by specifying
    /// their unique identifier (ID) and providing the updated information in the request body. It sends a command to the
    /// mediator to update the worker's information based on the provided ID and updated details. Upon successful update,
    /// it returns an HTTP response with a status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWorkerReuqest request)
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

    /// <summary>
    /// Retrieves a list of classes associated with a worker by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to retrieve classes for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) containing a list of classes associated with the worker.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the roles "admin," "muellim," "mentor," or "ta" to retrieve a list of
    /// classes associated with a specific worker by providing their unique identifier (ID). It sends a query to the mediator
    /// to fetch the classes associated with the worker based on the provided ID. Upon successful retrieval, it returns an HTTP
    /// response with a status code 200 (OK) containing the list of classes associated with the worker in the response body.
    /// </remarks>
    [Authorize(Roles = "admin,muellim,mentor,ta")]
    [HttpGet("{id}/classes")]
    public async Task<IActionResult> GetClassByWorker([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetClassesByWorkerQuery(id)));

    /// <summary>
    /// Retrieves worker attendance records for a specific class by worker's unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the worker to retrieve attendance records for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) containing the worker's attendance records for the specified class.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve attendance records for a specific worker
    /// in a particular class by providing the worker's unique identifier (ID). It sends a query to the mediator to fetch
    /// the worker's attendance records for the specified class based on the provided ID. Upon successful retrieval, it
    /// returns an HTTP response with a status code 200 (OK) containing the worker's attendance records for the specified class
    /// in the response body.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet("{id}/attendance-by-class")]
    public async Task<IActionResult> GetWorkerAttendanceByClassId([FromRoute] Guid id) => StatusCode(200, await Mediator.Send(new GetWorkerAttendanceByClassQuery(id)));
}
