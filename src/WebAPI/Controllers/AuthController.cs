namespace Space.WebAPI.Controllers;

/// <summary>
/// Authentication controller
/// </summary>
public class AuthController : BaseApiController
{
    /// <summary>
    /// Logs the user out of the system.
    /// </summary>
    /// <remarks>
    /// This endpoint logs the authenticated user out of the system by sending a logout command.
    /// </remarks>
    /// <returns>No content on successful logout.</returns>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutCommand());
        return NoContent();
    }


    /// <summary>
    /// Handles user login requests.
    /// </summary>
    /// <param name="request">A JSON object containing email and password.</param>
    /// <returns>An HTTP 200 (OK) response upon successful login.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        await Mediator.Send(new LoginCommand()
        {
            Email = request.Email,
            Password = request.Password,
            //ReCaptchaToken = request.Token,
        });
        return Ok();
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">A JSON object containing user registration details.</param>
    /// <returns>An HTTP response indicating the success of the registration.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        return Ok(await Mediator.Send(new RegisterCommand()
        {
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname
        }));
    }

    /// <summary>
    /// Confirms a user's registration by verifying a confirmation code.
    /// </summary>
    /// <param name="request">A JSON object containing confirmation code and email.</param>
    /// <returns>An HTTP response indicating the success of the confirmation process.</returns>
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm(ConfimCodeRequest request)
       => Ok(await Mediator.Send(new ConfirmCodeCommand()
       {
           Code = request.Code,
           Email = request.Email,
       }));

    /// <summary>
    /// Initiates a password refresh process for a user.
    /// </summary>
    /// <param name="request">A JSON object containing the user's email.</param>
    /// <returns>
    /// An HTTP 204 (No Content) response indicating the initiation of the password refresh process.
    /// </returns>
    [HttpPost("refresh-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RefreshPassword([FromBody] RefreshPasswordRequestDto request)
    {
        await Mediator.Send(new RefreshPasswordCommand()
        {
            Email = request.Email,
        });
        return NoContent();
    }

    /// <summary>
    /// Updates a user's password using a provided key.
    /// </summary>
    /// <param name="request">A JSON object containing the key and new password.</param>
    /// <returns>
    /// An HTTP 204 (No Content) response indicating the successful password update.
    /// </returns>
    [HttpPost("update-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto request)
    {
        await Mediator.Send(new UpdatePasswordCommand()
        {
            Key = request.Key,
            Password = request.Password
        });
        return NoContent();
    }

    //[Authorize]
    //[HttpPost("refresh-token")]
    //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    //    => Ok(await Mediator.Send(new RefreshTokenCommand()
    //    {
    //        RefreshToken = request.RefreshToken,
    //    }));


}
