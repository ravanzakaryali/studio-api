namespace Space.WebAPI.Controllers;

/// <summary>
/// Authentication controller
/// </summary>
public class AuthController : BaseApiController
{
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutCommand());
        return NoContent();
    }


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

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm(ConfimCodeRequest request)
       => Ok(await Mediator.Send(new ConfirmCodeCommand()
       {
           Code = request.Code,
           Email = request.Email,
       }));

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
