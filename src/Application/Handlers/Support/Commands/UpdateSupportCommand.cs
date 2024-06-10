namespace Space.Application.Handlers;

public class UpdateSupportCommand : IRequest
{
    public int Id { get; set; }
    public SupportStatus Status { get; set; }
    public string? Note { get; set; }
}

internal class UpdateSupportCommandHandler : IRequestHandler<UpdateSupportCommand>
{
    private readonly ISpaceDbContext _context;
    public UpdateSupportCommandHandler(ISpaceDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSupportCommand request, CancellationToken cancellationToken)
    {
        Support support = await _context.Supports.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Support), request.Id);

        support.Status = request.Status;
        support.Note = request.Note;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}