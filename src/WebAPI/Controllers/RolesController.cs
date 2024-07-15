namespace Space.WebAPI.Controllers;

/// <summary>
/// Role controller
/// </summary>
[Authorize ]
public class RolesController : BaseApiController
{
    /// <summary>
    /// Retrieves a list of all user roles.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a list of all user roles upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve a list of all user roles. It returns a list of user roles
    /// as an HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllRolesQuery()));
}
