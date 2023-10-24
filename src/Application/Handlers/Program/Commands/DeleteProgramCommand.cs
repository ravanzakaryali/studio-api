namespace Space.Application.Handlers.Commands;

public record DeleteProgramCommand(Guid Id) : IRequest;

internal class DeleteProgramCommandHandler : IRequestHandler<DeleteProgramCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IProgramRepository _programRepository;

    public DeleteProgramCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProgramCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _programRepository.GetAsync(request.Id)
                ?? throw new NotFoundException(nameof(Program), request.Id);
        _programRepository.Remove(program);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
