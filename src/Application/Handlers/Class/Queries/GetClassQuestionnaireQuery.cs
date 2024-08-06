namespace Space.Application.Handlers;

public class GetClassQuestionnaireQuery : IRequest<IEnumerable<GetAllClassDto>>
{

}
internal class GetClassQuestionnaireQueryHandler : IRequestHandler<GetClassQuestionnaireQuery, IEnumerable<GetAllClassDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassQuestionnaireQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllClassDto>> Handle(GetClassQuestionnaireQuery request, CancellationToken cancellationToken)
    {
        throw new Exception("Not implemented");
    }

}