using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelDataAccess.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }

        [Required]
        public int TripID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // "Pending", "Confirmed", "Cancelled"

        // Navigation properties
        [ForeignKey("TripID")]
        public virtual Trip? Trip { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer? Customer { get; set; }
    }
}