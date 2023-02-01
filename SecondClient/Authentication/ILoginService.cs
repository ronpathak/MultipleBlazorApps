using MultipleBlazorApps.Shared.DTOs;
using System.Threading.Tasks;

namespace MultipleBlazorApps.SecondClient.Authentication
{
    public interface ILoginService
    {
        Task Login(UserToken userToken);
        Task Logout();
        Task TryRenewToken();
    }
}
