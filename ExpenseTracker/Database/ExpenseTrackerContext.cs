using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Database
{
    public class ExpenseTrackerContext : DbContext
    {
        public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TransactionDate).IsRequired();

                // Configure relationships
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vendor)
                    .WithMany(v => v.Transactions)
                    .HasForeignKey(e => e.VendorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);

                // Seed initial categories
                entity.HasData(
                    new Category { CategoryId = 1, Name = "Food & Dining", Description = "Restaurants, groceries, and dining expenses" },
                    new Category { CategoryId = 2, Name = "Transportation", Description = "Public transport, fuel, and vehicle maintenance" },
                    new Category { CategoryId = 3, Name = "Shopping", Description = "Clothing, electronics, and general shopping" },
                    new Category { CategoryId = 4, Name = "Utilities", Description = "Electricity, water, internet, and phone bills" },
                    new Category { CategoryId = 5, Name = "Entertainment", Description = "Movies, games, and recreational activities" },
                    new Category { CategoryId = 6, Name = "Healthcare", Description = "Medical expenses and healthcare costs" },
                    new Category { CategoryId = 7, Name = "Education", Description = "Books, courses, and educational materials" },
                    new Category { CategoryId = 8, Name = "Housing", Description = "Rent, mortgage, and home maintenance" }
                );
            });

            // Configure Vendor entity
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.ContactInfo).HasMaxLength(100);

                // Seed initial vendors
                entity.HasData(
                    new Vendor { VendorId = 1, Name = "Local Supermarket", Description = "Local grocery store", ContactInfo = "123-456-7890" },
                    new Vendor { VendorId = 2, Name = "City Transport", Description = "Public transportation", ContactInfo = "transport@city.com" },
                    new Vendor { VendorId = 3, Name = "Online Marketplace", Description = "E-commerce platform", ContactInfo = "support@marketplace.com" }
                );
            });
        }
    }
}
