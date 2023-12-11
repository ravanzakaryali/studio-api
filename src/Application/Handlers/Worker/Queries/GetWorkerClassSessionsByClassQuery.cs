namespace Space.Application.Handlers;

public record GetWorkerClassSessionsByClassQuery(int Id) : IRequest<GetWorkerClassSessionsByClassResponseDto>;


internal class GetWorkerClassSessionsByClassQueryHandler : IRequestHandler<GetWorkerClassSessionsByClassQuery, GetWorkerClassSessionsByClassResponseDto>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetWorkerClassSessionsByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerClassSessionsByClassResponseDto> Handle(GetWorkerClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {
        throw new NotFoundException();
    }
}
