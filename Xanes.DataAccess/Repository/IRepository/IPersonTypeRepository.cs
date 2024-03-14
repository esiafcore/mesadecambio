using Xanes.Models;

namespace Xanes.DataAccess.Repository.IRepository;

public interface IPersonTypeRepository : IRepository<PersonType>
{
    void Update(PersonType obj);
}