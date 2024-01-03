using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleBlazorApps.Shared.DTOs
{
    public class UploadFileImageDTO
    {
        public string ImageBase64 { get; set; }
        public string UploadedFileName { get; set; }

    }
}
