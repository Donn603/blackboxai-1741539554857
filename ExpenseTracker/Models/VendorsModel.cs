using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class VendorsModel
    {

        [Key]
        public int VendorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public UsersModel User { get; set; }
        public ICollection<TransactionsModel> Transactions { get; set; }
    }
}