using API.BLL.Models;
using API.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.BLL.Services.Games
{
    public interface IGameServices : ICRUDService<Game>
    {
        Task<long> CreateGame(long IdUser);
        Task<IEnumerable<Game>> GetAllGameByUser(long IdUser);
        Task ConnectToGame(long idGame, long idUser);
        Task<StateGame> GetStateGame(long idGame);
        long CheckStateGame(int[][] plyingField, Dictionary<int, long> players, int countItemForWin);

    }
}
