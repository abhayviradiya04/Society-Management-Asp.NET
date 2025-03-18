using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class MaintenanceModel
    {
        public int MaintenanceID { get; set; }
        public int FlatID { get; set; } 

        public string? FlatNumber { get; set; }
      
        public int UserID { get; set; }
        
        public string? UserName { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [Required]
        public string PaymentStatus { get; set; } = "Pending";
        [Required(ErrorMessage = "Paid Date is required.")]
        public DateTime? PaidDate { get; set; }

        public string Notes { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    public class GetFlatnumber
    {
        public int FlatID { get; set; }

        public string? FlatNumber { get; set; }
    }

    public class GetUserNameByFlatID
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }

}
