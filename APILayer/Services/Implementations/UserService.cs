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
        public UserService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _emailService = emailService;
        }

        public async Task<bool> RegisterUserAsync(RegisterReq registerReq)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => 
                                u.Username == registerReq.Username
                                && u.Email == registerReq.Email);

            if (existingUser != null)
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

            var confirmationLink = $"https://localhost:7036/api/Auth/confirm-email?userId={user.Id}&token={user.EmailConfirmationToken}";

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
            return await _context.Users.Include(u => u.FAQs)
                                       .Include(u => u.Reviews)
                                       .Include(u => u.Payments)
                                       .Include(u => u.UserSubscriptions)
                                       .FirstOrDefaultAsync(u => u.Username == username);
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
    }
}
