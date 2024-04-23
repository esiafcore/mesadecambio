using System.Linq.Expressions;
using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICustomerRepository : IRepository<Customer>
{
    void Update(Customer obj);
    Task<long> NextSequentialNumber(Expression<Func<Customer, bool>>? filter = null);
}