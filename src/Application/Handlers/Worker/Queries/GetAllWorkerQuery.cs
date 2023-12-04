namespace Space.Application.Handlers;

public record GetAllWorkerQuery : IRequest<IEnumerable<GetWorkerDto>>;


internal class GetAllWorkerQueryHandler : IRequestHandler<GetAllWorkerQuery, IEnumerable<GetWorkerDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllWorkerQueryHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetWorkerDto>> Handle(GetAllWorkerQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetWorkerDto> workers = _mapper.Map<IEnumerable<GetWorkerDto>>(
            await _spaceDbContext.Workers
                .Include(c => c.UserRoles)
                .ThenInclude(c => c.Role)
                .ToListAsync(cancellationToken: cancellationToken));

        return workers.ToList().OrderBy(w => w.Name);
    }
}
