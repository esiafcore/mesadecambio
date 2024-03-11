using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class IdentificationTypeRepository : Repository<IdentificationType>, IIdentificationTypeRepository
{
    private readonly ApplicationDbContext _db;

    public IdentificationTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(IdentificationType obj)
    {
        _db.IdentificationsTypes.Update(obj);
    }
}