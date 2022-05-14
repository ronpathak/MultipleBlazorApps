using MultipleBlazorApps.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Client.Repository
{
    public interface IPeopleRepository
    {
        Task CreatePeople(People person);
        Task DeletePeople(int Id);
        Task<People> GetPeople(int Id);
        Task<List<People>> GetPeoples();
    }
}