using API.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Games.MakeMove.Models
{
    public class InModel
    {
        public long IdGame { get; set; }
        public StateGame StateGame { get; set; }
        public int[][] PlayingField { get; set; }
        public Dictionary<int, long> Players { get; set; }
        public int CountItemForWin { get; set; }
    }

    public class OutModel
    {
        public long IdGame { get; set; }
        public StateGame StateGame { get; set; }
        public int[][] PlayingField { get; set; }
        public Dictionary<int, long> Players { get; set; }
        public int CountItemForWin { get; set; }
        public long IdWinUser { get; set; }
    }

}
