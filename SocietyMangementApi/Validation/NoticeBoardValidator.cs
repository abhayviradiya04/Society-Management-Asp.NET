using FluentValidation;
using SocietyManagementApi.Model;

namespace SocietyManagementApi.Validation
{
    public class NoticeBoardValidator : AbstractValidator<NoticeBoardModel>
    {
        public NoticeBoardValidator()
        {
            RuleFor(n => n.Title)
                .NotEmpty().WithMessage("Title can't be empty")
                .NotNull().WithMessage("Title can't be null");

            RuleFor(n => n.PostedBy)
                .GreaterThan(0).WithMessage("PostedBy must be greater than 0");

            RuleFor(n => n.PostingDate)
                .NotEmpty().WithMessage("Posting Date can't be empty")
                .NotNull().WithMessage("Posting Date can't be null");

            RuleFor(n => n.ExpirationDate)
                .GreaterThanOrEqualTo(n => n.PostingDate).WithMessage("Expiration Date must be after Posting Date")
                .When(n => n.ExpirationDate.HasValue); // Only check if ExpirationDate is set

            RuleFor(n => n.Visibility)
                .Must(visibility => visibility == "All" || visibility == "Members" || visibility == "Admins")
                .WithMessage("Visibility must be either 'All', 'Members', or 'Admins'")
                .When(n => n.Visibility != null); 

            RuleFor(n => n.Status)
                .Must(status => status == "Active" || status == "Inactive")
                .WithMessage("Status must be either 'Active' or 'Inactive'");
        }
    }
}
