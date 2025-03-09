using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Database;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpenseTrackerContext _context;

        public HomeController(ILogger<HomeController> logger, ExpenseTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new DashboardViewModel();

            try
            {
                // Get all transactions
                var transactions = await _context.Transactions
                    .Include(t => t.Category)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                // Calculate total expenses
                viewModel.TotalExpenses = transactions.Sum(t => t.Amount);

                // Calculate monthly expenses
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                viewModel.MonthlyExpenses = transactions
                    .Where(t => t.TransactionDate.Month == currentMonth && t.TransactionDate.Year == currentYear)
                    .Sum(t => t.Amount);

                // Calculate weekly expenses
                var weekStart = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                viewModel.WeeklyExpenses = transactions
                    .Where(t => t.TransactionDate >= weekStart)
                    .Sum(t => t.Amount);

                // Get recent transactions
                viewModel.RecentTransactions = transactions.Take(5).ToList();

                // Calculate category-wise expenses
                viewModel.CategoryWiseExpenses = transactions
                    .GroupBy(t => t.Category.Name)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(t => t.Amount)
                    );

                // Calculate daily expenses for the last 30 days
                var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                viewModel.DailyExpenses = transactions
                    .Where(t => t.TransactionDate >= thirtyDaysAgo)
                    .GroupBy(t => t.TransactionDate.Date)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(t => t.Amount)
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating dashboard data");
                // Initialize empty collections to prevent null reference exceptions in the view
                viewModel.RecentTransactions = new List<Transaction>();
                viewModel.CategoryWiseExpenses = new Dictionary<string, decimal>();
                viewModel.DailyExpenses = new Dictionary<DateTime, decimal>();
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
