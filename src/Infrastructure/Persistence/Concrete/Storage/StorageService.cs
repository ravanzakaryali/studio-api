using Microsoft.AspNetCore.Http;

namespace Space.Infrastructure.Persistence;

public class StorageService : IStorageService
{
    readonly IStorage _storage;

    public StorageService(IStorage storage)
    {
        _storage = storage;
    }
    public string StorageName { get => _storage.GetType().Name; }

    public void Delete(string fileName, params string[] paths)
        => _storage.Delete(fileName, paths);

    public List<string> GetFiles(params string[] paths)
        => _storage.GetFiles(paths);

    public bool HasFile(string fileName, params string[] paths)
        => _storage.HasFile(fileName, paths);

    public Task<List<FileUploadResponse>> UploadAsync(IFormFileCollection files, params string[] paths)
        => _storage.UploadAsync(files, paths);
}
