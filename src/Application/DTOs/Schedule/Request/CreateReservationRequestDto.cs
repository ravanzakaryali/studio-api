using System;
namespace Space.Application.DTOs;


public class CreateReservationRequestDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomId { get; set; }
    public List<string> WorkersId { get; set; } = null!;
}

