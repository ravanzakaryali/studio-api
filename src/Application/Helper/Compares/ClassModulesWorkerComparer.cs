namespace Space.Application.Compares;

public class ClassModulesWorkerComparer : IEqualityComparer<ClassModulesWorker>
{
    public bool Equals(ClassModulesWorker? x, ClassModulesWorker? y)
    {
        return x?.WorkerId == y?.WorkerId && x?.RoleId == y?.RoleId;
    }

    public int GetHashCode(ClassModulesWorker obj)
    {
        return obj.WorkerId.GetHashCode() ^ obj.RoleId.GetHashCode();
    }
}
