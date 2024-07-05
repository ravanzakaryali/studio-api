using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

[Authorize]
public class SupportsController : BaseApiController
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSupportRequestDto request)
    {
        await Mediator.Send(new CreateSupportCommand(request.Title, request.Description, request.ClassId, request.CategoryId, request.PhoneNumber, request.Images));
        return StatusCode(StatusCodes.Status201Created);
    }

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



    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllSupportQuery()));


    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategoryAsync()
        => Ok(await Mediator.Send(new GetSupportCategoriesQuery()));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await Mediator.Send(new DeleteSupportCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
        => Ok(await Mediator.Send(new GetSupportQuery(id)));
}
