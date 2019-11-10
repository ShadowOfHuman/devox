using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Helpers
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public int SendEmailPeriodInSeconds { get; set; }
        public int SendEmailRetryCount { get; set; }
        public int SendEmailRetryDelayInSeconds { get; set; }
        public string UtcScheduleSendTo { get; set; }

    }
}

