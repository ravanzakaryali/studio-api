namespace Space.Application.Handlers;

public record GetWorkerQuery(int Id) : IRequest<GetWorkerByIdDto>;
public class GetWorkerQueryCommand : IRequestHandler<GetWorkerQuery, GetWorkerByIdDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetWorkerQueryCommand(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerByIdDto> Handle(GetWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Worker), request.Id);
        return _mapper.Map<GetWorkerByIdDto>(worker);
    }
}
