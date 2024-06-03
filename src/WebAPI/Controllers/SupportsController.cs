using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

public class SupportsController : BaseApiController
{

    [Authorize(Roles = "admin,ta,mentor,muellim")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSupportRequestDto request)
    {
        await Mediator.Send(new CreateSupportCommand(request.Title, request.Description, request.ClassId, request.CategoryId, request.PhoneNumber, request.Images));
        return StatusCode(StatusCodes.Status201Created);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateSupportRequestDto request)
    {
        await Mediator.Send(new UpdateSupportCommand()
        {
            Id = id,
            Note = request.Note,
            Status = request.Status
        });
        return NoContent();
    }



    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllSupportQuery()));


    [Authorize(Roles = "admin,mentor,muellim")]
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategoryAsync()
        => Ok(await Mediator.Send(new GetSupportCategoriesQuery()));

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await Mediator.Send(new DeleteSupportCommand(id));
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
        => Ok(await Mediator.Send(new GetSupportQuery(id)));
}
