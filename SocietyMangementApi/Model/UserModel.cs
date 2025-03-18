using System.ComponentModel.DataAnnotations;

namespace SocietyManagementApi.Model
{
    public class UserModel
    {
        public int UserID { get; set; } // Primary key
        public string? UserName { get; set; }  // NOT NULL
        public string? Email { get; set; }  // UNIQUE NOT NULL
        public string? Password { get; set; } // NOT NULL
        public string? PhoneNumber { get; set; }  // UNIQUE NOT NULL
        public string? Role { get; set; }  // Admin, Resident, Security

        public int FlatID { get; set; }
        public string? FlatNumber { get; set; } // Foreign key to Flats table
        public string Status { get; set; }  // Active, Inactive, Banned
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Default GETDATE()
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string Role { get; set; }
    }


    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        public string Contact_No { get; set; }

        public string Role { get; set; }


    }
}
