namespace Space.Application.Compares;

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


