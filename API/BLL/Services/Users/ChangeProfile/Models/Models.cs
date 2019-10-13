using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Users.ChangeProfile.Models
{
    public class InModel
    {
        public DateTime CreateDate { get; set; }
        public string Username { get; set; }
        public int Rating { get; set; }
    }
}
