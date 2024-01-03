using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Shared.Entities
{
    public class Tenancy
    {
        public int Id { get; set; }
        public string Attachment { get; set; }
        public string AttachmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double RentAmount { get; set; }  


    }
}
