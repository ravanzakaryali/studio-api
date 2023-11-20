namespace Space.WebAPI.Controllers;

/// <summary>
/// Support controller
/// </summary>
public class SupportsController : BaseApiController
{

    /// <summary>
    /// Creates a new support request with the provided information.
    /// </summary>
    /// <param name="request">A form containing the title, description, and optional images for the support request.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the support request.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with roles "admin," "ta," "mentor," or "muellim" to create a new support
    /// request. Users can submit a form with the title, description, and optional images for the support request.
    /// The action sends a command to the mediator to create the support request. Upon successful creation, it returns
    /// an HTTP response with a status code 201 (Created) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSupportRequestDto request)
    {
        await Mediator.Send(new CreateSupportCommand(request.Title, request.Description, request.Images));
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Retrieves a list of all support requests.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of all support requests.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve a list of all support requests.
    /// It sends a query to the mediator to fetch all support request records. Upon successful retrieval, it returns
    /// an HTTP response with a status code 200 (OK) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllSupportQuery()));

    /// <summary>
    /// Deletes a support request by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the support request to delete.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful deletion of the support request.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to delete a support request by specifying the unique
    /// identifier (ID) of the support request to delete. It sends a command to the mediator to delete the support request
    /// based on the provided ID. Upon successful deletion, it returns an HTTP response with a status code 204 (No Content)
    /// to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteSupportCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Retrieves details of a support request by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the support request to retrieve.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the support request details.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to retrieve details of a support request by specifying
    /// the unique identifier (ID) of the support request. It sends a query to the mediator to fetch the details of the
    /// support request based on the provided ID. Upon successful retrieval, it returns an HTTP response with a status code
    /// 200 (OK) to indicate the success of the operation and includes the support request details in the response body.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetSupportQuery(id)));
}
