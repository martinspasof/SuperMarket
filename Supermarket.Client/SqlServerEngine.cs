using System;
using Supermarket.Data;
using XMLReader;

namespace Supermarket.Client
{
    public class SqlServerEngine
    {
        public static void ImportExpensesFromXml()
        {
            var expenses = ReadXML.GetObjects("..\\..\\..\\Vendors-Expenses.xml");

            using (var context = new SuperMarketContext())
            {
                foreach (var expense in expenses)
                {
                    context.Expenses.Add(new Expense { VendorName = expense.Item1, Month = expense.Item2, Value = expense.Item3 });
                }

                context.SaveChanges();
            }
        }
    }
}
