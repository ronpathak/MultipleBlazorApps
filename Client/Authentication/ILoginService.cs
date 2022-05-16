using MultipleBlazorApps.Shared.DTOs;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Client.Authentication
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);
        Task Logout();
        Task TryRenewToken();
    }
}
