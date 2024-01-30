namespace Space.Application.Handlers.Commands;

public record CreateProgramCommand(string Name, int TotalHours) : IRequest;
internal class CreateProgramCommandHandler : IRequestHandler<CreateProgramCommand>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;
    public CreateProgramCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateProgramCommand request, CancellationToken cancellationToken)
    {
        Program program = _mapper.Map<Program>(request);
        Program? programDb = await _spaceDbContext.Programs.Where(p => program.Name == p.Name).FirstOrDefaultAsync();
        if (programDb is not null) throw new AlreadyExistsException(nameof(Program), request.Name);
        await _spaceDbContext.Programs.AddAsync(program);
        await _spaceDbContext.SaveChangesAsync();
    }
}
