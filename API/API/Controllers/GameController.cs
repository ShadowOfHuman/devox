using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using API.BLL.Validators;
using FluentValidation;
using API.BLL.Helpers;
using API.BLL.Services.Games;

using GameModel = API.BLL.Services.Games;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameServices gameService;

        public GameController(IGameServices gameServices)
        {
            this.gameService = gameService;
        }

        
        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<GameModel.CreateGame.Models.OutModel> CreateGame([FromBody] GameModel.CreateGame.Models.InModel inModel) {
            GameModel.CreateGame.Models.OutModel outModel = new GameModel.CreateGame.Models.OutModel();
            outModel.IdGame =  await gameService.CreateGame(inModel.IdCreatedUser, inModel.Title, inModel.Size);
            return outModel;
        }


        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}