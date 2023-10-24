using Azure.Core;
using System;

namespace Space.Infrastructure.Persistence;
internal class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    readonly SpaceDbContext _dbContext;
    public AttendanceRepository(SpaceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    
}


