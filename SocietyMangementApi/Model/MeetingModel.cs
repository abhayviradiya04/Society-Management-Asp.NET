namespace SocietyManagementApi.Model
{
    public class MeetingModel
    {
        public int MeetingID { get; set; }             // Unique ID for each meeting
        public string MeetingTitle { get; set; }       // Title of the meeting
        public string Description { get; set; }       // Detailed description of the meeting
        public int OrganizerID { get; set; }           // UserID of the organizer (e.g., Admin or Resident)
        public DateTime StartDateTime { get; set; }    // Start date and time of the meeting
        public DateTime EndDateTime { get; set; }      // End date and time of the meeting
        public string? Location { get; set; }          // Optional: Location of the meeting
        public string Status { get; set; } = "Scheduled"; // Status (e.g., "Scheduled", "Completed", "Cancelled")
    }
}
