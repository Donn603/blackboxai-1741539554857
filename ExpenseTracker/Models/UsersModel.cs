using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Expense_Tracker.Models
{
    public class UsersModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TransactionsModel> Transactions { get; set; }
        public ICollection<CategoriesModel> Categories { get; set; }
        public ICollection<VendorsModel> Vendors { get; set; }
    }
}
