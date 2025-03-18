using FluentValidation;
using SocietyManagementApi.Model;

namespace SocietyManagementApi.Validation
{
    public class EventValidator : AbstractValidator<EventModel>
    {
        public EventValidator()
        {
            RuleFor(e => e.EventTitle)
                .NotEmpty().WithMessage("Event Title can't be empty")
                .NotNull().WithMessage("Event Title can't be null");

            RuleFor(e => e.OrganizerID)
                .GreaterThan(0).WithMessage("Organizer ID must be greater than 0");

            RuleFor(e => e.StartDateTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start Date and Time cannot be in the past");

            RuleFor(e => e.EndDateTime)
                .GreaterThan(e => e.StartDateTime).WithMessage("End Date and Time must be after Start Date and Time");

            RuleFor(e => e.Status)
                .Must(status => status == "Active" || status == "Completed" || status=="Cancelled").WithMessage("Status must be either 'Active' or 'Inactive'");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Location can't be empty")
                .When(e => e.StartDateTime < DateTime.Now); // Make location mandatory only for past events if needed
        }
    }
}
