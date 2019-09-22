using API.DAL.Models;
using System.Threading.Tasks;

namespace API.BLL.Services.Games
{
    public interface IGameServices : ICRUDService<Game>
    {
        Task<long> CreateGame(long IdUser);
        Task<Game> GetAllGameByUser(long IdUser);
    }
}
