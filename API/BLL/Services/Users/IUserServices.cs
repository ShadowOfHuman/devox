using API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GetProfile = API.BLL.Services.Users.GetProfile;
using GetPublicProfile = API.BLL.Services.Users.GetPublicProfile;

namespace API.BLL.Services.Users
{
    public interface IUserServices: ICRUDService<User>
    {
        Task ResetPasword(long userId, string newPasswordHash, string oldPasswordHash);
        Task UpdateProfile(long userId, string newEmail, string newUsername);
        Task<GetProfile.Models.OutModel> GetFullProfile(long userId);
        Task<GetPublicProfile.Models.OutModel> GetPublicProfile(long userId);
    }
}
