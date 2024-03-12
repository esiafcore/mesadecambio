using System.Linq.Expressions;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null);
    T Get(Expression<Func<T,bool>> filter, bool isTracking=true);
    void Add(T entity);
    //Decisión personal. Tener fuera el método Update
    //void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
}