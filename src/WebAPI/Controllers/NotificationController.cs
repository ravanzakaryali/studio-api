using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;


[Authorize]
public class NotificationController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
                => StatusCode(200, await Mediator.Send(new GetAllNotificationsQuery()));

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id)
    {
        await Mediator.Send(new UpdateNotificationCommand() { Id = id });
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create(NotificationRequestDto request)
    {
        await Mediator.Send(
            new CreateNotificationCommand()
            {
                Title = request.Title,
                Content = request.Content,
            }
        );
        return NoContent();
    }
}