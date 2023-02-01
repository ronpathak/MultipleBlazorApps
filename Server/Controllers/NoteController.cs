using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleBlazorApps.Shared.Entities;


namespace MultipleBlazorApps.Server.Controllers
{
    //[Route("FirstApp/api/[controller]")]
    //[Route("FirstApp/[controller]")]
    //[Route("SecondApp/[controller]")]
    [Route("/consumer/[controller]")]
    [Route("/professional/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public NoteController(ApplicationDbContext context)
        {
            _context = context;

        }


        // GET: api/Note
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            Console.WriteLine("Note Controller received GetNotes request");
            return await _context.Note
                .ToListAsync();

        }


        // GET: api/Note/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var Person = await _context.Note
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (Person == null)
            {
                return NotFound();
            }
            else
            {
                return Person;
            }

        }

        // POST: api/Note
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note person)
        {

            _context.Note.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = person.Id }, person);
        }

        // DELETE: api/Note/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(int id)
        {
            var person = await _context.Note.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Note.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }

}
