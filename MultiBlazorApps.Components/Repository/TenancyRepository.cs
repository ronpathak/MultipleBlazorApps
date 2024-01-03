using MultiBlazorApps.Components.Helpers;
using MultipleBlazorApps.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBlazorApps.Components.Repository
{
    public class TenancyRepository : ITenancyRepository
    {
        private readonly IHttpService httpService;
        //private readonly string url = "api/SecondApp/people";
        private readonly string url = "tenancy";

        public TenancyRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }


        public async Task<List<Tenancy>> GetTenancys()
        {
            var response = await httpService.Get<List<Tenancy>>($"{url}/getall");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<Tenancy> GetTenancy(int Id)
        {
            var response = await httpService.Get<Tenancy>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }


        public async Task CreateTenancy(Tenancy tenancy)
        {
            var response = await httpService.Post(url, tenancy);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }



        public async Task DeleteTenancy(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
