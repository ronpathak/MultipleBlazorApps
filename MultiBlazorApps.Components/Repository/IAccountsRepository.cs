using MultipleBlazorApps.Shared.DTOs;
using System.Threading.Tasks;

namespace MultiBlazorApps.Components.Repository
{
    public interface IAccountsRepository
    {
        Task ForgotPassword(ForgotPasswordDTO ForgotPasswordmodel);
        Task<UserToken> Login(UserInfoDTO userInfo);
        Task<UserToken> Register(UserInfoDTO userInfo);
        Task<UserToken> RenewToken();
        Task ResetPassword(ResetPasswordDTO ResetPasswordmodel);
    }
}