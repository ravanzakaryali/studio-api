namespace Space.Infrastructure.Persistence.Concrete;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    private readonly SpaceDbContext _context;

    public Repository(SpaceDbContext context)
    {
        _context = context;
    }

    public DbSet<TEntity> Table => _context.Set<TEntity>();
    public async Task<TEntity?> GetAsync(Guid id, bool tracking = true, params string[] includes)
    {
        IQueryable<TEntity> query = Table.GetIncludeQuery(includes);
        if (!tracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true, params string[] includes)
    {

        IQueryable<TEntity> query = Table.GetIncludeQuery(includes);
        if (!tracking)
            query = query.AsNoTracking();
        return predicate is null
            ? await query.FirstOrDefaultAsync()
            : await query.Where(predicate).PredicateEntityActive(predicate).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true, params string[] includes)
    {
        IQueryable<TEntity> query = Table.GetIncludeQuery(includes);
        if (!tracking)
            query = query.AsNoTracking();
        return predicate is null
            ? await query.ToListAsync()
            : await query.Where(predicate).PredicateEntityActive(predicate).ToListAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(int page, int size, Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true, params string[] includes)
    {
        IQueryable<TEntity> query = Table.GetIncludeQuery(includes).Skip((page - 1) * size).Take(size);
        if (!tracking)
            query = query.AsNoTracking();
        return predicate is null
            ? await query.ToListAsync()
            : await query.Where(predicate).PredicateEntityActive(predicate).ToListAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync<TOrderBy>(int page, int size, Expression<Func<TEntity, TOrderBy>> orderBy, bool isOrderBy = true, Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true, params string[] includes)
    {
        IQueryable<TEntity> query = Table.GetIncludeQuery(includes).Skip((page - 1) * size).Take(size);
        if (!tracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query.Where(predicate).PredicateEntityActive(predicate);

        if (isOrderBy)
            query.OrderBy(orderBy);
        else
            query.OrderByDescending(orderBy);

        return await query.ToListAsync();
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        EntityEntry<TEntity> entry = await Table.AddAsync(entity);
        entry.State = EntityState.Added;
        return entry.Entity;
    }

    public TEntity Remove(TEntity entity, bool isHardDelete = false)
    {
        EntityEntry<TEntity> entry = Table.Remove(entity);
        if (isHardDelete)
            entry.State = EntityState.Deleted;
        else
        {
            entity.IsDeleted = true;
            entry.State = EntityState.Modified;
        }
        return entry.Entity;
    }
    public void RemoveRange(IEnumerable<TEntity> entities, bool isHardDelete = false)
    {
        foreach (TEntity entity in entities)
        {
            EntityEntry<TEntity> entry = Table.Remove(entity);
            if (isHardDelete)
                entry.State = EntityState.Deleted;
            else
            {
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
    public void Update(TEntity entity)
    {
        EntityEntry<TEntity> entry = Table.Update(entity);
        entry.State = EntityState.Modified;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await Table.AddRangeAsync(entities);
    }
}
