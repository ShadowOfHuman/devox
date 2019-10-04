using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Games.CreateGame.Models
{
    public class InModel
    {
        public long IdCreatedUser { get; set; }
    }
    public class OutModel
    {
        public long IdGame { get; set; }
        //TODO send url game
    }
}
