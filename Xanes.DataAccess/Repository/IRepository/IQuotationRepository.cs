using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IQuotationRepository : IRepository<Quotation>
{
    void Update(Quotation obj);
}