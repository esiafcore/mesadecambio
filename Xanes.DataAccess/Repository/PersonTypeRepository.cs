using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class PersonTypeRepository : Repository<PersonType>, IPersonTypeRepository
{
    private readonly ApplicationDbContext _db;

    public PersonTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(PersonType obj)
    {
        _db.PersonsTypes.Update(obj);
    }
}