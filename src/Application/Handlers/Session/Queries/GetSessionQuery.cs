namespace Space.Application.Handlers;

public record GetSessionQuery(Guid Id) : IRequest<GetSessionWithDetailsResponseDto>;

internal class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, GetSessionWithDetailsResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetSessionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        Session? session = await _unitOfWork.SessionRepository.GetAsync(s=>s.Id == request.Id,includeProperties: "Details") ?? 
            throw new NotFoundException(nameof(Session),request.Id);
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
