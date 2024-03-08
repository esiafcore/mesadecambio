using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IQuotationTypeRepository : IRepository<QuotationType>
{
    void Update(QuotationType obj);

}