namespace Space.Domain.Entities
{
    public class PermissionGroupAppModulePermissionLevel : BaseEntity
    {
        public int ApplicationModuleId { get; set; }
        public ApplicationModule ApplicationModule { get; set; } = null!;
        public int PermissionLevelId { get; set; }
        public PermissionLevel PermissionLevel { get; set; } = null!;
        public int PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; } = null!;
    }
}