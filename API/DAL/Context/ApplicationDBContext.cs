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
    public class ApplicationDBContext : DbContext, IDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base(option) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameMove> GameMoves {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            modelBuilder.Entity<Game>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            modelBuilder.Entity<User>().HasMany(u => u.Games).WithOne(g => g.FirstUser);
            modelBuilder.Entity<Game>().HasMany(s => s.GameMoves).WithOne(s => s.Game);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var rowAffecteds = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return rowAffecteds;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}
