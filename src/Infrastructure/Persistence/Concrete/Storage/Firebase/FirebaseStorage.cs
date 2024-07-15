using Firebase.Storage;
using Microsoft.AspNetCore.Http;

namespace SikayetVar.Infrastructure.Concretes;

public class FireBaseStorage : StorageHelper, IStorage
{
    readonly IConfiguration _configuration;

    public FireBaseStorage(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Delete(string fileName, params string[] paths)
    {
        throw new NotImplementedException();
    }

    public List<string> GetFiles(params string[] paths)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasFile(string fileName, params string[] paths)
    {
        var storage = new FirebaseStorage("studio-35145.appspot.com");
        string path = string.Join("/", paths);
        var fileReference = storage.Child(path).Child(fileName);

        var url = await fileReference.GetDownloadUrlAsync();
        return true;

    }

    public async Task<FileUploadResponse> UploadFileAsync(IFormFile file, params string[] paths)
    {
        FirebaseStorage storage = new("studio-35145.appspot.com");


        string fileName = await FileRename(file.FileName);
        FirebaseStorageReference fileReference = storage.Child(fileName);

        await fileReference.PutAsync(file.OpenReadStream());

        var url = await fileReference.GetDownloadUrlAsync();
        var response = new FileUploadResponse
        {
            FileName = fileName,
            PathName = url,
            Size = file.Length,
            Extension = Path.GetExtension(file.FileName),
            ContentType = file.ContentType,
        };

        return response;
    }

    public async Task<List<FileUploadResponse>> UploadFilesAsync(IFormFileCollection files, params string[] paths)
    {
        FirebaseStorage storage = new FirebaseStorage("studio-35145.appspot.com");

        List<FileUploadResponse> datas = new();

        foreach (IFormFile file in files)
        {
            string fileName = await FileRename(file.FileName);
            FirebaseStorageReference fileReference = storage.Child(fileName);

            await fileReference.PutAsync(file.OpenReadStream());

            var url = await fileReference.GetDownloadUrlAsync();
            datas.Add(new FileUploadResponse
            {
                FileName = fileName,
                PathName = url,
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
            });
        }

        return datas;
    }

}