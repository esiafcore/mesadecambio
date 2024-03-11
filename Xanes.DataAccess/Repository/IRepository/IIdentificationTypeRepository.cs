using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IIdentificationTypeRepository : IRepository<IdentificationType>
{
    void Update(IdentificationType obj);
}