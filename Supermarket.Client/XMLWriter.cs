using System.Linq;
using System.Xml.Linq;
using Supermarket.Data;

namespace Supermarket.Client
{
    public class XMLWriter
    {
        public static void GenerateReports()
        {
            string reportPath = "../../../XMLReports/Sales-by-Vendors-report.xml";

            using (var supermarketDb = new SuperMarketContext())
            {
                XElement root = new XElement("sales");

                var vendors = supermarketDb.Vendors.ToList();
                foreach (var vendor in vendors)
                {
                    var entries = 
                        from s in supermarketDb.Sales
                        join p in supermarketDb.Products on s.ProductId equals p.ID
                        join v in supermarketDb.Vendors on p.VendorID equals v.ID
                        where v.ID == vendor.ID
                        group s by s.Date into y
                        select new
                            {
                                Date = y.Key,
                                Sum = y.Sum(x => x.Sum)
                            };

                    // Skip vendors without sales
                    if (entries.Count() == 0)
                    {
                        continue;
                    }

                    XElement sale = new XElement("sale");
                    foreach (var entry in entries)
                    {
                        sale.SetAttributeValue("vendor", vendor.VendorName);
                        sale.Add(new XElement("summary",
                            new XAttribute("date",
                                entry.Date.SaleDate),
                                new XAttribute("total-sum", entry.Sum.ToString("0.00"))));
                    }

                    root.Add(sale);
                }

                root.Save(reportPath);
            }
        }
    }
}
