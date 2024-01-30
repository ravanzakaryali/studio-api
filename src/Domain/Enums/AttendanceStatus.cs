using System.ComponentModel;

namespace Space.Domain.Enums;

public enum AttendanceStatus
{
    [Description("Tam iştirak")]
    Full = 1,
    [Description("Gecikmə")]
    Late = 2,
    [Description("Erkən çıxma")]
    Early = 3,

    [Description("İştirak etməyib")]
    Absent = 4,
}
