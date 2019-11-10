using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Models
{
    public class Email
    {
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
