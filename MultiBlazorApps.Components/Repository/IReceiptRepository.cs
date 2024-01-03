using MultipleBlazorApps.Shared.Entities;

namespace MultiBlazorApps.Components.Repository
{
    public interface IReceiptRepository
    {
        Task CreateReceipt(Receipt person);
        Task DeleteReceipt(int Id);
        Task<Receipt> GetReceipt(int Id);
        Task<List<Receipt>> GetReceipts();
    }
}