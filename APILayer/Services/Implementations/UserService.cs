using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace APILayer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IFirebaseService _firebaseService;
        private readonly ILogger<UserService> _logger;
        public UserService(ApplicationDbContext context, IEmailService emailService, IFirebaseService firebaseService, ILogger<UserService> logger)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _emailService = emailService;
            _firebaseService = firebaseService;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(RegisterReq registerReq)
        {
            var existingUsername = await _context.Users.SingleOrDefaultAsync(u => 
                                u.Username == registerReq.Username);

            if (existingUsername != null)
            {
                return false;
            }

            var existingEmail = await _context.Users.SingleOrDefaultAsync(u =>
                       u.Email == registerReq.Email);

            if (existingEmail != null)
            {
                return false;
            }

            var user = new User
            {
                Username = registerReq.Username,
                Email = registerReq.Email,
                Role = registerReq.Role,
                IsEmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
            };

            Console.WriteLine("User Email: " + user.Email);

            user.HashedPassword = _passwordHasher.HashPassword(user, registerReq.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var confirmationLink = $"https://apilayer-hvg5bbfkf5hteqc7.southeastasia-01.azurewebsites.net/api/Auth/confirm-email?userId={user.Id}&token={user.EmailConfirmationToken}";

            var emailBody = $@"
            <h2>Confirm your registration email</h2>
            <p>Hi, {user.Username}</p>
            <p>Please click the link below to confirm your account:</p>
            <a href='{confirmationLink}'>Confirm email</a>";

            await _emailService.SendEmailAsync(user.Email, "Xác nhận đăng ký tài khoản", emailBody);

            return true;
        }

        public async Task<bool> ForgotPassword(string mail)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == mail);
            if (user == null) return false;

            var random = new Random();
            var confirmationCode = random.Next(1000, 9999).ToString();

            user.EmailConfirmationToken = confirmationCode;

            await _context.SaveChangesAsync();

            var code = $"{confirmationCode}";

            var emailBody = $@"
            <h2>Change password</h2>
            <p>Hi, {user.Username}</p>
            <p>Here is the code, enter it on the verification page to proceed to the next step: <b>{code}</b></p>";

            await _emailService.SendEmailAsync(user.Email, "Change password", emailBody);

            return true;
        }

        public async Task<bool> VerifyCode(string email, string code)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.EmailConfirmationToken == code);
            if (user == null)
            {
                return false;
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePass(string email, string newPass)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            user.HashedPassword = _passwordHasher.HashPassword(user, newPass);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId && u.EmailConfirmationToken == token);
            if (user == null)
            {
                return false;
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            await _context.SaveChangesAsync();

            return true;
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
            if (result == PasswordVerificationResult.Success)
                return user;

            return null;
        }

        public void SaveRefreshToken(int userId, string refreshToken)
        {
            var token = new RefreshTokens
            {
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(token);
            _context.SaveChanges();
        }

        public RefreshTokens GetRefreshToken(int userId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && !rt.IsUsed)
                .OrderByDescending(rt => rt.ExpiryDate)
                .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public RefreshTokens GetRefreshTokenByToken(string token)
        {
            return _context.RefreshTokens.SingleOrDefault(rt => rt.Token == token);
        }

        public void MarkRefreshTokenAsUsed(RefreshTokens refreshToken)
        {
            refreshToken.IsUsed = true;
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChanges();
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.FAQs)
                    .Include(u => u.Reviews)
                    .Include(u => u.Payments)
                    .Include(u => u.UserSubscriptions)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with ID {userId}", ex);
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.FAQs)
                    .Include(u => u.Reviews)
                    .Include(u => u.Payments)
                    .Include(u => u.UserSubscriptions)
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with username {username}", ex);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                            .Include(u => u.FAQs)
                            .Include(u => u.Reviews)
                            .Include(u => u.Payments)
                            .Include(u => u.UserSubscriptions)
                            .FirstOrDefaultAsync(u => u.Username == username);

            //if (user != null && !string.IsNullOrEmpty(user.Avatar))
            //{
            //    user.Avatar = await _firebaseService.GetFileUrlAsync(user.Avatar);
            //}

            return user;
            //return await _context.Users.Include(u => u.FAQs)
            //                           .Include(u => u.Reviews)
            //                           .Include(u => u.Payments)
            //                           .Include(u => u.UserSubscriptions)
            //                           .FirstOrDefaultAsync(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return _context.Users.Include(u => u.FAQs)
                                       .Include(u => u.Reviews)
                                       .Include(u => u.Payments)
                                       .Include(u => u.UserSubscriptions).ToList();
        }

        public bool DeleteUserById(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public async Task<User> GetOrCreateUserFromGoogleTokenAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = email,
                    Role = "User",
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<bool> UpdateAvaUserByUsername(string username, IFormFile avatar)
        {
            try
            {
                _logger.LogInformation("Starting avatar update for user: {Username}", username);

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", username);
                    return false;
                }

                _logger.LogInformation("Current avatar URL: {CurrentAvatar}", user.Avatar);

                // Try to delete old avatar, but continue even if it fails
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    try
                    {
                        await _firebaseService.DeleteFileAsync(user.Avatar);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old avatar, continuing with update");
                    }
                }

                if (avatar != null)
                {
                    _logger.LogInformation("Uploading new avatar. File size: {Size}, Content type: {ContentType}",
                        avatar.Length, avatar.ContentType);
                    user.Avatar = await _firebaseService.UploadFileAsync(avatar, "avatars");
                    _logger.LogInformation("New avatar URL: {NewAvatar}", user.Avatar);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Avatar update completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating avatar for user {Username}", username);
                return false;
            }
        }

        public async Task<bool> UpdateUsername(string username, string newUsername, string coolInfoMyselft)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", username);
                    return false;
                }
                user.Username = newUsername;

                if (!string.IsNullOrEmpty(coolInfoMyselft))
                {
                    user.CoolInfoMySelft = coolInfoMyselft;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Username update completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating username");
                return false;
            }
        }

        public async Task<bool> ChangePassword(string username, string currentPassword, string newPassword)
        {
            try
            {
                // Retrieve the user by username
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", username);
                    return false;
                }

                // Verify the current password
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, currentPassword);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    _logger.LogWarning("Current password is incorrect for user: {Username}", username);
                    return false;
                }

                // Hash the new password
                user.HashedPassword = _passwordHasher.HashPassword(user, newPassword);

                // Save the changes
                await _context.SaveChangesAsync();
                _logger.LogInformation("Password updated successfully for user: {Username}", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user: {Username}", username);
                return false;
            }
        }

    }
}
