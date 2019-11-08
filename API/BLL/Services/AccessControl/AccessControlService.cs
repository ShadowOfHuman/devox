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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.BLL.Services.AccessControl
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IDbContext _dbContext;

        public AccessControlService(IDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        public async Task<Authentication.Models.OutModel> Authentication(Authentication.Models.InModel inModel, 
            string secret,
            CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(inModel.Email) || string.IsNullOrEmpty(inModel.PasswordHash))
            {
                throw new ArgumentNullException("Username, password or email is empty.");
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == inModel.Email, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User with email not exist.");
            }
            if (!Cryptor.VerifyPasswordHash(inModel.PasswordHash, user.PasswordHash))
            {
                throw new InvalidOperationException("Email or password is incorrect.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return new Authentication.Models.OutModel{ IdUser = user.Id, 
                UserName = user.Username, Token=user.Token};
        }

        public async Task<Registration.Models.OutModel> Registration(Registration.Models.InModel inModel, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(inModel.UserName) || inModel.PasswordHash == null || string.IsNullOrEmpty(inModel.Email)){
                throw new ArgumentNullException("Username, password or email is empty.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == inModel.UserName || 
                x.Email == inModel.Email, cancellationToken);

            User newUser;
            if (user != null)
            {
                throw new InvalidOperationException($"User with {user.Email} or {user.Username} already exist.");
            }

            byte[] genSalt = Cryptor.GenerateSalt();
            newUser = new User
            {
                Username = inModel.UserName,
                Email = inModel.Email,
                PasswordHash = Cryptor.CalculateHashOfPassword(inModel.PasswordHash)

            };
            _dbContext.Users.Add(newUser);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Registration.Models.OutModel { IdUser = newUser.Id };
        }
    }
}
