using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterReq registerReq);
        Task<bool> ConfirmEmailAsync(int userId, string token);
        User Authenticate(string username, string password);
        void SaveRefreshToken(int userId, string refreshToken);
        RefreshTokens GetRefreshToken(int userId);
        RefreshTokens GetRefreshTokenByToken(string token);
        void MarkRefreshTokenAsUsed(RefreshTokens refreshToken);
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByUsernameAsync(string username);
        List<User> GetUsers();
        bool DeleteUserById(int userId);
        Task<User> GetOrCreateUserFromGoogleTokenAsync(string email);
        Task<bool> ForgotPassword(string mail);
        Task<bool> VerifyCode(string email, string code);
        Task<bool> ChangePass(string email, string newPass);
    }
}
