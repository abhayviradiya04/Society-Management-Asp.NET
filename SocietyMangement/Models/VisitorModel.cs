using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class VisitorModel
    {
        public int VisitorID { get; set; }
        public string VisitorName { get; set; }
        public string PhoneNumber { get; set; }
        public string WhomToMeet { get; set; }
        [Required(ErrorMessage = "Flat Type is required.")]
        public string FlatType { get; set; }
        public string FlatNumber { get; set; } 
        public string VisitPurpose { get; set; }
        public DateTime EntryTime { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Exit Time is required.")]
        public DateTime? ExitTime { get; set; } // Nullable DateTime

        public string? Status { get; set; } = "In";
    }
    public class FlatByVisitorModel
    {
        public int FlatID { get; set; }
        public string? FlatNumber { get; set; }

    }

    public class VisitorStatsModel
    {
        public int TodayVisitors { get; set; }
        public int LastWeekVisitors { get; set; }
        public int LastTwoWeeksVisitors { get; set; }
    }

}
