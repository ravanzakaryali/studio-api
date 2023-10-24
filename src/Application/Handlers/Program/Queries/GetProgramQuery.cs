namespace Space.Application.Handlers.Queries;

public record GetProgramQuery(Guid Id) : IRequest<GetProgramResponseDto>;
internal class GetProgramQueryHandler : IRequestHandler<GetProgramQuery, GetProgramResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IProgramRepository _programRepository;

    public GetProgramQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IProgramRepository programRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _programRepository = programRepository;
    }

    public async Task<GetProgramResponseDto> Handle(GetProgramQuery request, CancellationToken cancellationToken)
    {
        Program? program = await _programRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Program), request.Id);
        return _mapper.Map<GetProgramResponseDto>(program);
    }
}