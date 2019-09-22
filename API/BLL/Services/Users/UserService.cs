using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public void ResetPasword()
        {
            throw new NotImplementedException();
        }

        public async Task Update(User item, CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateProfile(User user)
        {
            throw new NotImplementedException();
        }

    }
}
