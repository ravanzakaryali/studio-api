namespace Space.WebAPI.Controllers;


[Authorize]
public class FinanceController : BaseApiController
{

    [HttpGet("report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetReport([FromQuery] MonthOfYear? month, [FromQuery] string? role)
    {
        var report = await Mediator.Send(new GetReportQuery()
        {
            Role = role,
            Month = month
        });

        return Ok(report);
    }
}