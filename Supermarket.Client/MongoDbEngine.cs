using System;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Data;
using Supermarket.Data;
using XMLReader;
using Newtonsoft.Json;

namespace Supermarket.Client
{
    public static class MongoDbEngine
    {
        public static void ImportProductReportsFromSqlServer()
        {
            SuperMarketContext supermarketDb = new SuperMarketContext();

            var salesInfo =
                from sale in supermarketDb.Sales
                join product in supermarketDb.Products
                on sale.ProductId equals product.ID
                select new
                {
                    ProductId = product.ID,
                    ProductName = product.ProductName,
                    VendorName = product.Vendor.VendorName,
                    Quantity = supermarketDb.Sales.Where(s => s.ProductId == product.ID).Select(s => s.Quantity).Sum(),
                    Income = supermarketDb.Sales.Where(s => s.ProductId == product.ID).Select(s => s.Sum).Sum()
                };

            foreach (var sale in salesInfo)
            {
                Mongo.Product product = new Mongo.Product();
                product.ProductId = sale.ProductId;
                product.ProductName = sale.ProductName;
                product.VendorName = sale.VendorName;
                product.TotalQuantitySold = sale.Quantity;
                product.TotalIncomes = sale.Income;

                int productId = MongoDbProvider.LoadData<Mongo.Product>().Where(x => x.ProductId == sale.ProductId).Select(x => x.ProductId).FirstOrDefault();
                if (productId == 0)
                {
                    MongoDbProvider.SaveData<Mongo.Product>(product);
                }
            }
        }

        public static void ImportExpensesReportsFromXml()
        {
            var expenses = ReadXML.GetObjects("..\\..\\..\\Vendors-Expenses.xml");
            foreach (var expense in expenses)
            {
                Mongo.Expense newExpense = new Mongo.Expense();
                newExpense.VendorName = expense.Item1;
                newExpense.Month = expense.Item2;
                newExpense.Value = expense.Item3;

                MongoDbProvider.SaveData<Mongo.Expense>(newExpense);
            }
        }

        public static void GenerateJsonReports()
        {
            var products = MongoDbProvider.LoadData<Mongo.Product>();//.ToList();

            foreach (var product in products)
            {
                using (StreamWriter writer = new StreamWriter(String.Format("../../../JSONReports/{0:00}.json", product.ProductId)))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("{").AppendLine();
                    sb.AppendFormat("\t\"product-id\": {0},", product.ProductId).AppendLine();
                    sb.AppendFormat("\t\"product-name\": \"{0}\",", product.ProductName).AppendLine();
                    sb.AppendFormat("\t\"vendor-name\": \"{0}\",", product.VendorName).AppendLine();
                    sb.AppendFormat("\t\"quantity\": {0},", product.TotalQuantitySold).AppendLine();
                    sb.AppendFormat("\t\"income\": {0},", product.TotalIncomes).AppendLine();
                    sb.Append("}").AppendLine();

                    writer.WriteLine(sb);
                }
            }
        }
    }
}
