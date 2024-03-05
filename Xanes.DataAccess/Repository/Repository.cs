using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;

namespace Xanes.DataAccess.Repository;


public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }

    public T Get(Expression<Func<T, bool>> filter, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        if (!isTracking)
            query = query.AsNoTracking();

        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }


    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = dbSet;
        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}