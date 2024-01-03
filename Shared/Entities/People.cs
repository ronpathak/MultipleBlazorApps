using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MultipleBlazorApps.Shared.Entities
{
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string DocumentName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

    }
}
