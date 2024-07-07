using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IBusinessExecutiveRepository : IRepository<BusinessExecutive>
{
    void Update(BusinessExecutive obj);
}