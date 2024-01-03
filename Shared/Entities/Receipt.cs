using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MultipleBlazorApps.Shared.Entities
{

    public class Receipt
    {
        public int Id { get; set; }
        public string Attachment{ get; set; }
        public string AttachmentName { get; set; }
        public string Vendor { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public double Total { get; set; }

    }
}
