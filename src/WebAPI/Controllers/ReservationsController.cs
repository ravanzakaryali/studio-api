namespace Space.WebAPI.Controllers;

/// <summary>
/// Reservation controller
/// </summary>
[Authorize(Roles = "admin")]
public class ReservationsController : BaseApiController
{

    /// <summary>
    /// Creates a new reservation with the provided details.
    /// </summary>
    /// <param name="request">A JSON object containing details for creating the reservation.</param>
    /// <returns>
    /// An HTTP response with a status code 201 (Created) upon successful creation of the reservation.
    /// </returns>
    /// <remarks>
    /// This endpoint allows users to create a new reservation by providing a JSON object with the necessary details,
    /// including the reservation title, worker identifiers, description, start date, end date, and room identifier.
    /// It returns a 201 status code upon successful creation of the reservation.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequestDto request)
   => StatusCode(201, await Mediator.Send(new CreateReservationCommand()
   {
       Title = request.Title,
       WorkersId = request.WorkersId,
       Description = request.Description,
       EndDate = request.EndDate,
       RoomId = request.RoomId,
       StartDate = request.StartDate,
   }));

}

