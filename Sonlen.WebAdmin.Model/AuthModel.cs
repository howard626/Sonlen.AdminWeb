using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class AuthModel<T>
    {
        public T? Value { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
