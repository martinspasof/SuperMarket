using System;
using System.Data.Entity;

namespace Supermarket.Data
{
    public class SuperMarketContext : DbContext
    {
        public SuperMarketContext()
            : base("SupermarketMSSQL")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Date> Dates { get; set; }

        public DbSet<Expense> Expenses { get; set; }
    }
}
