using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IBankAccountRepository : IRepository<BankAccount>
{
    void Update(BankAccount obj);
}