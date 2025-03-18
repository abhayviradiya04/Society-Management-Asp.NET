namespace SocietyMangement.Models
{
    public class HomeModel
    {
        public List<EventModel> Events { get; set; }
        public int TotalEvents { get; set; }

        public List<FlatModel> Flats { get; set; }
        public int TotalFlats { get; set; }

        public List<MaintenanceModel> MaintenanceRecords { get; set; }
        public int TotalMaintenance { get; set; }

        public List<MeetingModel> Meetings { get; set; }
        public int TotalMeetings { get; set; }

        public List<NoticeBoardModel> Notices { get; set; }
        public int TotalNotices { get; set; }

        public List<VisitorModel> Visitors { get; set; }
        public int TotalVisitors { get; set; }

        public List<UserModel> Users { get; set; }
        public int TotalUsers { get; set; }

        public VisitorStatsModel VisitorStats { get; set; }
    }


}

