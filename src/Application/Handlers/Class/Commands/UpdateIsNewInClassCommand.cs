namespace Space.Application.Handlers;

public record UpdateIsNewInClassCommand(Guid Id) : IRequest<GetUpdateIsNewInClassResponseDto>;
internal class UpdateIsNewInClassCommandHander : IRequestHandler<UpdateIsNewInClassCommand, GetUpdateIsNewInClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;

    public UpdateIsNewInClassCommandHander(IUnitOfWork unitOfWork, IMapper mapper, IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
    }

    public async Task<GetUpdateIsNewInClassResponseDto> Handle(UpdateIsNewInClassCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _classRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Class), request.Id);

        @class.IsNew = !@class.IsNew;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetUpdateIsNewInClassResponseDto>(@class);
    }
}
