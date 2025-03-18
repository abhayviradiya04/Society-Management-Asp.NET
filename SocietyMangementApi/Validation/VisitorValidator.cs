//using FluentValidation;
//using SocietyMangementApi.Model;
//using System.Text.RegularExpressions;

//namespace SocietyMangementApi.Validation
//{
//    public class VisitorValidator : AbstractValidator<VisitorModel>
//    {
//        public VisitorValidator()
//        {
//            RuleFor(v => v.VisitorName)
//                .NotEmpty().WithMessage("Visitor Name can't be empty")
//                .NotNull().WithMessage("Visitor Name can't be null");

//            RuleFor(v => v.PhoneNumber)
//                .NotEmpty().WithMessage("Phone Number can't be empty")
//                .NotNull().WithMessage("Phone Number can't be null")
//                .Matches(@"^\d{10}$").WithMessage("Phone Number must be a 10-digit number");

//            RuleFor(v => v.WhomToMeet)
//                .NotEmpty().WithMessage("Whom to Meet can't be empty")
//                .NotNull().WithMessage("Whom to Meet can't be null");

//            RuleFor(v => v.FlatType)
//                .NotEmpty().WithMessage("Flat Type can't be empty")
//                .NotNull().WithMessage("Flat Type can't be null");

//            RuleFor(v => v.FlatNumber)
//                .NotEmpty().WithMessage("Flat Number can't be empty")
//                .NotNull().WithMessage("Flat Number can't be null");

//            RuleFor(v => v.EntryTime)
//                .NotEmpty().WithMessage("Entry Time can't be empty")
//                .NotNull().WithMessage("Entry Time can't be null");

//            RuleFor(v => v.ExitTime)
//                .GreaterThan(v => v.EntryTime).WithMessage("Exit Time must be after Entry Time")
//                .When(v => v.ExitTime.HasValue); // Only check if ExitTime is provided

//            RuleFor(v => v.Status)
//                .Must(status => status == "In" || status == "Out")
//                .WithMessage("Status must be either 'In' or 'Out'");
//        }
//    }
//}
