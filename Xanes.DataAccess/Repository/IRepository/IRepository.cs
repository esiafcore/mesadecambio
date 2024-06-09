using System.Linq.Expressions;
using Xanes.DataAccess.Helpers;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<PagedList<T>> GetAllAsync(Expression<Func<T, bool>>? filter,
        List<Expression<Func<T, object>>>? orderExpressions = null,
        SD.OrderDirection orderDirection = SD.OrderDirection.Desc,
        string? includeProperties = null,
         int pageSize = 15, int pageNumber = 1);

    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null);

    T Get(Expression<Func<T,bool>> filter, string? includeProperties = null, bool isTracking=true);
    void Add(T entity);
    //Decisión personal. Tener fuera el método Update
    //void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<bool> IsExists(Expression<Func<T, bool>>? filter = null);
    bool RemoveByFilter(Expression<Func<T, bool>> filter);

}