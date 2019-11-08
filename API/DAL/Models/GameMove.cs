using API.DAL.Models.Abstract;


namespace API.DAL.Models
{
    public class GameMove : BasicModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NumberMove { get; set; }
        public Game Game { get; set; }
        public User User { get; set; }

    }
}
