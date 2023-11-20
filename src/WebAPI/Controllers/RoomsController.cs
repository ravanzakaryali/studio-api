namespace Space.WebAPI.Controllers;

/// <summary>
/// Room controller
/// </summary>
[Authorize(Roles = "admin")]
public class RoomsController : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all rooms.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of all rooms.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve a list of all rooms. It returns a list of rooms
    /// as an HTTP response with a status code 200 (OK) upon successful retrieval.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => StatusCode(200, await Mediator.Send(new GetAllRoomQuery()));

    /// <summary>
    /// Creates a new room with the provided details.
    /// </summary>
    /// <param name="request">A JSON object containing details for creating the room.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the room.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to create a new room by providing a JSON object with the necessary details,
    /// including the room name, capacity, and type. It returns a 201 status code upon successful creation of the room.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto request)
        => StatusCode(201, await Mediator.Send(new CreateRoomCommand(request.Name, request.Capacity, request.Type)));

    /// <summary>
    /// Updates an existing room with the provided details.
    /// </summary>
    /// <param name="id">The unique identifier of the room to update.</param>
    /// <param name="request">A JSON object containing updated details for the room.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful update of the room.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to update an existing room based on its unique identifier by providing a JSON object
    /// with the updated details, including the room name, capacity, and type. It expects the unique identifier of the
    /// room as a route parameter. Upon successful update, it returns an HTTP response with a status code 200 (OK) to
    /// indicate the success of the operation.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequestDto request)
       => StatusCode(200, await Mediator.Send(new UpdateRoomCommand(id, request)));

    /// <summary>
    /// Deletes a room based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the room to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful deletion of the room.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to delete a room based on its unique identifier. It expects the unique identifier of the
    /// room as a route parameter. Upon successful deletion, it returns an HTTP response with a status code 200 (OK) to
    /// indicate the success of the operation.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
        => StatusCode(200, await Mediator.Send(new DeleteRoomCommand(id)));
}
