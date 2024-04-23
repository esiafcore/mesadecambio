using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;
using Xanes.Utility;

namespace Xanes.DataAccess.Repository;

public class ConfigCxcRepository : Repository<ConfigCxc>, IConfigCxcRepository
{
    private readonly ApplicationDbContext _db;
    private IConfigCxcRepository _configCxcRepositoryImplementation;

    public ConfigCxcRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ConfigCxc obj)
    {
        _db.ConfigsCxc.Update(obj);
    }

    public async Task<long> NextSequentialNumber(Expression<Func<ConfigCxc, bool>>? filter = null
        , SD.TypeSequential typeSequential = SD.TypeSequential.Draft
        , bool mustUpdate = false)
    {
        IQueryable<ConfigCxc> query = _db.Set<ConfigCxc>();
        if (filter != null)
        {
            query = query.Where(filter);

        }
        long nextSequential = 0L;

        var itemRecord = query
            .FirstOrDefault();

        if (itemRecord != null)
        {
            nextSequential = typeSequential == SD.TypeSequential.Official ? itemRecord.SequentialNumberCustomer
                : itemRecord.SequentialNumberDraftCustomer;
            nextSequential++;

            if (typeSequential == SD.TypeSequential.Official)
            {
                itemRecord.SequentialNumberCustomer = nextSequential;
            }
            else
            {
                itemRecord.SequentialNumberDraftCustomer = nextSequential;
            }
            Update(itemRecord);
        }

        return await Task.FromResult(nextSequential);

    }

}