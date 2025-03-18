using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyMangementApi.Services;
using SocietyManagementApi.Model;
using System;
using System.Data;

namespace SocietyMangementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(UserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("auth")]
        public IActionResult UserAuth([FromBody] UserLoginModel users)
        {
            try
            {
                // Use LoginUser instead of UserAuth
                DataTable user = _userRepository.LoginUser(users.UserName, users.Password, users.Role);

                if (user == null || user.Rows.Count == 0)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }


                var userRow = user.Rows[0];
                var authenticatedUser = new UserModel
                {
                    UserID = Convert.ToInt32(userRow["UserID"]),
                    UserName = userRow["UserName"].ToString(),
                    Password = userRow["Password"].ToString(),
                    Role = userRow["Role"].ToString()
                };

                // Generate JWT token
                var token = _jwtTokenService.GenerateJWTToken(authenticatedUser);

                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        authenticatedUser.UserID,
                        authenticatedUser.UserName,
                        authenticatedUser.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
