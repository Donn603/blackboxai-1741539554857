using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Expense_Tracker.Models
{
    public class TransactionsModel
    {

        [Key]
        public int TransactionId { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRecurring { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public VendorsModel Vendor { get; set; }
        public UsersModel User { get; set; }
        public ICollection<TransactionDetailsModel> TransactionDetails { get; set; }
    }
}