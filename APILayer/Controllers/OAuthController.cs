using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Services.Implementations;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APILayer.Controllers
{
    [Route("")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public OAuthController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return BadRequest("Google login failed.");
            }

            // Xử lý thông tin người dùng ở đây
            var userInfo = result.Principal;

            // Trả về thông tin người dùng hoặc token
            return Ok(new
            {
                Email = userInfo.FindFirst("email")?.Value,
                Name = userInfo.FindFirst("name")?.Value
            });
        }

        [HttpGet("signin-facebook")]
        public IActionResult LoginWithFacebook()
        {
            var redirectUrl = Url.Action(nameof(HandleFacebookCallback));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook-response")]
        public async Task<IActionResult> HandleFacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return BadRequest("Facebook authentication failed.");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == "email")?.Value;

            return Ok(new { Message = "Facebook authentication successful", Email = email });
        }
    }
}
