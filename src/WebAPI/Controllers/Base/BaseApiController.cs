namespace Space.WebAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class BaseApiController : Controller
{
    private IMediator? _mediator;
    private ISpaceDbContext? _spaceDbContext;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    protected ISpaceDbContext SpaceDbContext => _spaceDbContext ??= HttpContext.RequestServices.GetService<ISpaceDbContext>()!;
}
