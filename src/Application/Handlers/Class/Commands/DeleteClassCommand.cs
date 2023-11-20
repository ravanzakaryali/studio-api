namespace Space.Application.Handlers.Commands;

public record DeleteClassCommand(Guid Id) : IRequest<GetClassResponseDto>;

internal class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, GetClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteClassCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetClassResponseDto> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);
        @class.IsDeleted = true;
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetClassResponseDto>(@class);
    }
}
