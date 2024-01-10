namespace Space.Application.DTOs;

public class GetSchedulesRoomsResponseDto
{
    public GetSchedulesRoomsResponseDto()
    {
        OccupancyRates = new HashSet<OccupancyRate>();
    }
    public GetRoomResponseDto Room { get; set; } = null!;
    public ICollection<OccupancyRate> OccupancyRates { get; set; }

}
public class OccupancyRate
{
    public double Value { get; set; }
    public int MonthOfYear { get; set; }
}
