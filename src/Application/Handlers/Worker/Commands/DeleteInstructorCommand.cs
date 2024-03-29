namespace Space.Application.Handlers;

public record DeleteWorkerCommand(int Id) : IRequest<GetWorkerResponseDto>;
internal class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, GetWorkerResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public DeleteWorkerCommandHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerResponseDto> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? Worker = await _spaceDbContext.Workers.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Worker), request.Id);
        //Todo: support delete
        _spaceDbContext.Supports.RemoveRange(await _spaceDbContext.Supports.Where(x => x.UserId == Worker.Id).ToListAsync(cancellationToken: cancellationToken));
        _spaceDbContext.Workers.Remove(Worker);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(Worker);
    }
}
