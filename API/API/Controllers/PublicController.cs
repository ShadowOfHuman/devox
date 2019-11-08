using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BLL.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using GetPublicProfile = API.BLL.Services.Users.GetPublicProfile.Models;
using ChangeProfile = API.BLL.Services.Users.ChangeProfile.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public PublicController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        [Route("Users/{userId}")]
        public async Task<GetPublicProfile.OutModel> GetProfile(int userId)
        {
            return await _userServices.GetPublicProfile(userId);
        }
    }
}