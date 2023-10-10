using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

/// <summary>
/// User controller
/// </summary>
public class UsersController : BaseApiController
{
    /// <summary>
    /// Retrieves login information for the current user.
    /// </summary>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of the user's login information.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve login information for the currently authenticated user.
    /// It sends a query to the mediator to fetch the user's login information. Upon successful retrieval, it returns
    /// an HTTP response with a status code 200 (OK) to indicate the success of the operation and includes the user's
    /// login information in the response body.
    /// </remarks>
    [HttpGet("login")]
    public async Task<IActionResult> GetUserLogin()
        => Ok(await Mediator.Send(new UserLoginQuery()));

    /// <summary>
    /// Creates user roles for a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to assign roles to.</param>
    /// <param name="request">A collection of role request data to create user roles.</param>
    /// <returns>
    /// An HTTP response with a status code 204 (No Content) upon successful creation of user roles.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users with the "admin" role to create user roles for a specific user by providing
    /// their unique identifier (ID) and a collection of role request data. It sends a command to the mediator to create
    /// user roles based on the provided data. Upon successful creation, it returns an HTTP response with a status code
    /// 204 (No Content) to indicate the success of the operation.
    /// </remarks>
    [Authorize(Roles = "admin")]
    [HttpPost("{id}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateUserRole([FromRoute] Guid id, [FromBody] IEnumerable<CreateRoleRequestDto> request)
    {
        await Mediator.Send(new CreateRoleByUserCommand(id, request));
        return NoContent();
    }

    /// <summary>
    /// Retrieves the roles assigned to a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve roles for.</param>
    /// <returns>
    /// An HTTP response with a status code 200 (OK) upon successful retrieval of user roles.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve the roles assigned to a specific user by providing their unique
    /// identifier (ID). It sends a query to the mediator to fetch the roles assigned to the user based on the provided ID.
    /// Upon successful retrieval, it returns an HTTP response with a status code 200 (OK) to indicate the success of the
    /// operation and includes the user's roles in the response body.
    /// </remarks>
    [Authorize]
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetRolesByUserQuery(id)));
}
