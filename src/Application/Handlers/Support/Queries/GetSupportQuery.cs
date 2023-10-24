namespace Space.Application.Handlers;

public record GetSupportQuery(Guid Id) : IRequest<GetSupportResponseDto>;

internal class GetSupportQueryHandler : IRequestHandler<GetSupportQuery, GetSupportResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISupportRepository _supportRepository;

    public GetSupportQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISupportRepository supportRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _supportRepository = supportRepository;
    }

    public async Task<GetSupportResponseDto> Handle(GetSupportQuery request, CancellationToken cancellationToken)
    {
        Support? support = await _supportRepository.GetAsync(request.Id, false, "SupportImages", "User")
            ?? throw new NotFoundException(nameof(Support), request.Id);

        return _mapper.Map<GetSupportResponseDto>(support);
    }
}
