using System;
using Microsoft.AspNetCore.Authorization;
using Space.Application.DTOs.Worker;

namespace Space.WebAPI.Controllers;


[Authorize(Roles = "admin")]
public class ReservationController : BaseApiController
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequestDto request)
   => StatusCode(201, await Mediator.Send(new CreateReservationCommand(request.Title, request.Description, request.StartDate, request.EndDate, request.RoomId, request.WorkersId)));

}

