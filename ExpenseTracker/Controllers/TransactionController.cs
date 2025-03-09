using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Database;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ExpenseTrackerContext _context;

        public TransactionController(ExpenseTrackerContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Vendor)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
            return View(transactions);
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Vendor)
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            var viewModel = new TransactionViewModel
            {
                TransactionDate = DateTime.Now,
                Categories = _context.Categories.ToList(),
                Vendors = _context.Vendors.ToList()
            };
            return View(viewModel);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    Amount = viewModel.Amount,
                    Description = viewModel.Description,
                    TransactionDate = viewModel.TransactionDate,
                    CategoryId = viewModel.CategoryId,
                    VendorId = viewModel.VendorId
                };

                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = _context.Categories.ToList();
            viewModel.Vendors = _context.Vendors.ToList();
            return View(viewModel);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            var viewModel = new TransactionViewModel
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                CategoryId = transaction.CategoryId,
                VendorId = transaction.VendorId,
                Categories = _context.Categories.ToList(),
                Vendors = _context.Vendors.ToList()
            };

            return View(viewModel);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionViewModel viewModel)
        {
            if (id != viewModel.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var transaction = new Transaction
                    {
                        TransactionId = viewModel.TransactionId,
                        Amount = viewModel.Amount,
                        Description = viewModel.Description,
                        TransactionDate = viewModel.TransactionDate,
                        CategoryId = viewModel.CategoryId,
                        VendorId = viewModel.VendorId
                    };

                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(viewModel.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = _context.Categories.ToList();
            viewModel.Vendors = _context.Vendors.ToList();
            return View(viewModel);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Vendor)
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
