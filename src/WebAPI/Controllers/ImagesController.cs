    using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

public class ImagesController : BaseApiController
{
    [HttpGet("{imageName}")]
    public async Task<IActionResult> GetImagesAsync(string imageName)
    {
        FileContentResponseDto fileResponse = await Mediator.Send(new GetImageQuery(imageName));
        return File(fileResponse.FileBytes, fileResponse.ContentType);
    }
}
