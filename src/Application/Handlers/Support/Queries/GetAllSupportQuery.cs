namespace Space.Application.Handlers;

public record GetAllSupportQuery : IRequest<IEnumerable<GetSupportResponseDto>>;

internal class GetAllSupportQueryHandler : IRequestHandler<GetAllSupportQuery, IEnumerable<GetSupportResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllSupportQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetSupportResponseDto>> Handle(GetAllSupportQuery request, CancellationToken cancellationToken)
    {
        List<Support> supports = await _spaceDbContext.Supports
            .Include(c => c.SupportImages)
            .Include(c => c.User)
            .Include(c => c.Class)
            .Include(c => c.SupportCategory)
            .ToListAsync(cancellationToken: cancellationToken);

        return _mapper.Map<List<GetSupportResponseDto>>(supports).OrderByDescending(c => c.CreatedDate);
    }

}
