using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Users.ResetPassword.Models
{
    class InModel
    {
        public long IdUser { get; set; }
        public string NewPassword { get; set; }
    }
}
