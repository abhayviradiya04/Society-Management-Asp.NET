using FluentValidation;
using SocietyManagementApi.Model;

namespace SocietyManagementApi.Validation
{
    public class MeetingValidator : AbstractValidator<MeetingModel>
    {
        public MeetingValidator()
        {
            RuleFor(m => m.MeetingTitle)
                .NotEmpty().WithMessage("Meeting Title can't be empty")
                .NotNull().WithMessage("Meeting Title can't be null");

            RuleFor(m=>m.Description)
                .NotEmpty().WithMessage("Meeting Description is Required")
                .NotNull().WithMessage("Meeting Description can't be null");

            RuleFor(m => m.OrganizerID)
                .GreaterThan(0).WithMessage("Organizer ID must be greater than 0");

            RuleFor(m => m.StartDateTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start Date and Time cannot be in the past");

            RuleFor(m => m.EndDateTime)
                .GreaterThan(m => m.StartDateTime).WithMessage("End Date and Time must be after Start Date and Time");

            RuleFor(m => m.Status)
                .Must(status => status == "Scheduled" || status == "Completed" || status == "Cancelled")
                .WithMessage("Status must be either 'Scheduled', 'Completed', or 'Cancelled'");

            RuleFor(m => m.Location)
                .NotEmpty().WithMessage("Location can't be empty")
                .When(m => m.StartDateTime < DateTime.Now); // Make location mandatory for past meetings if needed
        }
    }
}
