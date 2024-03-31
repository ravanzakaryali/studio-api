namespace Space.WebAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class BaseApiController : Controller
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}
