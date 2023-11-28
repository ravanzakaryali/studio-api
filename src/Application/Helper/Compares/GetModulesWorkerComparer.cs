namespace Space.Application.Helper.Compares;

public class GetModulesWorkerComparer : IEqualityComparer<GetWorkerForClassDto>
{
    public bool Equals(GetWorkerForClassDto? x, GetWorkerForClassDto? y)
    {
        return x?.Id == y?.Id || x?.RoleId == y?.RoleId;
    }

    public int GetHashCode(GetWorkerForClassDto obj)
    {
        return obj.Id.GetHashCode() ^ obj.RoleId.GetHashCode();
    }

}
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
