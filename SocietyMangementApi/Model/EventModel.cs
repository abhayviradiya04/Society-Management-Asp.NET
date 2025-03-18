using System.ComponentModel.DataAnnotations;

namespace SocietyManagementApi.Model
{
    public class EventModel
    {
        public int EventID { get; set; } // Unique ID for the event

       
        public string EventTitle { get; set; } = string.Empty; // Title of the event
        public string? Description { get; set; } // Detailed description (optional)
        public int OrganizerID { get; set; } // ID of the organizer
        public DateTime StartDateTime { get; set; } // Start date and time
        public DateTime EndDateTime { get; set; } // End date and time
        public string? Location { get; set; } // Location (optional)
        public string Status { get; set; } // Status with a default value

        public string? EventImage { get; set; } // URL of the event image
    }
}
