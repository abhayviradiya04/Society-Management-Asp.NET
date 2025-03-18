namespace SocietyManagementApi.Model
{
    public class NoticeBoardModel
    {
        public int NoticeID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostingDate { get; set; } = DateTime.Now;
        public DateTime? ExpirationDate { get; set; }
        public string? Visibility { get; set; } = "All";
        public string? Status { get; set; } = "Active";
    }
}
