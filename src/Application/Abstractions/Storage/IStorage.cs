namespace Space.Application.Abstractions;

public interface IStorage
{
    Task<List<FileUploadResponse>> UploadAsync(IFormFileCollection files, params string[] paths);
    void Delete(string fileName, params string[] paths);
    List<string> GetFiles(params string[] paths);
    bool HasFile(string fileName, params string[] paths);
}
