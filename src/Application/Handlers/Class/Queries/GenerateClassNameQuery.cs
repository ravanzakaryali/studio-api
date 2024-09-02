namespace Space.Application.Handlers.Queries;

public class GenerateClassNameQuery : IRequest<GetClassNameDto>
{
    public int ProgramId { get; set; }
    public int SessionId { get; set; }
    public DateOnly StartDate { get; set; }
}
internal class GenerateClassNameQueryHandler : IRequestHandler<GenerateClassNameQuery, GetClassNameDto>
{
    private readonly ISpaceDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateClassNameQueryHandler(ISpaceDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetClassNameDto> Handle(GenerateClassNameQuery request, CancellationToken cancellationToken)
    {
        Program? program = await _context.Programs
            .FirstOrDefaultAsync(x => x.Id == request.ProgramId, cancellationToken)
            ?? throw new NotFoundException(nameof(Program), request.ProgramId);

        Session? session = await _context.Sessions
            .Include(c => c.Details)
            .FirstOrDefaultAsync(x => x.Id == request.SessionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Session), request.SessionId);

        DateOnly endDate = await _unitOfWork.ClassService.EndDateCalculationAsync(new Class()
        {
            Program = program,
            Session = session,
            StartDate = request.StartDate,
        });

        string generateName = await _unitOfWork.ClassService.GenerateClassName(new Class()
        {
            Program = program,
            Session = session,
            StartDate = request.StartDate
        });

        return new GetClassNameDto
        {
            Name = generateName,
            Program = new GetProgramResponseDto
            {
                Id = program.Id,
                Name = program.Name,
                TotalHours = program.TotalHours
            },
            StartDate = request.StartDate,
            EndDate = endDate,
            Session = new GetSessionResponseDto
            {
                Id = session.Id,
                Name = session.Name,

            },
        };
    }
}