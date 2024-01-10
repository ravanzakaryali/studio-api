namespace Space.Application.Handlers;


public record GetClassByProgramQuery(int Id) : IRequest<IEnumerable<GetClassByProgramQueryDto>>;

internal class GetClassByProgramQueryHandler : IRequestHandler<GetClassByProgramQuery, IEnumerable<GetClassByProgramQueryDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassByProgramQueryHandler(
    ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }


    public async Task<IEnumerable<GetClassByProgramQueryDto>> Handle(GetClassByProgramQuery request, CancellationToken cancellationToken)
    {

        List<Class> classes = await _spaceDbContext.Classes.Where(c => c.ProgramId == request.Id).ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<GetClassByProgramQueryDto> response = classes.Select(q => new GetClassByProgramQueryDto()
        {
            Id = q.Id,
            Name = q.Name
        });

        return response;
    }
}

