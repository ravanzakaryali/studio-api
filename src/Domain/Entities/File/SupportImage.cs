namespace Space.Domain.Entities;

public class SupportImage : File
{
    public Guid SupportId { get; set; }
    public Support Support { get; set; } = null!;
}
