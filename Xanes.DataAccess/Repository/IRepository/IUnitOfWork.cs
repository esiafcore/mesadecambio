namespace Xanes.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBankRepository Bank { get; }
    void Save();
}