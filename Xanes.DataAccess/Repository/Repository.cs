using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Helpers;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Utility;

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

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { AC.SeparationCharProperties }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        if (!isTracking)
            query = query.AsNoTracking();

        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public async Task<PagedList<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter,
        List<Expression<Func<T, object>>>? orderExpressions = null,
        SD.OrderDirection orderDirection = SD.OrderDirection.Desc,
        string? includeProperties = null,
        int pageSize = 15, int pageNumber = 1)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { AC.SeparationCharProperties }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        //Para varios campos
        if (orderExpressions != null && orderExpressions.Any())
        {
            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var order in orderExpressions)
            {
                switch (orderDirection)
                {
                    case SD.OrderDirection.Asc:
                        orderedQuery = orderedQuery == null
                            ? query.OrderBy(order)
                            : orderedQuery.ThenBy(order);
                        break;
                    case SD.OrderDirection.Desc:
                        orderedQuery = orderedQuery == null
                            ? query.OrderByDescending(order)
                            : orderedQuery.ThenByDescending(order);
                        break;
                }
            }

            query = orderedQuery ?? query;
        }

        pageSize = Math.Min(pageSize, 50);

        return await PagedList<T>.ToPagedList(query, pageNumber, pageSize);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { AC.SeparationCharProperties }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
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

    public async Task<bool> IsExists(Expression<Func<T, bool>>? filter = null)
    {
        if (filter != null)
        {
            IQueryable<T> query = dbSet;
            return await query.AnyAsync(filter);
        }
        return await Task.FromResult(false);
    }

    public bool RemoveByFilter(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);
        return query.ExecuteDelete() > 0;
    }


}