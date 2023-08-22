namespace Space.Application.Handlers.Commands;

public record DeleteClassCommand(Guid Id) : IRequest<GetClassResponseDto>;

internal class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, GetClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public DeleteClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetClassResponseDto> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(c => c.Id == request.Id) ?? 
            throw new NotFoundException(nameof(Class),request.Id);
        _unitOfWork.ClassRepository.Remove(@class);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(@class);
    }
}
