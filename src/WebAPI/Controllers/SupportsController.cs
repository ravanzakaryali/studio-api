using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

public class SupportsController : BaseApiController
{
    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSupportRequestDto request)
    {
        await Mediator.Send(new CreateSupportCommand(request.Title, request.Description, request.Images));
        return StatusCode(StatusCodes.Status201Created);
    }
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllSupportQuery()));

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteSupportCommand(id));
        return NoContent();
    }
    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetSupportQuery(id)));
}
