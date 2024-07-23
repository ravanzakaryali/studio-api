
namespace Space.Application.Handlers;

public class GetReportQuery : IRequest<IEnumerable<GetFincanceReportGetDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

internal class GetReportQueryHandler : IRequestHandler<GetReportQuery, IEnumerable<GetFincanceReportGetDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetReportQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public Task<IEnumerable<GetFincanceReportGetDto>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}