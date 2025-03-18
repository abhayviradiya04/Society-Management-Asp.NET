using System;
using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class EventModel
    {
        public int EventID { get; set; } // Primary key

        [Required(ErrorMessage = "Event Title is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Event Title must be between 5 and 100 characters")]
        public string EventTitle { get; set; } = string.Empty; // NOT NULL

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Required(ErrorMessage = "Event Description is required")]
        public string Description { get; set; } // NULL

        [Required(ErrorMessage = "Organizer ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Organizer ID must be greater than 0")]
        public int OrganizerID { get; set; } // Foreign key to Users table

        [Required(ErrorMessage = "Start Date and Time is required")]
        [DataType(DataType.DateTime)]


        public DateTime? StartDateTime { get; set; } // NOT NULL

        [Required(ErrorMessage = "End Date and Time is required")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDateTime { get; set; } // NOT NULL

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Location must be between 5 and 200 characters")]
        [Required(ErrorMessage ="Location is required")]
        public string Location { get; set; } // Optional

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Active|Completed|Cancelled)$", ErrorMessage = "Status must be 'Active', 'Completed', or 'Cancelled'")]
        public string Status { get; set; } = "Active"; // Default to "Active"

       
        [Required(ErrorMessage = "Url is required")]
        public string? EventImage { get; set; } // Optional
    }
}
