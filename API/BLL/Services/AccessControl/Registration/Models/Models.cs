using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.AccessControl.Registration.Models
{
    public class InModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class OutModel
    {
        public long IdUser { get; set; }
    }
}
