namespace Space.Application.Handlers.Queries;

public class GetProgramsByProjectIdQuery : IRequest<IEnumerable<GetPropramsReponseDto>>
{
    public int ProjectId { get; set; }
}
internal class GetProgramsByProjectIdQueryHandler : IRequestHandler<GetProgramsByProjectIdQuery, IEnumerable<GetPropramsReponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetProgramsByProjectIdQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetPropramsReponseDto>> Handle(GetProgramsByProjectIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _spaceDbContext.Projects
            .Include(c => c.Programs)
            .FirstOrDefaultAsync(c => c.Id == request.ProjectId)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId);

        IEnumerable<GetPropramsReponseDto> programs = project.Programs.Select(p => new GetPropramsReponseDto()
        {
            Id = p.Id,
            Name = p.Name,
            TotalHours = p.TotalHours
        });
        return programs;
    }
}