namespace Space.Application.Handlers;
public record GetAllClassSessionsByClassQuery(Guid Id) : IRequest<IEnumerable<GetAllClassSessionByClassResponseDto>>;

public class GetAllClassSessionsByClassQueryHandler : IRequestHandler<GetAllClassSessionsByClassQuery, IEnumerable<GetAllClassSessionByClassResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllClassSessionsByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllClassSessionByClassResponseDto>> Handle(GetAllClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassSessions)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException();

        //Todo: review
        IEnumerable<GetAllClassSessionByClassResponseDto> response = @class.ClassSessions
            .OrderByDescending(q => q.Date).DistinctBy(q => q.Date)
            .Select(q => new GetAllClassSessionByClassResponseDto()
            {
                ClassName = q.Class.Name,
                ClassSessionDate = q.Date,
                ClassId = q.ClassId,
                ClassSessionStatus = q.Status
            });

        return response;

    }
}
