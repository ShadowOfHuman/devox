using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Registration = API.BLL.Services.AccessControl.Registration.Models;
using Authentication = API.BLL.Services.AccessControl.Authentication.Models;
using API.BLL.Services.AccessControl;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccessControlService _iAccessControlService;

        public UserController(IAccessControlService accessControlService)
        {
            _iAccessControlService = accessControlService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<Registration.OutModel> Registration([FromBody]Registration.InModel inModel)
        {
            return await _iAccessControlService.Registration(inModel);
        }

        [HttpPost]
        [Route("authentication")]
        public async Task<Authentication.OutModel> Authentication([FromBody]Authentication.InModel inModel)
        {
            return await _iAccessControlService.Authentication(inModel);
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}