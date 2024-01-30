namespace Space.Application.Handlers.Queries;

public record GetProgramQuery(int Id) : IRequest<GetProgramResponseDto>;
internal class GetProgramQueryHandler : IRequestHandler<GetProgramQuery, GetProgramResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetProgramQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetProgramResponseDto> Handle(GetProgramQuery request, CancellationToken cancellationToken)
    {
        Program? program = await _spaceDbContext.Programs.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Program), request.Id);
        return _mapper.Map<GetProgramResponseDto>(program);
    }
}