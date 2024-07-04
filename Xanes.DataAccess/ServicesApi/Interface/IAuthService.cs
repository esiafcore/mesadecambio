using Xanes.Models.Dtos;

namespace Xanes.DataAccess.ServicesApi.Interface;
public interface IAuthService
{
    Task<string> LoginAsync(CredencialesUsuarioDto obj);
    Task<string> RegisterAsync(CredencialesUsuarioDto obj);
}

