using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

public class NotificationController : BaseApiController
{

    [HttpGet]
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    public async Task<IActionResult> GetAll()
                => StatusCode(200, await Mediator.Send(new GetAllNotificationsQuery()));


    [HttpPut("{id}")]
    [Authorize(Roles = "admin,mentor,ta,muellim")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id)
    {
        await Mediator.Send(new UpdateNotificationCommand() { Id = id });
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
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