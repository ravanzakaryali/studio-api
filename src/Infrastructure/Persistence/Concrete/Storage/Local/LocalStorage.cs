using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Space.Application.Extensions;

namespace Space.Infrastructure.Persistence;

public class LocalStorage : StorageHelper, ILocalStorage
{
    readonly IWebHostEnvironment _webHostEnvironment;

    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {

        _webHostEnvironment = webHostEnvironment;

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

    public async Task<bool> HasFile(string fileName, params string[] paths)
    {
        string path = Path.Combine(paths);
        return IO.File.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, path, fileName));
    }
    public async Task<List<FileUploadResponse>> UploadFilesAsync(IFormFileCollection files, params string[] paths)
    {
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<FileUploadResponse> datas = new();
        foreach (IFormFile file in files)
        {
            //Todo: File Name
            string fileNewName = await FileRename(file.FileName);
            using (FileStream fileStream = IO.File.Create(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue))))
            {
                await file.CopyToAsync(fileStream);
            }

            FileUploadResponse fileResponse = new()
            {
                FileName = fileNewName.CharacterRegulatory(int.MaxValue),
                PathName = uploadPath,

                Size = file.Length,
                Extension = Path.GetExtension(fileNewName.CharacterRegulatory(int.MaxValue)),
                ContentType = file.ContentType,

            };

            // if (Helper.IsImageFile(fileNewName.CharacterRegulatory(int.MaxValue)))
            // {
            //     Image? img = Image.FromFile(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue)));
            //     if (img != null)
            //     {
            //         fileResponse.Height = img.Height;
            //         fileResponse.Width = img.Width;
            //     }
            // }
            datas.Add(fileResponse);
        }

        return datas;
    }

    public async Task<FileUploadResponse> UploadFileAsync(IFormFile file, params string[] paths)
    {
        string path = Path.Combine(paths);
        string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        string fileNewName = await FileRename(file.FileName);

        using (FileStream fileStream = IO.File.Create(Path.Combine(uploadPath, fileNewName.CharacterRegulatory(int.MaxValue))))
        {
            await file.CopyToAsync(fileStream);
        }

        return new FileUploadResponse()
        {
            FileName = fileNewName.CharacterRegulatory(int.MaxValue),
            PathName = uploadPath,
            Size = file.Length,
            Extension = Path.GetExtension(fileNewName),
            ContentType = file.ContentType,
        };
    }
}
