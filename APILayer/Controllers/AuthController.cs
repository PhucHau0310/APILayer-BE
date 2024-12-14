
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

            var result = await _userService.RegisterUserAsync(registerReq);
            if (!result)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Username is already taken.",
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

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle(string role = "Customer")
        {
            //var redirectUrl = Url.Action("GoogleResponse", "GoogleLogin", new { role });
            //var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            //return Challenge(properties, "Google");

            try
            {
                // Log the incoming request details
                Console.WriteLine($"Google Sign-In initiated with role: {role}");

                var redirectUrl = Url.Action("GoogleResponse", "Auth", new { role }, Request.Scheme);

                // Log the redirect URL
                Console.WriteLine($"Redirect URL: {redirectUrl}");

                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUrl,
                    // Add additional properties like saving the role
                    Items = { { "role", role } }
                };

                return Challenge(properties, "Google");
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Google Sign-In Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = $"Google Sign-In failed: {ex.Message}",
                    Data = null
                });
            }
        }

        [Authorize(AuthenticationSchemes = "Google")]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse(string state)
        {
            string role = state;
            var result = await HttpContext.AuthenticateAsync("Google");
            Console.WriteLine("suc: " + result.Succeeded);
            if (result.Succeeded)
            {
                var claims = result.Principal.Identities.First().Claims.ToList();
                var googleEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                // Kiểm tra tài khoản đã tồn tại chưa
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == googleEmail);
                if (user != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));

                    var accessToken = _authService.GenerateAccessToken(claims);
                    var refreshToken = _authService.GenerateRefreshToken();
                    _userService.SaveRefreshToken(user.Id, refreshToken);
                    return Ok(new Response<object>
                    {
                        Success = true,
                        Message = "Login successful",
                        Data = new
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        }
                    });
                }
                // Tạo tài khoản mới
                var regis = new RegisterReq
                {
                    Username = googleEmail,
                    Password = "123456789",
                    Email = googleEmail,
                    Role = role
                };

                var isSuccess = await _userService.RegisterUserAsync(regis);
                if (isSuccess)
                {
                    var userByGoogle = await _context.Users.FirstOrDefaultAsync(u => u.Username == regis.Username);

                    claims.Add(new Claim(ClaimTypes.Name, googleEmail));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userByGoogle.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, role));

                    var accessToken = _authService.GenerateAccessToken(claims);
                    var refreshToken = _authService.GenerateRefreshToken();

                    _userService.SaveRefreshToken(userByGoogle.Id, refreshToken);

                    return Ok(new Response<object>
                    {
                        Success = true,
                        Message = "Account created and login successful",
                        Data = new
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        }
                    });
                }

                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Unable to create user.",
                    Data = null
                });
            }
            var errorDetails = result.Failure?.Message;
            return Unauthorized($"Google authentication failed. {errorDetails}");
        }

        [HttpGet("signin-facebook")]
        public IActionResult SignInWithFacebook()
        {
            var redirectUrl = Url.Action("FacebookResponse", "FacebookLogin");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Facebook");
        }

        [HttpGet("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Facebook");
            if (result.Succeeded)
            {
                // Xử lý người dùng sau khi đăng nhập thành công
                var claims = result.Principal.Identities.First().Claims;
                _authService.GenerateAccessToken(claims);
            }

            return Redirect("/");
        }
    }
}
