using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultipleBlazorApps.Shared.DTOs;
using MultipleBlazorApps.Shared.Entities;
using MultipleBlazorApps.Client.Helpers;


namespace MultipleBlazorApps.Client.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly IHttpService httpService;
        private readonly string baseURL = "accounts";

        public AccountsRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }


        public async Task<UserToken> Register(UserInfoDTO userInfo)
        {
            var httpResponse = await httpService.Post<UserInfoDTO, UserToken>($"{baseURL}/create", userInfo);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            return httpResponse.Response;
        }

        public async Task<UserToken> Login(UserInfoDTO userInfo)
        {
            var httpResponse = await httpService.Post<UserInfoDTO, UserToken>($"{baseURL}/login", userInfo);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            return httpResponse.Response;
        }

        public async Task<UserToken> RenewToken()
        {
            var response = await httpService.Get<UserToken>($"{baseURL}/RenewToken");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

        public async Task ForgotPassword(ForgotPasswordDTO ForgotPasswordmodel)
        {
            var httpResponse = await httpService.Post($"{baseURL}/Forgot", ForgotPasswordmodel);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            //return httpResponse.Response;
        }

        public async Task ResetPassword(ResetPasswordDTO ResetPasswordmodel)
        {
            var httpResponse = await httpService.Post($"{baseURL}/ResetPassword", ResetPasswordmodel);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            //return httpResponse.Response;
        }
    }

}
