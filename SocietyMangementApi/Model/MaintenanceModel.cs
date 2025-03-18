namespace SocietyManagementApi.Model
{
    public class MaintenanceModel
    {
        public int MaintenanceID { get; set; } // Unique ID for maintenance records
        public int FlatID { get; set; } // Foreign key to Flats table

        public string? FlatNumber { get; set; }
        public int UserID { get; set; } // Optional: Foreign key to Users table

        public string? UserName { get; set; }
        public decimal Amount { get; set; } // Maintenance amount
        public DateTime DueDate { get; set; } // Due date for payment
        public string PaymentStatus { get; set; } = "Pending"; // Payment status (e.g., "Pending", "Paid", "Overdue")
        public DateTime? PaidDate { get; set; } // Optional: Date of payment (if paid)
        public string Notes { get; set; } // Optional: Notes or remarks
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Record creation date
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
