namespace Space.Application.Handlers;

public record GetSupportCategoriesQuery : IRequest<IEnumerable<GetSupportCategory>>;

internal class GetSupportCategoriesQueryHandler : IRequestHandler<GetSupportCategoriesQuery, IEnumerable<GetSupportCategory>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetSupportCategoriesQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetSupportCategory>> Handle(GetSupportCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<SupportCategory> supportCategories = await _spaceDbContext.SupportCategories
            .ToListAsync(cancellationToken: cancellationToken);

        return supportCategories.Select(s => new GetSupportCategory()
        {
            Id = s.Id,
            Name = s.Name
        });
    }

}
