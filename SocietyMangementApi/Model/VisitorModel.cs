using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocietyMangementApi.Model
{
    public class VisitorModel
    {
       
        public int VisitorID { get; set; } 

     
        public string VisitorName { get; set; } 

   
        public string PhoneNumber { get; set; } 


        public string WhomToMeet { get; set; } 


        public string FlatType { get; set; } 


        public string FlatNumber { get; set; } 


        public string? VisitPurpose { get; set; } 


        public DateTime EntryTime { get; set; } = DateTime.Now; 

        public DateTime? ExitTime { get; set; } 


        public string? Status { get; set; } = "In";
    }

    public class FlatModel
    {
        public int FlatID { get; set; }          
        public string FlatNumber { get; set; }

    }

    public class VisitorStatsModel
    {
        public int TodayVisitors { get; set; }
        public int LastWeekVisitors { get; set; }
        public int LastTwoWeeksVisitors { get; set; }
    }


}
