using MultipleBlazorApps.Shared.Entities;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Server.Services
{
    public interface IReceiptReaderService
    {
        Task<Receipt> AnalyseFile(string FileURI);
    }
}