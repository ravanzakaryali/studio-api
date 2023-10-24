namespace Space.Application.Handlers.Queries;

public class GetAllModuleQuery : IRequest<IEnumerable<GetModuleDto>>
{
}
internal class GetAllModuleQueryHandler : IRequestHandler<GetAllModuleQuery, IEnumerable<GetModuleDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IModuleRepository _moduleRepository;

    public GetAllModuleQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IModuleRepository moduleRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _moduleRepository = moduleRepository;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetModuleDto>>(await _moduleRepository.GetAllAsync());
    }
}
