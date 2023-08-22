namespace Space.Application.Handlers.Queries;

public class GetAllProgramsQuery : IRequest<IEnumerable<GetAllProgramResponseDto>>
{

}
public class GetAllProgramsQueryHandler : IRequestHandler<GetAllProgramsQuery, IEnumerable<GetAllProgramResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    public GetAllProgramsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllProgramResponseDto>> Handle(GetAllProgramsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Program> programs = await _unitOfWork.ProgramRepository.GetAllAsync(includes: "Modules");
        programs.ToList().ForEach(a => a.Modules = a.Modules.Where(a => a.TopModuleId == null).ToList());
        var programss = _mapper.Map<IEnumerable<GetAllProgramResponseDto>>(programs);
        return programss;
    }
}
