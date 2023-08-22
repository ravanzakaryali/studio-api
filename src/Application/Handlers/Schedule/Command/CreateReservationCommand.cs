using System;

namespace Space.Application.Handlers;


public record CreateReservationCommand(CreateReservationRequestDto request) : IRequest<CreateReservationResponseDto>;



internal class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationResponseDto>
{

    readonly IUnitOfWork _unitOfWork;


    public CreateReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public async Task<CreateReservationResponseDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var requestModel = request.request;

        Reservation reservation = new Reservation();
        reservation.Title = requestModel.Title;
        reservation.Description = requestModel.Description;
        reservation.People = requestModel.People;


        await _unitOfWork.ReservationRepository.AddAsync(reservation);

        if (requestModel.StartDate.DayOfYear != requestModel.EndDate.DayOfYear)
        {

        }

        RoomSchedule roomSchedule = new RoomSchedule();
        roomSchedule.Category = EnumScheduleCategory.Reservation;
        roomSchedule.RoomId = request.request.RoomId;
        


        throw new NotImplementedException();
    }
}
