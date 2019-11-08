using API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Registration = API.BLL.Services.AccessControl.Registration.Models;
using Authentication = API.BLL.Services.AccessControl.Authentication.Models;

namespace API.BLL.Services.AccessControl
{
    public interface IAccessControlService
    {
        Task<Authentication.Models.OutModel> Authentication(Authentication.Models.InModel inModel,
            string secret, CancellationToken cancellationToken = default);
        Task<Registration.Models.OutModel> Registration(Registration.Models.InModel inModel, CancellationToken cancellationToken = default);
    }
}
