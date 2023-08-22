namespace Space.Application.Handlers.Commands;

public record CreateProgramCommand(string Name, int TotalHours) : IRequest;
internal class CreateProgramCommandHandler : IRequestHandler<CreateProgramCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    public CreateProgramCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateProgramCommand request, CancellationToken cancellationToken)
    {
        Program program = _mapper.Map<Program>(request);
        Program? programDb = await _unitOfWork.ProgramRepository.GetAsync(p => program.Name == p.Name);
        if (programDb is not null) throw new AlreadyExistsException(nameof(Program), request.Name);
        await _unitOfWork.ProgramRepository.AddAsync(program);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
