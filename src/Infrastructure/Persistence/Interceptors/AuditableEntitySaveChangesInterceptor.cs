using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Xml.Linq;
using System;

namespace Space.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    readonly ICurrentUserService _currentUserService;

    public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        foreach (EntityEntry<BaseAuditableEntity> entity in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedDate = DateTime.Now;
                if (entity.Entity.CreatedBy is null)
                {
                    entity.Entity.CreatedBy = _currentUserService.Email ?? null;
                }
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Entity.LastModifiedDate = DateTime.Now;
                entity.Entity.LastModifiedBy = _currentUserService.Email ?? null;
            }
        }
        foreach (EntityEntry<User> entity in context.ChangeTracker.Entries<User>())
        {
            if (entity.State == EntityState.Added)
            {
                if (entity.Entity.CreatedBy is null)
                {
                    entity.Entity.CreatedBy = _currentUserService.Email ?? null;
                }
            }
        }
    }
}