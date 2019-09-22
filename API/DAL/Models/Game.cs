using API.DAL.Models.Abstract;

namespace API.DAL.Models
{
    public class Game : BasicModel
    {
        public User CreatedUser { get; set; }
        public User User { get; set; }
        public bool IsEndGame { get; set; }
        public long IdWinUser { get; set; }
        public string UrlHash { get; set; }
    }
}
