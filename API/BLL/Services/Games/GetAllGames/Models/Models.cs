using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Services.Games.GetAllGames.Models
{
    public class OutModel
    {
        public OutModel(long id, string title, int size)
        {
            Id = id;
            Title = title;
            Size = size;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public int Size { get; set; }
    }
}
