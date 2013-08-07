namespace Supermarket.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Supermarket.Data.SuperMarketContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Supermarket.Data.SuperMarketContext context)
        {
            var openAccesContext = new SupermarketModel();

            using (openAccesContext)
            {
                foreach (var product in openAccesContext.Products)
                {

                    context.Products.AddOrUpdate(new Product { ID = product.ID, VendorID = product.VendorID, ProductName = product.ProductName, MeasureID = product.MeasureID, BasePrice = product.BasePrice });
                }

                foreach (var measure in openAccesContext.Measures)
                {
                    context.Measures.AddOrUpdate(new Measure { ID = measure.ID, MeasureName = measure.MeasureName });
                }

                foreach (var vendor in openAccesContext.Vendors)
                {
                    context.Vendors.AddOrUpdate(new Vendor { ID = vendor.ID, VendorName = vendor.VendorName });
                }
            }
        }
    }
}
