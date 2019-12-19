using API.BLL.Models;
using API.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using GetAllGames = API.BLL.Services.Games.GetAllGames.Models;

namespace API.BLL.Services.Games
{
    public interface IGameServices : ICRUDService<Game>
    {
        Task<IEnumerable<GetAllGames.Models.OutModel>> GetAllPublicGames();
        Task<long> CreateGame(long IdUser, string title, int size);
        Task<IEnumerable<Game>> GetAllGameByUser(long IdUser);
        Task ConnectToGame(long idGame, long idUser);
        Task<StateGame> GetStateGame(long idGame);
        Task<StateGame> MakeMove(long userId, long gameId, int newX, int newY);
        Task SetGameNoFinished(long gameId);

    }
}
