namespace Space.Application.Handlers.Queries;

public class GetAllModuleQuery : IRequest<IEnumerable<GetModuleDto>>
{
}
internal class GetAllModuleQueryHandler : IRequestHandler<GetAllModuleQuery, IEnumerable<GetModuleDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetModuleDto>>(await _unitOfWork.ModuleRepository.GetAllAsync());
    }
}
