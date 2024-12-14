using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using Google.Apis.Auth.OAuth2.Responses;
using System.Security.Claims;

namespace APILayer.Services.Interfaces
{
    public interface IAuthService
    {
        TokenDto Login(string username, string password);
        Task<TokenRes> RefreshTokenAsync(string token);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResponse> ExchangeCodeForTokensAsync(string code);
    }
}
