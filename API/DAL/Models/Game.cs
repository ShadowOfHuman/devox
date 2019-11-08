using API.DAL.Models.Abstract;
using System.Collections.Generic;

namespace API.DAL.Models
{
    public class Game : BasicModel
    {
        public int  GameState{ get; set; }
        public long IdWinUser { get; set; }
        public string UrlHash { get; set; }
        public int SizeField { get; set; }
        public int CountItemForWin { get; set; }
        public User FirstUser { get; set; }
        public User TwoUser { get; set; }
        public string ConnectionIdUserFirst { get; set; }
        public string ConnectionIdUserSecond { get; set; }
        public IEnumerable<GameMove> GameMoves { get; set; }

    }
}
