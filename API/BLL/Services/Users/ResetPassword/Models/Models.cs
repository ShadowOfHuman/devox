using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Users.ResetPassword.Models
{
    public class InModel
    {
        public long IdUser { get; set; }
        public string OldPasswordHash { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
