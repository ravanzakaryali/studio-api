using Space.Application.Extensions;

namespace Space.Infrastructure.Persistence;

public class StorageHelper
{
    public string FileRename(Func<string, string[], bool> hasfileMethod, string fileName, string email = "", params string[] paths)
    {
        string path = Path.Combine(paths);
        string newFileName = NewFileName(fileName, email);
        if (hasfileMethod(fileName, paths))
            return FileRename(hasfileMethod, fileName, email, path);
        else
            return newFileName;
    }
    string NewFileName(string fileName, string username)
    {
        string extension = Path.GetExtension(fileName);
        return string.Concat(username,".", Path.GetFileNameWithoutExtension(fileName)).ConcatWithDate(extension);
    }
}
