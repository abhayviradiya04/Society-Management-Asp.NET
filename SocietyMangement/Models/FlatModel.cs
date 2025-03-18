using System.ComponentModel.DataAnnotations;

namespace SocietyMangement.Models
{
    public class FlatModel
    {
        public int FlatID { get; set; }
        [Required]
        public string? FlatNumber { get; set; }
        [Required]
        public int? FlatTypeID { get; set; }
        [Required]
        public int? FloorNumber { get; set; }
        [Required]
        public string? Block { get; set; }
    }
}
