namespace Space.WebAPI.Controllers;

/// <summary>
/// Session controller
/// </summary>
public class SessionsController : BaseApiController
{
    /// <summary>
    /// Creates a new session with the provided details.
    /// </summary>
    /// <param name="request">A JSON object containing details for creating the session.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the session.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to create a new session by providing a JSON object
    /// with the necessary details, including the session name. It returns a 201 status code upon successful creation
    /// of the session.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateSessionRequestDto request)
                => StatusCode(201, await Mediator.Send(new CreateSessionCommand(request.Name)));

    /// <summary>
    /// Creates details for a session with the provided session ID.
    /// </summary>
    /// <param name="id">The unique identifier of the session for which to create details.</param>
    /// <param name="request">A JSON object containing details to create for the session.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of session details.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to create details for a session identified by its
    /// unique identifier (ID). Users can provide a JSON object containing details to create for the session, and the
    /// action will create these details. Upon successful creation, it returns an HTTP response with a status code 201
    /// (Created) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpPost("{id}/details")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateDetails([FromRoute] Guid id, [FromBody] CreateSessionDetailRequestDto request)
        => StatusCode(201, await Mediator.Send(new CreateSessionDetailCommand(id, request)));

    /// <summary>
    /// Retrieves a list of all sessions.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of all sessions.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve a list of all sessions. It sends a query
    /// to the mediator to fetch all session records. Upon successful retrieval, it returns an HTTP response with a
    /// status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
        => StatusCode(200, await Mediator.Send(new GetAllSessionQuery()));

    /// <summary>
    /// Retrieves session details by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the session to retrieve.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of session details.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with roles "admin," "ta," "mentor," or "muellim" to retrieve session details
    /// by specifying the unique identifier (ID) of the session. It sends a query to the mediator to fetch session details
    /// based on the provided ID. Upon successful retrieval, it returns an HTTP response with a status code 200 (OK) to
    /// indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
        => StatusCode(200, await Mediator.Send(new GetSessionQuery(id)));

    /// <summary>
    /// Deletes a session by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the session to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful deletion of the session.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to delete a session by specifying the unique
    /// identifier (ID) of the session to delete. It sends a command to the mediator to delete the session based on
    /// the provided ID. Upon successful deletion, it returns an HTTP response with a status code 200 (OK) to indicate
    /// the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
           => StatusCode(200, await Mediator.Send(new DeleteSessionCommand(id)));

    /// <summary>
    /// Deletes a session detail by its unique identifier within a specified session.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the session containing the detail.</param>
    /// <param name="sessionDetailId">The unique identifier of the session detail to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful deletion of the session detail.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to delete a session detail by specifying the unique
    /// identifiers (ID) of both the session and the session detail to delete. It sends a command to the mediator to
    /// delete the session detail within the specified session. Upon successful deletion, it returns an HTTP response
    /// with a status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpDelete("{sessionId}/details/{sessionDetailId}")]
    public async Task<IActionResult> DeleteSessionDetail([FromRoute] Guid sessionId, [FromRoute] Guid sessionDetailId)
        => StatusCode(200, await Mediator.Send(new DeleteSessionDetailCommand(sessionId, sessionDetailId)));
}
