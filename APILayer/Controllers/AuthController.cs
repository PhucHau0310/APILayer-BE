
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using APILayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(IAuthService authService, IUserService userService, ApplicationDbContext context)
        {
            _authService = authService;
            _userService = userService;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReq registerReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Invalid data.",
                    Data = null
                });
            }

            var existingUsernameCheck = await _context.Users.AnyAsync(u => u.Username == registerReq.Username);
            if (existingUsernameCheck)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Username already taken.",
                    Data = null
                });
            }

            var existingEmailCheck = await _context.Users.AnyAsync(u => u.Email == registerReq.Email);
            if (existingEmailCheck)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Email already in use.",
                    Data = null
                });
            }

            var result = await _userService.RegisterUserAsync(registerReq);
            if (!result)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Registration failed.",
                    Data = null
                });
            }

            return Ok(new Response<string>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = null
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (!result)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Invalid confirmation link or token.",
                    Data = null
                });
            }

            return Ok(new Response<string>
            {
                Success = true,
                Message = "Email confirmed successfully.",
                Data = null
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq request)
        {
            TokenDto token = _authService.Login(request.Username, request.Password);
            if (token == null)
            {
                return Unauthorized(new Response<string>
                {
                    Success = false,
                    Message = "Invalid credentials.",
                    Data = null
                });
            }

            //User user = _userservice.getuserbyusername(request.Username);
            //string refreshtoken = _authservice.generaterefreshtoken();
            //_userservice.saverefreshtoken(user.id, refreshtoken);

            return Ok(new Response<object>
            {
                Success = true,
                Message = "Login successful",
                Data = new
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenDto request)
        {
            var tokenResponse = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (tokenResponse == null)
            {
                return Unauthorized(new Response<string>
                {
                    Success = false,
                    Message = "Invalid refresh token.",
                    Data = null
                });
            }

            return Ok(new Response<TokenRes>
            {
                Success = true,
                Message = "Token refreshed",
                Data = tokenResponse
            });
        }
    }
}
