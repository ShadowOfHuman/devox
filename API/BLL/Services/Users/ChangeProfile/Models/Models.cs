using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Users.ChangeProfile.Models
{
    public class InModel
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
