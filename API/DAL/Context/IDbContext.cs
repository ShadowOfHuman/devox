using API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Context
{
    public interface IDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Game> Games { get; }
        DbSet<GameMove> GameMoves { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void EnsureCreated();
    }
}
