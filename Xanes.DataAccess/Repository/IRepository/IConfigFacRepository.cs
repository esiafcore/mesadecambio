using System.Linq.Expressions;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IConfigFacRepository : IRepository<ConfigFac>
{
    void Update(ConfigFac obj);
    Task<long> NextSequentialNumber(Expression<Func<ConfigFac, bool>>? filter = null
        , SD.TypeSequential typeSequential = SD.TypeSequential.Draft
        , bool mustUpdate = false);
}