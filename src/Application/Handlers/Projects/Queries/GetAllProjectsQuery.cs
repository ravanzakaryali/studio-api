namespace Space.Application.Handlers.Queries;

public class GetAllProjectsQuery : IRequest<IEnumerable<GetAllProjectResponseDto>>
{

}
internal class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<GetAllProjectResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllProjectsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllProjectResponseDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        List<Project> projects = await _spaceDbContext.Projects.ToListAsync();
        IEnumerable<GetAllProjectResponseDto> projectsDto = projects.Select(p => new GetAllProjectResponseDto()
        {
            Id = p.Id,
            Name = p.Name
        });

        return projectsDto;
    }
}
