using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.DAL.Context;
using API.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace API.BLL.Services.GameMoves
{
    public class GameMoveService : IGameMoveService
    {
        private readonly IDbContext _dbContext;
        public GameMoveService(IDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        async public Task<long> Create(GameMove item, CancellationToken cancellationToken = default)
        {
            await _dbContext.GameMoves.AddAsync(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return item.Id;
        }

        async public Task Delete(long id, CancellationToken cancellationToken = default)
        {
            var gameMove = await this.Get(id);
            _dbContext.GameMoves.Remove(gameMove);
            await _dbContext.SaveChangesAsync();
        }

        async public Task<GameMove> Get(long id, CancellationToken cancellationToken = default)
        {
            var gameMove = await _dbContext.GameMoves.FindAsync(id);
            if (gameMove == null)
            {
                throw new InvalidOperationException("Game not found.");
            }
            return gameMove;
        }

        async public Task<IEnumerable<GameMove>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.GameMoves.ToListAsync(cancellationToken);
        }

        async public Task Update(GameMove item, CancellationToken cancellationToken = default)
        {
            _dbContext.GameMoves.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
