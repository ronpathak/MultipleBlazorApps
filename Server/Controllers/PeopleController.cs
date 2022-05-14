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
    [Route("/FirstApp/[controller]")]
    [Route("/SecondApp/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public PeopleController(ApplicationDbContext context)
        {
            _context = context;

        }


        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<People>>> GetPeoples()
        {
            Console.WriteLine("People Controller received GetPeoples request");
            return await _context.People
                .ToListAsync();

        }


        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<People>> GetPeople(int id)
        {
            var Person = await _context.People
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

        // POST: api/People
        [HttpPost]
        public async Task<ActionResult<People>> PostPeople(People person)
        {

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPeople", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<People>> DeletePeople(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }

}
