using Space.Application.DTOs.Enums;

namespace Space.Application.DTOs;

public class GetClassCountResponse
{
    public ClassStatus Status { get; set; }
    public int Count { get; set; }
}
