using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICustomerRepository : IRepository<Customer>
{
    void Update(Customer obj);
}