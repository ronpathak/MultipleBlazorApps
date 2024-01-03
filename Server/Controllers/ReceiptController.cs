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
    public class ReceiptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService fileStorageService;
        private readonly IReceiptReaderService receiptReaderService;


        public ReceiptController(ApplicationDbContext context, IFileStorageService fileStorageService, IReceiptReaderService receiptReaderService)
        {
            _context = context;
            this.fileStorageService = fileStorageService;
            this.receiptReaderService = receiptReaderService;
        }


        // GET: api/Receipt
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipts()
        {
            Console.WriteLine("Receipt Controller received GetReceipts request");
            return await _context.Receipts
                .ToListAsync();

        }


        // GET: api/Receipt/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int id)
        {
            var Receipt = await _context.Receipts
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (Receipt == null)
            {
                return NotFound();
            }
            else
            {
                return Receipt;
            }

        }

        // POST: api/Receipt
        [HttpPost]
        public async Task<ActionResult<Receipt>> PostReceipt(Receipt receipt)
        {

            if (!string.IsNullOrWhiteSpace(receipt.Attachment))
            {
                var receiptAttachment = Convert.FromBase64String(receipt.Attachment);
                if (System.IO.Path.GetExtension(receipt.AttachmentName) == ".pdf")
                {
                    receipt.Attachment = await fileStorageService.SaveFile(receiptAttachment,
                        ".pdf", "receipts");
                }
                else
                {
                    receipt.Attachment = await fileStorageService.SaveFile(receiptAttachment,
                        ".jpg", "receipts");
                }
            }


            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();

            //await receiptReaderService.AnalyseFile(receipt.Attachment);
            await AnalyseReceipt(receipt);


            return CreatedAtAction("GetReceipt", new { id = receipt.Id }, receipt);
        }

        // DELETE: api/Receipt/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Receipt>> DeleteReceipt(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            _context.Receipts.Remove(receipt);
            await _context.SaveChangesAsync();

            return receipt;
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipts.Any(e => e.Id == id);
        }



        public async Task AnalyseReceipt(Receipt receipt)
        {
            var receiptDB = await _context.Receipts
                .Where(x => x.Id == receipt.Id)
                .FirstOrDefaultAsync();

            var receiptdetail = await receiptReaderService.AnalyseFile(receipt.Attachment);
            
            receiptDB.Vendor = receiptdetail.Vendor;    
            receiptDB.Date =    receiptdetail.Date;
            receiptDB.Total = receiptdetail.Total;

            await _context.SaveChangesAsync();

        }
        


    }

}
