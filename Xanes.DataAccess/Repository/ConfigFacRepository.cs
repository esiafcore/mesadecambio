using System.Linq;
using System.Linq.Expressions;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository;

public class ConfigFacRepository : Repository<ConfigFac>, IConfigFacRepository
{
    private readonly ApplicationDbContext _db;

    public ConfigFacRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ConfigFac obj)
    {
        _db.ConfigsFac.Update(obj);
    }

    public async Task<long> NextSequentialNumber(Expression<Func<ConfigFac, bool>>? filter = null
        , SD.TypeSequential typeSequential = SD.TypeSequential.Draft,
        bool mustUpdate = false)
    {
        IQueryable<ConfigFac> query = _db.Set<ConfigFac>();
        if (filter != null)
        {
            query = query.Where(filter);

        }
        long nextSequential = 0L;

        var itemRecord = query
            .FirstOrDefault();

        if (itemRecord != null)
        {
            nextSequential = typeSequential == SD.TypeSequential.Official ? itemRecord.SequentialNumberQuotation
                : itemRecord.SequentialNumberDraftQuotation;
            nextSequential++;

            if (mustUpdate)
            {
                if (typeSequential == SD.TypeSequential.Official)
                {
                    itemRecord.SequentialNumberQuotation = nextSequential;
                }
                else
                {
                    itemRecord.SequentialNumberDraftQuotation = nextSequential;
                }
                Update(itemRecord);
            }
        }

        return await Task.FromResult(nextSequential);
    }
}