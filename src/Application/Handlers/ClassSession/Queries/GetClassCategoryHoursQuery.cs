using Space.Domain.Entities;

namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(int Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

internal class GetClassCategoryHoursQueryHandler : IRequestHandler<GetClassCategoryHoursQuery, IEnumerable<GetClassSessionCategoryHoursResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassCategoryHoursQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSessionCategoryHoursResponseDto>> Handle(GetClassCategoryHoursQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.Date == requestDate && c.ClassId == @class.Id)
            .ToListAsync(cancellationToken: cancellationToken);



        if (!classSessions.Any() && classSessions.Any(c => c.IsHoliday == true)) throw new NotFoundException("Class session not found");

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.Date == requestDate && c.ClassId == @class.Id)
            .ToListAsync(cancellationToken: cancellationToken);


        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = new List<GetClassSessionCategoryHoursResponseDto>();
        if (classTimeSheets.Count == 0)
        {
            response = classSessions
           .Select(c => new GetClassSessionCategoryHoursResponseDto()
           {
               Category = c.Category,
               Status = classTimeSheets.Where(ct => ct.Status == c.Status).FirstOrDefault() != null ?
                        classTimeSheets.Where(ct => ct.Status == c.Status).First().Status : c.Status,
               Hour = c.TotalHours
           });
        }
        else
        {
            response = classTimeSheets
            .Select(c => new GetClassSessionCategoryHoursResponseDto()
            {
                Category = c.Category,
                Status = c.Status,
                Hour = c.TotalHours
            });

        }
        return response;

    }
}
