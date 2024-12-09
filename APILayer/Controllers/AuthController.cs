using APILayer.Helpers;
using APILayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly GoogleService _googleService;

        public AuthController(IOptions<JwtSettings> jwtSettings, GoogleService googleService)
        {
            _jwtSettings = jwtSettings.Value;
            _googleService = googleService;
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] string googleToken)
        {
            try
            {
                var payload = await _googleService.ValidateGoogleToken(googleToken);
                var jwt = GenerateJwtToken(payload.Email);
                return Ok(new { Token = jwt });
            }
            catch (Exception)
            {
                return Unauthorized(new { Message = "Invalid Google Token" });
            }
        }

        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
