using System.ComponentModel;

namespace Space.Domain.Enums;

public enum AttendanceStatus
{
    [Description("Tam iştirak")]
    Full = 1,
    [Description("Erkən çıxma")]
    Late = 2,
    [Description("Gecikmə")]
    Early = 3,
}
