using FluentValidation;
using SocietyManagementApi.Model;
using System.Text.RegularExpressions;

namespace SocietyManagementApi.Validation
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("User Name can't be empty")
                .NotNull().WithMessage("User Name can't be null");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .NotNull().WithMessage("Email can't be null")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(BeUniqueEmail).WithMessage("Email must be unique");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .NotNull().WithMessage("Password can't be null")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number can't be empty")
                .NotNull().WithMessage("Phone Number can't be null")
                .Matches(@"^\d{10}$").WithMessage("Phone Number must be a 10-digit number")
                .Must(BeUniquePhoneNumber).WithMessage("Phone Number must be unique");

            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Role can't be empty")
                .NotNull().WithMessage("Role can't be null")
                .Must(role => role == "Admin" || role == "Resident" || role == "Security")
                .WithMessage("Role must be either 'Admin', 'Resident', or 'Security'");

            RuleFor(u => u.Status)
                .Must(status => status == "Active" || status == "Inactive" || status == "Banned")
                .WithMessage("Status must be either 'Active', 'Inactive', or 'Banned'");

            RuleFor(u => u.CreatedDate)
                .NotEmpty().WithMessage("Created Date can't be empty")
                .NotNull().WithMessage("Created Date can't be null");
        }

        // Custom method to check if the email is unique
        private bool BeUniqueEmail(string email)
        {
            // Logic to check if the email is unique (e.g., query the database)
            return true; // Placeholder: replace with actual check
        }

        // Custom method to check if the phone number is unique
        private bool BeUniquePhoneNumber(string phoneNumber)
        {
            // Logic to check if the phone number is unique (e.g., query the database)
            return true; // Placeholder: replace with actual check
        }
    }
}
