namespace Space.Application.Compares;

public class GetWorkerForClassDtoComparer : IEqualityComparer<ClassModulesWorker>
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
public class GetWorkerForClassDtoExtraModuleComparer : IEqualityComparer<ClassExtraModulesWorkers>
{
    public bool Equals(ClassExtraModulesWorkers? x, ClassExtraModulesWorkers? y)
    {
        return x?.WorkerId == y?.WorkerId && x?.RoleId == y?.RoleId;
    }

    public int GetHashCode(ClassExtraModulesWorkers obj)
    {
        return obj.WorkerId.GetHashCode() ^ obj.RoleId.GetHashCode();
    }
}