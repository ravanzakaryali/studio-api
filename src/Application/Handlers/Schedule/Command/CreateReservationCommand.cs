using System;

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
    readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateReservationResponseDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        Reservation reservation = new()
        {
            Title = request.Title,
            Description = request.Description,
        };

        await _unitOfWork.ReservationRepository.AddAsync(reservation);

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
