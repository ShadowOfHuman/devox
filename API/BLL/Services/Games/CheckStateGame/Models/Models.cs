using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Games.CheckStateGame.Models
{
    class InModel
    {
        public long IdGame { get; set; }
        public long IdCreatedUser { get; set; }
        public long IdUser { get; set; }
        public char[][] PlayingField { get; set; }
    }

    class OutModel
    {

    }
}
