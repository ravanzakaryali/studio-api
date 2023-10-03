using Microsoft.AspNetCore.Authorization;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Image controller
/// </summary>
public class ImagesController : BaseApiController
{
    /// <summary>
    /// Retrieves an image by its file name.
    /// </summary>
    /// <param name="imageName">The file name of the image to retrieve.</param>
    /// <returns>
    /// An HTTP response containing the image file based on its file name.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve an image file by specifying its file name as a route parameter.
    /// Upon successful retrieval, it returns an HTTP response containing the image file.
    /// </remarks>
    [HttpGet("{imageName}")]
    public async Task<IActionResult> GetImagesAsync(string imageName)
    {
        FileContentResponseDto fileResponse = await Mediator.Send(new GetImageQuery(imageName));
        return File(fileResponse.FileBytes, fileResponse.ContentType);
    }
}
