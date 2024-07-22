namespace Space.Application.Handlers;

public class GetReportQuery : IRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

internal class GetReportQueryHandler : IRequestHandler<GetReportQuery>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetReportQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(GetReportQuery request, CancellationToken cancellationToken)
    {

    }
}