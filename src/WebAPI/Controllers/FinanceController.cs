namespace Space.WebAPI.Controllers;


[Authorize]
public class FinanceController : BaseApiController
{

    [HttpGet("report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var report = await Mediator.Send(new GetReportQuery()
        {
            StartDate = startDate,
            EndDate = endDate
        });

        return Ok(report);
    }
}