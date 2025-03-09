using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class TransactionDetailsModel
    {

        [Key]
        public int TransactionDetailId { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public TransactionsModel Transaction { get; set; }
        public CategoriesModel Category { get; set; }
    }
}
