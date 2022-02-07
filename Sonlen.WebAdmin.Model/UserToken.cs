using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class UserToken
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
