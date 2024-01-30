namespace Space.Application.Handlers;

public record DeleteSessionCommand(int Id) : IRequest<GetSessionResponseDto>;
internal class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, GetSessionResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public DeleteSessionCommandHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSessionResponseDto> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _spaceDbContext.Sessions.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Session), request.Id);
        _spaceDbContext.Sessions.Remove(session);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetSessionResponseDto>(session);
    }
}
