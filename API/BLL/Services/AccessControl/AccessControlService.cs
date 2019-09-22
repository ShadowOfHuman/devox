using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.DAL.Models;
using Registration = API.BLL.Services.AccessControl.Registration.Models;
using Authentication = API.BLL.Services.AccessControl.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using API.DAL.Context;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using API.Common;
using Microsoft.Extensions.Options;

namespace API.BLL.Services.AccessControl
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IDbContext _dbContext;

        public AccessControlService(IDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        public async Task<Authentication.Models.OutModel> Authentication(Authentication.Models.InModel inModel, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(inModel.UserName) || string.IsNullOrEmpty(inModel.PasswordHash))
            {
                return null;
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == inModel.UserName, cancellationToken);

            if (user == null)
            {
                return null;
            }

            if (!Cryptor.VerifyPasswordHash(inModel.PasswordHash, user.PasswordHash, user.Salt))
            {
                return null;
            }
            return new Authentication.Models.OutModel{ IdUser = user.Id, UserName = user.Username};
        }

        public async Task<Registration.Models.OutModel> Registration(Registration.Models.InModel inModel, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(inModel.UserName) || 
                inModel.PasswordHash == null || 
                string.IsNullOrEmpty(inModel.Email)){
                return null;
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == inModel.UserName || 
                x.Email == inModel.Email, cancellationToken);

            User newUser;
            if (user != null)
            {
                return null;
            }
            else
            {
                byte[] genSalt = Cryptor.GenerateSalt();
                newUser = new User
                {
                    Username = inModel.UserName,
                    Email = inModel.Email,
                    Salt = genSalt,
                    PasswordHash = Cryptor.CalculateHashOfPassword(inModel.PasswordHash, genSalt)
                    
                };

                _dbContext.Users.Add(newUser);

                await _dbContext.SaveChangesAsync(cancellationToken);
                return new Registration.Models.OutModel { IdUser = newUser.Id};
            }
        }
    }
}
