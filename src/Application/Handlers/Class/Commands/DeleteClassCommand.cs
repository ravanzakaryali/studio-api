namespace Space.Application.Handlers.Commands;

public record DeleteClassCommand(Guid Id) : IRequest<GetClassResponseDto>;

internal class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, GetClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;

    public DeleteClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
    }

    public async Task<GetClassResponseDto> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(c => c.Id == request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);
        _classRepository.Remove(@class);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(@class);
    }
}
