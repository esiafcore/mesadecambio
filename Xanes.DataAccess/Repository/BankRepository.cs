using Xanes.DataAccess.Data;
using Xanes.DataAccess.Repository.IRepository;
using Xanes.Models;

namespace Xanes.DataAccess.Repository;

public class BankRepository : Repository<Bank>, IBankRepository
{
    private readonly ApplicationDbContext _db;

    public BankRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Bank obj)
    {
        var objFromDb = _db.Banks.FirstOrDefault(x => (x.Id == obj.Id));

        if (objFromDb != null)
        {
            objFromDb.Code = obj.Code;
            objFromDb.Name = obj.Name;
            objFromDb.BankingCommissionPercentage = obj.BankingCommissionPercentage;
            objFromDb.BankAccountExcludeUId = obj.BankAccountExcludeUId;
            objFromDb.IsCompany = obj.IsCompany;
            objFromDb.OrderPriority = obj.OrderPriority;
            objFromDb.IsActive = obj.IsActive;

            objFromDb.CreatedDate = obj.CreatedDate;
            objFromDb.CreatedBy = obj.CreatedBy;
            objFromDb.CreatedIpv4 = obj.CreatedIpv4;
            objFromDb.CreatedHostName = obj.CreatedHostName;
            objFromDb.UpdatedDate = obj.UpdatedDate;
            objFromDb.UpdatedBy = obj.UpdatedBy;
            objFromDb.UpdatedIpv4 = obj.UpdatedIpv4;
            objFromDb.UpdatedHostName = obj.UpdatedHostName;
            objFromDb.InactivatedDate = obj.InactivatedDate;
            objFromDb.InactivatedBy = obj.InactivatedBy;
            objFromDb.InactivatedIpv4 = obj.InactivatedIpv4;
            objFromDb.InactivatedHostName = obj.InactivatedHostName;

            if (obj.LogoUrl != null)
            {
                objFromDb.LogoUrl = obj.LogoUrl;
            }
        }
    }

}