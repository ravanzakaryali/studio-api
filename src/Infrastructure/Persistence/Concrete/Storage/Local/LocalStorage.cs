using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Space.Application.Extensions;

namespace Space.Infrastructure.Persistence;

public class LocalStorage : StorageHelper, ILocalStorage
{
    readonly IWebHostEnvironment _webHostEnvironment;
    readonly ICurrentUserService _currentUserService;

    public LocalStorage(IWebHostEnvironment webHostEnvironment, ICurrentUserService currentUserService)
    {

        _webHostEnvironment = webHostEnvironment;
        _currentUserService = currentUserService;
    }
    public void Delete(string fileName, params string[] paths)
    {
        string path = Path.Combine(paths);
        IO.File.Delete(Path.Combine(_webHostEnvironment.ContentRootPath, path, fileName));
    }

    public List<string> GetFiles(params string[] paths)
    {
        string path = Path.Combine(paths);
        DirectoryInfo directory = new(path);
        return directory.GetFiles().Select(f => f.Name).ToList();
    }

    public bool HasFile(string fileName, params string[] paths)
    {
        string path = Path.Combine(paths);
        return IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, path, fileName));
    }
    public async Task<List<FileUploadResponse>> UploadAsync(IFormFileCollection files, params string[] paths)
    {
        string currentUserEmail = _currentUserService.Email ?? "";
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<FileUploadResponse> datas = new();
        foreach (IFormFile file in files)
        {
            //Todo: File Name
            string fileNewName = FileRename(HasFile, file.FileName, currentUserEmail).CharacterRegulatory(int.MaxValue);
            using (FileStream fileStream = IO.File.Create(Path.Combine(uploadPath, fileNewName)))
            {
                await file.CopyToAsync(fileStream);
            }
            datas.Add(new FileUploadResponse()
            {
                FileName = fileNewName,
                PathName = Path.Combine(uploadPath, fileNewName),
                Size = file.Length,
                Extension = Path.GetExtension(fileNewName),
                ContentType = file.ContentType,
            });
        }

        return datas;
    }

    public async Task<FileUploadResponse> UploadAsync(IFormFile file, params string[] paths)
    {
        string currentUserEmail = _currentUserService.Email ?? "";
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        string fileNewName = FileRename(HasFile, file.FileName, currentUserEmail).CharacterRegulatory(int.MaxValue);
        using (FileStream fileStream = IO.File.Create(Path.Combine(uploadPath, fileNewName)))
        {
            await file.CopyToAsync(fileStream);
        }
        return new FileUploadResponse()
        {
            FileName = fileNewName,
            PathName = Path.Combine(uploadPath, fileNewName),
            Size = file.Length,
            Extension = Path.GetExtension(fileNewName),
            ContentType = file.ContentType,
        };

    }
}
