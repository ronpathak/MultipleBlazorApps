using MultiBlazorApps.Components.Helpers;
using MultipleBlazorApps.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBlazorApps.Components.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IHttpService httpService;
        //private readonly string url = "api/SecondApp/people";
        private readonly string url = "receipt";

        public ReceiptRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }


        public async Task<List<Receipt>> GetReceipts()
        {
            var response = await httpService.Get<List<Receipt>>($"{url}/getall");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<Receipt> GetReceipt(int Id)
        {
            var response = await httpService.Get<Receipt>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }


        public async Task CreateReceipt(Receipt receipt)
        {
            var response = await httpService.Post(url, receipt);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }



        public async Task DeleteReceipt(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
