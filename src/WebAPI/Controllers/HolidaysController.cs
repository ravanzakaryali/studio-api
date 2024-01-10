using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Space.WebAPI.Controllers;

[Authorize(Roles = "admin")]
public class HolidaysController : BaseApiController
{
   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateAsync([FromBody] CreateHolidayRequestDto request)
        => StatusCode(StatusCodes.Status201Created, await Mediator.Send(new CreateHolidayCommand(request.Description, request.StartDate, request.EndDate)));

   
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await Mediator.Send(new GetAllHolidayQuery()));

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
        => Ok(await Mediator.Send(new GetHolidayQuery(id)));

   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateHolidayRequestDto request)
        => Ok(await Mediator.Send(new UpdateHolidayCommand(id, request)));

   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        => Ok(await Mediator.Send(new DeleteHolidayCommand(id)));

    [HttpGet("dates")]
    public async Task<IActionResult> GetHolidaysDates()
        => Ok(await Mediator.Send(new GetHolidayDatesQuery()));

}
