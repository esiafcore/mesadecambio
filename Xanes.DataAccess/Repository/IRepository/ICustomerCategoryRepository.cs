using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICustomerCategoryRepository : IRepository<CustomerCategory>
{
    void Update(CustomerCategory obj);
}