namespace Space.Application.DTOs
{
    public class GetAllClassSessionByClassResponseDto
    {
        public string ClassName { get; set; } = null!;
        public DateOnly ClassSessionDate { get; set; }
        public ClassSessionStatus? ClassSessionStatus { get; set; }
        public int ClassId { get; set; }
    }
}
