using MultipleBlazorApps.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Client.Repository
{
    public interface INoteRepository
    {
        Task CreateNote(Note note);
        Task DeleteNote(int Id);
        Task<Note> GetNote(int Id);
        Task<List<Note>> GetNotes();
    }
}