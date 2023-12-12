namespace Space.Domain.Entities;

public class SupportImage : File
{
    public int SupportId { get; set; }
    public Support Support { get; set; } = null!;
}
