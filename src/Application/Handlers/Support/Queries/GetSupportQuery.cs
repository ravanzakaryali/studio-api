namespace Space.Application.Handlers;

public record GetSupportQuery(int Id) : IRequest<GetSupportResponseDto>;

internal class GetSupportQueryHandler : IRequestHandler<GetSupportQuery, GetSupportResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetSupportQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSupportResponseDto> Handle(GetSupportQuery request, CancellationToken cancellationToken)
    {
        Support? support = await _spaceDbContext.Supports
            .Include(c => c.SupportImages)
            .Include(c => c.User)
            .Include(c => c.SupportCategory)
            .Include(c => c.Class)
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Support), request.Id);

        return _mapper.Map<GetSupportResponseDto>(support);
    }
}
