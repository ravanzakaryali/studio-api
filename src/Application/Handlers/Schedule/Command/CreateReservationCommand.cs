namespace Space.Application.Handlers;


public class CreateReservationCommand : IRequest<CreateReservationResponseDto>
{
    public CreateReservationCommand()
    {
        WorkersId = new HashSet<string>();
    }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomId { get; set; }
    public ICollection<string> WorkersId { get; set; }
}



internal class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateReservationCommandHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<CreateReservationResponseDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        Reservation reservation = new()
        {
            Title = request.Title,
            Description = request.Description,
        };

        await _spaceDbContext.Reservations.AddAsync(reservation);

        if (request.StartDate.DayOfYear != request.EndDate.DayOfYear)
        {

        }

        RoomSchedule roomSchedule = new()
        {
            Category = EnumScheduleCategory.Reservation,
            RoomId = request.RoomId,
            Reservation = reservation,
            StartTime = request.StartDate.ToString("hh:mm tt"),
            EndTime = request.EndDate.ToString("hh:mm tt"),
            DayOfMonth = request.StartDate.Month,
            DayOfWeek = Convert.ToInt32(request.StartDate.DayOfWeek),
            Year = request.EndDate.Year,
        };

        throw new NotImplementedException();
    }
}
