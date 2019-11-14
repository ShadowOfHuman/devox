using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BLL.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using GetPublicProfile = API.BLL.Services.Users.GetPublicProfile.Models;
using ChangeProfile = API.BLL.Services.Users.ChangeProfile.Models;
using API.BLL.Services.AccessControl;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IAccessControlService _accessService;

        public PublicController(IUserServices userServices, IAccessControlService accessControlService)
        {
            _userServices = userServices;
            _accessService = accessControlService;
        }
        [HttpGet]
        [Route("Users/{userId}")]
        public async Task<GetPublicProfile.OutModel> GetProfile(int userId)
        {
            return await _userServices.GetPublicProfile(userId);
        }

        [HttpPost]
        [Route("resetPassword")]
        public async Task<ActionResult> ResetPassword([FromQuery] string email)
        {
            await _accessService.ResetPassword(email);
            return Ok();
        }
    }
}