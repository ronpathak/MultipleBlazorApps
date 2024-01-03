using MultipleBlazorApps.Shared.Entities;

namespace MultiBlazorApps.Components.Repository
{
    public interface ITenancyRepository
    {
        Task CreateTenancy(Tenancy tenancy);
        Task DeleteTenancy(int Id);
        Task<Tenancy> GetTenancy(int Id);
        Task<List<Tenancy>> GetTenancys();
    }
}