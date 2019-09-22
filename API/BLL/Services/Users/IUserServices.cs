using API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.BLL.Services.Users
{
    public interface IUserServices: ICRUDService<User>
    {
        void ResetPasword();
        Task UpdateProfile(User user);
    }
}
