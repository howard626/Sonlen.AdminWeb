using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class UploadFile
    {
        public string FileName { get; set; } = string.Empty;
        public byte[]? FileContent { get; set; }
    }
}
