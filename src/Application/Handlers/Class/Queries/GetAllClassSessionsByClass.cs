namespace Space.Application.Handlers;

public record GetAllClassSessionsByClassQuery(Guid Id) : IRequest<IEnumerable<GetAllClassSessionByClassResponseDto>>;


public class GetAllClassSessionsByClassQueryHandler : IRequestHandler<GetAllClassSessionsByClassQuery, IEnumerable<GetAllClassSessionByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllClassSessionsByClassQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllClassSessionByClassResponseDto>> Handle(GetAllClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {

        Class @class = await _unitOfWork.ClassRepository.GetAsync(request.Id, false, "ClassSessions") ?? throw new NotFoundException();


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
