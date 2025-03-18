using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyManagementApi.Model;
using System.Data;
using System.Threading.Tasks;

namespace SocietyMangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]

        public IActionResult GetAllUsers()
        {
            List<UserModel> users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            UserModel user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost]
        public IActionResult InsertUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("User object cannot be null.");

            bool isInserted = _userRepository.InsertUser(user);
            if (isInserted)
                return Ok(new { Message = "User inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the user.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("User object cannot be null.");

            bool isUpdated = _userRepository.UpdateUser(user);
            if (isUpdated)
                return Ok(new { Message = "User updated successfully!" });

            return StatusCode(500, "An error occurred while updating the user.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = _userRepository.DeleteUser(id);
            if (isDeleted)
                return Ok(new { Message = "User deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the user.");
        }

        [HttpGet("flatnumber")]
        public IActionResult GetFlatNumberByFlatID(int? flatID)
        {
            var flatnumber = _userRepository.GetFlatNumber();
            return Ok(flatnumber);
        }




        [HttpPost("login")]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            if (ModelState.IsValid)
            {
                DataTable userTable = _userRepository.LoginUser(userLoginModel.UserName, userLoginModel.Password, userLoginModel.Role);

                if (userTable.Rows.Count > 0)
                {
                    var userRow = userTable.Rows[0];
                    var user = new
                    {
                        UserID = userRow["UserID"],
                        UserName = userRow["UserName"],
                        PassWord = userRow["PassWord"],
                        Role = userRow["Role"]
                    };

                    return Ok(new
                    {
                        Message = "Login successful",
                        User = user
                    });
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }

            return BadRequest("Invalid data.");
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DataTable resultTable = _userRepository.RegisterUser(
                        userRegisterModel.UserName,
                        userRegisterModel.Password,
                        userRegisterModel.Email,
                        userRegisterModel.Contact_No,
                        userRegisterModel.Role
                    );

                    if (resultTable.Rows.Count > 0)
                    {
                        var resultRow = resultTable.Rows[0];
                        var resultMessage = resultRow["Message"].ToString();

                        if (resultMessage == "User registered successfully")
                        {
                            return Ok(new { Message = resultMessage });
                        }
                        else
                        {
                            return Conflict(new { Message = resultMessage });
                        }
                    }
                    else
                    {
                        return StatusCode(500, "User registration failed. No response from the database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return StatusCode(500, "An unexpected error occurred: " + ex.Message);
                }
            }

            return BadRequest("Invalid data.");
        }

        [HttpGet("Top3")]
        public IActionResult GetTop3Users()
        {
            List<UserModel> users = _userRepository.GetTop3Users();
            return Ok(users);
        }

        [HttpGet("Count")]
        public IActionResult GetUserCount()
        {
            int count = _userRepository.GetUserCount();
            return Ok(count);
        }

    }
}
