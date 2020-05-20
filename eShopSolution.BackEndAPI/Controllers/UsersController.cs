using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace eShopSolution.BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private static IHttpContextAccessor _httpContextAccessor;
        public UsersController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultToken = await _userService.Authencate(request);
            if(string.IsNullOrEmpty(resultToken.ResultObject))
            {
                return BadRequest(resultToken);
            }
            return Ok(resultToken);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Register(request);
            if (result.IsSuccessed==false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(VerificationViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.ConfirmEmail(request);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https:localhost:5002/api/users/getList?pageIndex=&pageSize=&Keyword=
        [HttpGet("getListUser")]
        public async Task<IActionResult> GetListUser([FromQuery] GetUserPaggingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.GetListUser(request);
            return Ok(result);
        }

        [HttpGet("getListRole")]
        public async Task<IActionResult> GetListRole()
        {
            var result = await _userService.GetListRole();
            return Ok(result);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            var result = await _userService.Delete(userId);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var result = await _userService.GetById(userId);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("getByEmail/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByEmail( string email)
        {
            var result = await _userService.GetByEmail(email);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("getByUserName/{userName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUserName( string userName)
        {
            var result = await _userService.GetByUserName(userName);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("{userId}")]
        public async Task<IActionResult> Update([FromRoute] Guid userId,[FromForm] UserUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(userId,request);
            if (result.IsSuccessed == false) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("ExternalSignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoExternalAsync(ExternalLoginRequest request)
        {
            var result = await _userService.ExternalLoginCallback(request);
            if (string.IsNullOrEmpty(result.ResultObject))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("getPasswordResetToken/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPasswordResetToken(string email)
        {
            var result = await _userService.GetPasswordResetToken(email);
            if (string.IsNullOrEmpty(result.ResultObject))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var result = await _userService.ResetPassword(request);
            if (result.IsSuccessed==false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {
            var result = await _userService.ChangePassword(request);
            if (result.IsSuccessed == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}