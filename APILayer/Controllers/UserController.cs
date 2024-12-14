using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("get-users")]
        public IActionResult GetAllUser()
        {
            List<User> users = (List<User>)_userService.GetUsers();
            if (users == null)
            {
                return Ok(new Response<List<User>>
                {
                    Success = false,
                    Message = "No users found.",
                    Data = null
                });
            }
            return Ok(new Response<List<User>>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = users
            });
        }

        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound(new Response<User>
                    {
                        Success = false,
                        Message = $"No user found with ID: {userId}.",
                        Data = null
                    });
                }

                return Ok(new Response<User>
                {
                    Success = true,
                    Message = "User retrieved successfully.",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("get-user-by-name")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);

                if (user == null)
                {
                    return NotFound(new Response<User>
                    {
                        Success = false,
                        Message = $"No user found with username {username}.",
                        Data = null
                    });
                }

                return Ok(new Response<User>
                {
                    Success = true,
                    Message = "User retrieved successfully.",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user.",
                    Data = ex.Message
                });
            }
        }
        
        [HttpDelete("delete-user")]
        public IActionResult DeleteUserById(int userId)
        {
            try
            {
                var result = _userService.DeleteUserById(userId);

                if (!result)
                {
                    return NotFound(new Response<bool>
                    {
                        Success = false,
                        Message = $"No user found with ID {userId}.",
                        Data = false
                    });
                }

                return Ok(new Response<bool>
                {
                    Success = true,
                    Message = "User deleted successfully.",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while deleting the user.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Email is required.",
                    Data = null
                });
            }

            try
            {
                // Call the service to handle forgot password logic
                var result = await _userService.ForgotPassword(email);

                if (!result)
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = "Email not found in the system.",
                        Data = null
                    });
                }

                return Ok(new Response<string>
                {
                    Success = true,
                    Message = "Password reset email sent successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the request.",
                    Data = ex.Message
                });
            }
        }
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Email and Code is required.",
                    Data = null
                });
            }

            try
            {
                var result = await _userService.VerifyCode(email, code);

                if (!result)
                {
                    return BadRequest(new Response<string>
                    {
                        Success = false,
                        Message = "Code is invalid or expired",
                        Data = null
                    });
                }

                return Ok(new Response<string>
                {
                    Success = true,
                    Message = "Verify code is successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the verfiy code.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string newPass)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPass))
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Email and Newpass is required.",
                    Data = null
                });
            }

            try
            {
                var result = await _userService.ChangePass(email, newPass);

                if (!result)
                {
                    return NotFound(new Response<string>
                    {
                        Success = false,
                        Message = "Not found email.",
                        Data = null
                    });
                }

                return Ok(new Response<string>
                {
                    Success = true,
                    Message = "Change pass is successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the change pass.",
                    Data = ex.Message
                });
            }
        }
    }
}
