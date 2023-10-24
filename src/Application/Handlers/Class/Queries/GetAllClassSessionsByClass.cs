namespace Space.Application.Handlers;

public record GetAllClassSessionsByClassQuery(Guid Id) : IRequest<IEnumerable<GetAllClassSessionByClassResponseDto>>;


public class GetAllClassSessionsByClassQueryHandler : IRequestHandler<GetAllClassSessionsByClassQuery, IEnumerable<GetAllClassSessionByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;

    public GetAllClassSessionsByClassQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<GetAllClassSessionByClassResponseDto>> Handle(GetAllClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {

        Class @class = await _classRepository.GetAsync(request.Id, false, "ClassSessions") ?? throw new NotFoundException();


        var response = @class.ClassSessions.OrderByDescending(q => q.Date).DistinctBy(q => q.Date).Select(q => new GetAllClassSessionByClassResponseDto()
        {
            ClassName = q.Class.Name,
            ClassSessionDate = q.Date,
            ClassId = q.ClassId,
            ClassSessionStatus = q.Status
        });

        return response;

    }
}
