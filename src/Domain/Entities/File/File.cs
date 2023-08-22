namespace Space.Domain.Entities;

public class File : BaseEntity
{
    public string? Storage { get; set; }
    public string? Path { get; set; }
    public string FileName { get; set; } = null!;
    public double Size { get; set; }
    public string Extension { get; set; } = null!;
}
