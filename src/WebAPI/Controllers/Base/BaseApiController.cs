namespace Space.WebAPI.Controllers;

/// <summary>
/// Base Api Controller
/// </summary>
[ApiController, Route("api/[controller]")]
public class BaseApiController : Controller
{
    private IMediator? _mediator;
    /// <summary>
    /// Mediator get service
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}
