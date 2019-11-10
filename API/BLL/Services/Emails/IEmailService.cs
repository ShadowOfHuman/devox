using API.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.BLL.Services.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}
