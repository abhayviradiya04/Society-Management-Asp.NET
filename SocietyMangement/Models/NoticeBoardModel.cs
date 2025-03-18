using System;
using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class NoticeBoardModel
    {
        public int NoticeID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Posted By is required")]
        public int? PostedBy { get; set; }

        [Required(ErrorMessage = "Posting Date is required")]
        public DateTime PostingDate { get; set; } = DateTime.Now;

     

        [Required(ErrorMessage = "ExpirationDate Date is required")]
        public DateTime? ExpirationDate { get; set; }

        [Required(ErrorMessage = "Visibility is required")]
        [RegularExpression("^(All|Residents|Committee)$", ErrorMessage = "Visibility must be 'All', 'Residents', or 'Committee'")]
        public string? Visibility { get; set; } = "All";

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Active|Inactive)$", ErrorMessage = "Status must be 'Active' or 'Inactive'")]
        public string? Status { get; set; } = "Active";
    }
}
