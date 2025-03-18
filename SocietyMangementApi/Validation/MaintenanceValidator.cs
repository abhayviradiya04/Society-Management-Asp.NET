//using FluentValidation;
//using SocietyManagementApi.Model;

//namespace SocietyManagementApi.Validation
//{
//    public class MaintenanceValidator : AbstractValidator<MaintenanceModel>
//    {
//        public MaintenanceValidator()
//        {
//            RuleFor(m => m.FlatNumber)
//                .NotEmpty().WithMessage("Flat Number can't be empty")
//                .NotNull().WithMessage("Flat Number can't be null");

//            RuleFor(m => m.UserName)
//                .NotEmpty().WithMessage("User Name can't be empty")
//                .NotNull().WithMessage("User Name can't be null");

//            RuleFor(m => m.Amount)
//                .GreaterThan(0).WithMessage("Amount must be greater than 0");

//            RuleFor(m => m.DueDate)
//                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Due Date must be today or in the future");

//            RuleFor(m => m.PaymentStatus)
//                .Must(status => status == "Pending" || status == "Paid" || status == "Overdue")
//                .WithMessage("Payment Status must be either 'Pending', 'Paid', or 'Overdue'");

//            RuleFor(m => m.PaidDate)
//                .GreaterThanOrEqualTo(m => m.DueDate).WithMessage("Paid Date cannot be before the Due Date")
//                .When(m => m.PaymentStatus == "Paid");

//            RuleFor(m => m.Notes)
//                .MaximumLength(500).WithMessage("Notes can't be longer than 500 characters");

//            RuleFor(m => m.CreatedDate)
//                .NotEmpty().WithMessage("Created Date can't be empty")
//                .NotNull().WithMessage("Created Date can't be null");
//        }
//    }
//}
