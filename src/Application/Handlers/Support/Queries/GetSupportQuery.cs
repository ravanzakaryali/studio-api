namespace Space.Application.Handlers;

public record GetSupportQuery(Guid Id) : IRequest<GetSupportResponseDto>;

internal class GetSupportQueryHandler : IRequestHandler<GetSupportQuery, GetSupportResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetSupportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSupportResponseDto> Handle(GetSupportQuery request, CancellationToken cancellationToken)
    {
        Support? support = await _unitOfWork.SupportRepository.GetAsync(request.Id, false, "SupportImages", "User")
            ?? throw new NotFoundException(nameof(Support), request.Id);

        return _mapper.Map<GetSupportResponseDto>(support);
    }
}
