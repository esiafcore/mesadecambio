using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface ICurrencyRepository : IRepository<Currency>
{
    void Update(Currency obj);
}