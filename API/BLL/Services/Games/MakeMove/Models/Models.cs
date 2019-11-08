using API.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Games.MakeMove.Models
{
    public class InModel
    {
        public long IdUser { get; set; }
        public long IdGame { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }

    public class OutModel
    {

        public int LastX { get; set; }
        public int LastY { get; set; }
    }

}
