namespace Space.WebAPI.Controllers;


[Authorize]
public class FinanceController : BaseApiController
{

    // [HttpGet("report")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesDefaultResponseType]
    // public async Task<IActionResult> GetReport([FromQuery] GetReportRequestDto request)
    // {
    //     var report = await Mediator.Send(new GetReportQuery()
    //     {
    //         StartDate = request.StartDate,
    //         EndDate = request.EndDate
    //     });

    //     return Ok(report);
    // }
}