namespace Space.Application.Abstractions;

public interface IStorageService : IStorage
{
    public string StorageName { get; }
}
