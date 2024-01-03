using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleBlazorApps.Server.Helpers;
using MultipleBlazorApps.Server.Migrations;
using MultipleBlazorApps.Server.Services;
using MultipleBlazorApps.Shared.Entities;


namespace MultipleBlazorApps.Server.Controllers
{
    //[Route("FirstApp/api/[controller]")]
    //[Route("FirstApp/[controller]")]
    //[Route("SecondApp/[controller]")]
    [Route("/consumer/[controller]")]
    [Route("/professional/[controller]")]
    [ApiController]
    public class TenancyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService fileStorageService;
        private readonly IReceiptReaderService receiptReaderService;


        public TenancyController(ApplicationDbContext context, IFileStorageService fileStorageService, IReceiptReaderService receiptReaderService)
        {
            _context = context;
            this.fileStorageService = fileStorageService;
            this.receiptReaderService = receiptReaderService;
        }


        // GET: api/Tenancy
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Tenancy>>> GetTenancys()
        {
            Console.WriteLine("Tenancy Controller received GetTenancys request");
            return await _context.Tenancies
                .ToListAsync();

        }


        // GET: api/Tenancy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenancy>> GetTenancy(int id)
        {
            var Tenancy = await _context.Tenancies
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (Tenancy == null)
            {
                return NotFound();
            }
            else
            {
                return Tenancy;
            }

        }

        // POST: api/Tenancy
        [HttpPost]
        public async Task<ActionResult<Tenancy>> PostTenancy(Tenancy tenancy)
        {

            if (!string.IsNullOrWhiteSpace(tenancy.Attachment))
            {
                var tenancyAttachment = Convert.FromBase64String(tenancy.Attachment);
                if (System.IO.Path.GetExtension(tenancy.AttachmentName) == ".pdf")
                {
                    tenancy.Attachment = await fileStorageService.SaveFile(tenancyAttachment,
                        ".pdf", "tenancys");
                }
                else
                {
                    tenancy.Attachment = await fileStorageService.SaveFile(tenancyAttachment,
                        ".jpg", "tenancys");
                }
            }


            _context.Tenancies.Add(tenancy);
            await _context.SaveChangesAsync();

            //await tenancyReaderService.AnalyseFile(tenancy.Attachment);
            await AnalyseTenancy(tenancy);


            return CreatedAtAction("GetTenancy", new { id = tenancy.Id }, tenancy);
        }

        // DELETE: api/Tenancy/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tenancy>> DeleteTenancy(int id)
        {
            var tenancy = await _context.Tenancies.FindAsync(id);
            if (tenancy == null)
            {
                return NotFound();
            }

            _context.Tenancies.Remove(tenancy);
            await _context.SaveChangesAsync();

            return tenancy;
        }

        private bool TenancyExists(int id)
        {
            return _context.Tenancies.Any(e => e.Id == id);
        }



        public async Task AnalyseTenancy(Tenancy tenancy)
        {
            //var tenancyDB = await _context.Tenancys
            //    .Where(x => x.Id == tenancy.Id)
            //    .FirstOrDefaultAsync();

            //var tenancydetail = await receiptReaderService.AnalyseFile(tenancy.Attachment);
            
            //tenancyDB.Vendor = tenancydetail.Vendor;    
            //tenancyDB.Date =    tenancydetail.Date;
            //tenancyDB.Total = tenancydetail.Total;

            //await _context.SaveChangesAsync();

        }
        


    }

}
