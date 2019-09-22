using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.AccessControl.Authentication.Models
{
    public class InModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class OutModel
    {
        public long IdUser { get; set; }
        public string UserName { get; set; }
    }
}
