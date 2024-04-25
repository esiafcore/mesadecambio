using System.Linq.Expressions;
using Xanes.Models;
using Xanes.Utility;
using static Xanes.Utility.SD;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IQuotationRepository : IRepository<Quotation>
{
    void Update(Quotation obj);
    Task<int> NextSequentialNumber(Expression<Func<Quotation, bool>>? filter = null); 
    Task<bool> RemoveWithChildren(int id);

}