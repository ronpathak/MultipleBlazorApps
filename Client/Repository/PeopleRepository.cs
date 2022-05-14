using MultipleBlazorApps.Client.Helpers;
using MultipleBlazorApps.Client.Repository;
using MultipleBlazorApps.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Client.Repository
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly IHttpService httpService;
        //private readonly string url = "api/FirstApp/people";
        //private readonly string url = "api/people";
        private readonly string url = "people";
        public PeopleRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }


        public async Task<List<People>> GetPeoples()
        {
            var response = await httpService.Get<List<People>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<People> GetPeople(int Id)
        {
            var response = await httpService.Get<People>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }


        public async Task CreatePeople(People person)
        {
            var response = await httpService.Post(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }



        public async Task DeletePeople(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
