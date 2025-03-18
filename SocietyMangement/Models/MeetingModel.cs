using System;
using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class MeetingModel
    {
        public int MeetingID { get; set; }  // Unique ID for each meeting

        [Required(ErrorMessage = "Meeting Title is required")]
        [StringLength(100, ErrorMessage = "Meeting Title must be between 5 and 100 characters", MinimumLength = 5)]
        public string MeetingTitle { get; set; }  // Title of the meeting


        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Required(ErrorMessage = "Meeting Description is requiredsdfb")]
        public string Description { get; set; }  // Optional: Detailed description of the meeting

        [Required(ErrorMessage = "Organizer ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Organizer ID must be greater than 0")]
        public int OrganizerID { get; set; }  // UserID of the organizer (e.g., Admin or Resident)

        [Required(ErrorMessage = "Start Date and Time is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date & Time")]
        public DateTime? StartDateTime { get; set; }  // Start date and time of the meeting

        [Required(ErrorMessage = "End Date and Time is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date & Time")]
       // [Compare(nameof(StartDateTime), ErrorMessage = "End Date and Time must be after Start Date and Time")]
        public DateTime? EndDateTime { get; set; }  // End date and time of the meeting

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location must be between 5 and 200 characters", MinimumLength = 5)]
        public string? Location { get; set; }  // Location of the meeting

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Scheduled|Completed|Cancelled)$", ErrorMessage = "Status must be either 'Scheduled', 'Completed', or 'Cancelled'")]
        public string Status { get; set; } = "Scheduled";  // Status (e.g., "Scheduled", "Completed", "Cancelled")
    }
}
