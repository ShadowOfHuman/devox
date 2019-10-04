using API.DAL.Models.Abstract;

namespace API.DAL.Models
{
    public class Game : BasicModel
    {
        public User CreatedUser { get; set; }
        public User User { get; set; }
        public int  GameState{ get; set; }
        public long IdWinUser { get; set; }
        public string UrlHash { get; set; }
        public int[][] PlayingField { get; set; }
        public int LenghtField { get; set; }
        public int WidthField { get; set; }
        public int CountItemForWin { get; set; }
    }
}
