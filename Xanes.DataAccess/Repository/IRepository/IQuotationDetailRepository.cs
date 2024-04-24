using System.Linq.Expressions;
using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IQuotationDetailRepository : IRepository<QuotationDetail>
{
    void Update(QuotationDetail obj);
    Task<int> NextLineNumber(Expression<Func<QuotationDetail, bool>>? filter = null);
}