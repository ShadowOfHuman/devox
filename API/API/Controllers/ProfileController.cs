using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BLL.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using GetProfile = API.BLL.Services.Users.GetProfile.Models;
using ChangeProfile = API.BLL.Services.Users.ChangeProfile.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public ProfileController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        [Route("/{id}", Name = "userId")]
        public async Task<GetProfile.OutModel> GetProfile(int id)
        {
            return new GetProfile.OutModel();
        }

        /*[HttpPost]
        [Route("/update/{id}", Name = "userId")]
        public IActionResult ChangeProfiile([FromBody] ChangeProfile.InModel inModel)
        {
            return Ok();
        }*/


    }
}