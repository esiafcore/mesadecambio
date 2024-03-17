using System.Linq.Expressions;
using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICustomerSectorRepository : IRepository<CustomerSector>
{
    void Update(CustomerSector obj);
}