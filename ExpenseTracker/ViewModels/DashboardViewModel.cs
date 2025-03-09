using ExpenseTracker.Models;

namespace ExpenseTracker.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalExpenses { get; set; }
        public decimal MonthlyExpenses { get; set; }
        public decimal WeeklyExpenses { get; set; }
        public IEnumerable<Transaction> RecentTransactions { get; set; }
        public Dictionary<string, decimal> CategoryWiseExpenses { get; set; }
        public Dictionary<DateTime, decimal> DailyExpenses { get; set; }

        public DashboardViewModel()
        {
            RecentTransactions = new List<Transaction>();
            CategoryWiseExpenses = new Dictionary<string, decimal>();
            DailyExpenses = new Dictionary<DateTime, decimal>();
        }
    }
}
