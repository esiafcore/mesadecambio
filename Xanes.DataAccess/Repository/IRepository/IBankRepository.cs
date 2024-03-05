using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IBankRepository : IRepository<Bank>
{
    void Update(Bank obj);
    void Save();
}