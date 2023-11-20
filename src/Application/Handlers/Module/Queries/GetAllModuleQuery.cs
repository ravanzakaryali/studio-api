namespace Space.Application.Handlers.Queries;

public class GetAllModuleQuery : IRequest<IEnumerable<GetModuleDto>>
{
}
internal class GetAllModuleQueryHandler : IRequestHandler<GetAllModuleQuery, IEnumerable<GetModuleDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllModuleQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetModuleDto>>(await _spaceDbContext.Modules.ToListAsync());
    }
}
