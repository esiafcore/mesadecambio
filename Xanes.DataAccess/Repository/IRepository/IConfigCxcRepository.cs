using System.Linq.Expressions;
using Xanes.Models;
using static Xanes.Utility.SD;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IConfigCxcRepository : IRepository<ConfigCxc>
{
    void Update(ConfigCxc obj);
    Task<long> NextSequentialNumber(Expression<Func<ConfigCxc, bool>>? filter = null
    ,TypeSequential typeSequential = TypeSequential.Draft
    ,bool mustUpdate = false);
}