using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

/// <summary>
/// Holiday controller
/// </summary>
[Authorize(Roles = "admin")]
public class HolidaysController : BaseApiController
{
    /// <summary>
    /// Creates a new holiday with the provided details.
    /// </summary>
    /// <param name="request">A JSON object containing details for creating the holiday.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the holiday.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to create a new holiday by providing a JSON object with the necessary details,
    /// including the description, start date, end date, and class identifier. It returns a 201 status code upon successful creation.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromBody] CreateHolidayRequestDto request)
        => StatusCode(StatusCodes.Status201Created, await Mediator.Send(new CreateHolidayCommand(request.Description, request.StartDate, request.EndDate, request.ClassId)));

    /// <summary>
    /// Retrieves a list of all holidays.
    /// </summary>
    /// <returns>
    /// An HTTP response containing a list of all holidays upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve a list of all holidays. It returns a list of holidays
    /// as an HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllHolidayQuery()));

    /// <summary>
    /// Retrieves details of a specific holiday based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the holiday to retrieve details for.</param>
    /// <returns>
    /// An HTTP response containing details of the specified holiday upon successful retrieval.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to retrieve details of a specific holiday based on its unique identifier.
    /// It expects the unique identifier of the holiday as a route parameter and returns the holiday's details as an
    /// HTTP response upon successful retrieval.
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        => Ok(await Mediator.Send(new GetHolidayQuery(id)));

    /// <summary>
    /// Updates the details of a specific holiday based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the holiday to update.</param>
    /// <param name="request">A JSON object containing details for updating the holiday.</param>
    /// <returns>
    /// An HTTP response containing the updated details of the holiday upon successful update.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to update the details of a specific holiday based on its unique identifier.
    /// It expects the unique identifier of the holiday as a route parameter and a JSON object with details for updating
    /// the holiday in the request body. Upon successful update, it returns an HTTP response containing the updated details
    /// of the holiday.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateHolidayRequestDto request)
        => Ok(await Mediator.Send(new UpdateHolidayCommand(id, request)));

    /// <summary>
    /// Deletes a specific holiday based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the holiday to delete.</param>
    /// <returns>
    /// An HTTP response indicating the success of the holiday deletion.
    /// </returns>
    /// <remarks>
    /// This endpoint allows authorized users to delete a specific holiday based on its unique identifier.
    /// It expects the unique identifier of the holiday as a route parameter. Upon successful deletion,
    /// it returns an HTTP response indicating the success of the operation.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        => Ok(await Mediator.Send(new DeleteHolidayCommand(id)));

    [HttpGet("dates")]
    public async Task<IActionResult> GetHolidaysDates()
        => Ok(await Mediator.Send(new GetHolidayDatesQuery()));

}
