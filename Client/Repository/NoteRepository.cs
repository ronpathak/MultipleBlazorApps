using MultiBlazorApps.Components.Helpers;
using MultipleBlazorApps.Client.Repository;
using MultipleBlazorApps.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Client.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly IHttpService httpService;
        //private readonly string url = "api/FirstApp/Note";
        //private readonly string url = "api/Note";
        private readonly string url = "note";
        public NoteRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }


        public async Task<List<Note>> GetNotes()
        {
            var response = await httpService.Get<List<Note>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<Note> GetNote(int Id)
        {
            var response = await httpService.Get<Note>($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }


        public async Task CreateNote(Note note)
        {
            var response = await httpService.Post(url, note);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }



        public async Task DeleteNote(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
