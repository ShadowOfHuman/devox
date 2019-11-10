using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BLL.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string PolicyURL { get; set; }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
