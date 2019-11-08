using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.BLL.Services.Users.GetProfile.Models;
using API.BLL.Services.Users.GetPublicProfile.Models;
using API.Common;
using API.DAL.Context;
using API.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace API.BLL.Services.Users
{
    public class UserService : IUserServices
    {
        private readonly IDbContext _dbContext;

        public UserService(IDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<long> Create(User item, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Add(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return item.Id;
        }
        public async Task Delete(long id, CancellationToken cancellationToken = default)
        {
            User user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            if(user != null)
            {
                _dbContext.Users.Remove(user);
            }
        }
        public async Task<User> Get(long id, CancellationToken cancellationToken = default)
        {
            User user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            return user;
        }
        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
            return users;
        }
        public async Task Update(User item, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ResetPasword(long userId, string newPasswordHash, string oldPasswordHash)
        {
            User user = await this.Get(userId);
            if(user.PasswordHash.SequenceEqual(Cryptor.CalculateHashOfPassword(oldPasswordHash)))
            {
                user.PasswordHash = Cryptor.CalculateHashOfPassword(newPasswordHash);
            }
            else
            {
                throw new InvalidOperationException("Incorrect current password.");
            }
            await this.Update(user);
        }

        public async Task UpdateProfile(long userId, string newEmail, string newUsername)
        {
            User user = await this.Get(userId);
            //add validator
            user.Email = newEmail;
            user.Username = newUsername;
            await this.Update(user);
        }

        public async Task<GetProfile.Models.OutModel> GetFullProfile(long userId)
        {
            User user = await Get(userId);
            return new GetProfile.Models.OutModel
            {
                CreateDate = user.CreateDate,
                Email = user.Email,
                Rating = user.Rating,
                Username = user.Username
            };
        }

        public async Task<GetPublicProfile.Models.OutModel> GetPublicProfile(long userId)
        {
            User user = await Get(userId);
            return new GetPublicProfile.Models.OutModel
            {
                CreateDate = user.CreateDate,
                Rating = user.Rating,
                Username = user.Username
            };
        }
    }
}
