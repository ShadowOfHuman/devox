using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Registration = API.BLL.Services.AccessControl.Registration.Models;
using Authentication = API.BLL.Services.AccessControl.Authentication.Models;
using GetProfile = API.BLL.Services.Users.GetProfile.Models;
using ChangeProfile = API.BLL.Services.Users.ChangeProfile.Models;
using ResetPassword = API.BLL.Services.Users.ResetPassword.Models;

using API.BLL.Services.AccessControl;
using API.BLL.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using API.BLL.Validators;
using FluentValidation;
using API.BLL.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccessControlService _iAccessControlService;
        private readonly IUserServices _userService;

        public UserController(IAccessControlService accessControlService, IUserServices userServices)
        {
            _iAccessControlService = accessControlService;
            _userService = userServices;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public async Task<Registration.OutModel> Registration([FromBody]Registration.InModel inModel)
        {
            RegistrationValidator validationRules = new RegistrationValidator();
            await validationRules.ValidateAndThrowAsync(inModel);
            return await _iAccessControlService.Registration(inModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authentication")]
        public async Task<Authentication.OutModel> Authentication([FromBody]Authentication.InModel inModel)
        {
            AuthValidator validationRules = new AuthValidator();
            await validationRules.ValidateAndThrowAsync(inModel);
            return await _iAccessControlService.Authentication(inModel);
        }

        [HttpPost]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> ChangeProfile([FromBody] ChangeProfile.InModel inModel)
        {
            ChangeProfileValidator validationRules = new ChangeProfileValidator();
            await validationRules.ValidateAndThrowAsync(inModel);
            await _userService.UpdateProfile(inModel.UserId, inModel.Email, inModel.Username);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("{userId:int}")]
        public async Task<GetProfile.OutModel> GetFullProfile(int userId)
        {
            return await _userService.GetFullProfile(userId);
        }

        [HttpPost]
        [Authorize]
        [Route("{ResetPassword}")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword.InModel inModel) {
            ResetPasswordValidator resetPasswordValidator = new ResetPasswordValidator();
            await resetPasswordValidator.ValidateAndThrowAsync(inModel);
            await _userService.ResetPasword(inModel.IdUser, inModel.NewPasswordHash, inModel.OldPasswordHash);
            return Ok();
        }


        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}