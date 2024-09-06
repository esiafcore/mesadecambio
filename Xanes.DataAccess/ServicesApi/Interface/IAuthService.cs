using Xanes.Models.Dtos.XanesN8;

namespace Xanes.DataAccess.ServicesApi.Interface;
public interface IAuthService
{
    Task<T> LoginAsync<T>(CredencialesUsuarioDto obj);
    Task<T> RegisterAsync<T>(CredencialesUsuarioDto obj);
}

