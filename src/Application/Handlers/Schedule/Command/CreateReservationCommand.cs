using System;

namespace Space.Application.Handlers;


public record CreateReservationCommand(string Title,
                                       string Description,
                                       DateTime StartDate,
                                       DateTime EndDate,
                                       Guid RoomId,
                                       List<string> WorkersId) : IRequest<CreateReservationResponseDto>;



internal class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationResponseDto>
{

    readonly IUnitOfWork _unitOfWork;


    public CreateReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public async Task<CreateReservationResponseDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {

        Reservation reservation = new Reservation();
        reservation.Title = request.Title;
        reservation.Description = request.Description;


        await _unitOfWork.ReservationRepository.AddAsync(reservation);

        if (request.StartDate.DayOfYear != request.EndDate.DayOfYear)
        {

        }

        RoomSchedule roomSchedule = new();
        roomSchedule.Category = EnumScheduleCategory.Reservation;
        roomSchedule.RoomId = request.RoomId;



        throw new NotImplementedException();
    }
}
