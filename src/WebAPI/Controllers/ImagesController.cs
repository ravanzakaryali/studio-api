using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Image controller
/// </summary>
public class ImagesController : BaseApiController
{

    [HttpGet("{imageName}")]
    public async Task<IActionResult> GetImagesAsync(string imageName)
    {
        FileContentResponseDto fileResponse = await Mediator.Send(new GetImageQuery(imageName));
        return File(fileResponse.FileBytes, fileResponse.ContentType);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UploadImageAsync([FromForm] FileUpload file)
    {
        await Mediator.Send(new UploadImageCommand(file.File));
        return NoContent();
    }
}
