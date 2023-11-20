namespace Space.Application.Handlers;

public record UpdateIsNewInClassCommand(Guid Id) : IRequest<GetUpdateIsNewInClassResponseDto>;
internal class UpdateIsNewInClassCommandHander : IRequestHandler<UpdateIsNewInClassCommand, GetUpdateIsNewInClassResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _dbContext;

    public UpdateIsNewInClassCommandHander(
        IMapper mapper,
        ISpaceDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<GetUpdateIsNewInClassResponseDto> Handle(UpdateIsNewInClassCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _dbContext.Classes.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        @class.IsNew = !@class.IsNew;
        return _mapper.Map<GetUpdateIsNewInClassResponseDto>(@class);
    }
}
