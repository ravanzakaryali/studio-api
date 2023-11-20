namespace Space.Application.Handlers.Commands;

public record DeleteProgramCommand(Guid Id) : IRequest;

internal class DeleteProgramCommandHandler : IRequestHandler<DeleteProgramCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteProgramCommandHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(DeleteProgramCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _spaceDbContext.Programs.FindAsync(request.Id)
                ?? throw new NotFoundException(nameof(Program), request.Id);
        program.IsDeleted = true;
        await _spaceDbContext.SaveChangesAsync();
    }
}
