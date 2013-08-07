using System;
using System.IO;
using System.Linq;
using System.Text;
using MongodbDemo.Data.Helpers;
using Supermarket.Data;
using iTextSharp.text;

namespace PDFGenerator
{
    public static class PDFWriter
    {
        public static void Export()
        {
            var supermarketDb = new SuperMarketContext();
            var strBuilder = new StringBuilder();

            var dates = supermarketDb.Dates.Select(x => x.SaleDate).ToArray();
            decimal grandTotal = 0m;

            strBuilder.Append("<table border='1'>");
            strBuilder.Append("<tr>");
            strBuilder.Append("<th style=\"font-size:16px; text-align:center;\" colspan='5'>Aggregated Sales Report</th>");
            strBuilder.Append("</tr>");
            strBuilder.Append("</table>");
           
            for (int i = 0; i < dates.Count(); i++)
            {
                strBuilder.Append("<table border='1'>");
                strBuilder.Append("<tr bgcolor='#BBBBBB'>");
                strBuilder.AppendFormat("<th colspan='5'>Date: {0}</th>",dates[i]);
                strBuilder.Append("</tr>");
                strBuilder.Append("<tr bgcolor='#BBBBBB'>");
                strBuilder.Append("<th class=\"th\"><b>Product Name</b></th>");
                strBuilder.Append("<th><b>Quantity</b></th>");
                strBuilder.Append("<th><b>Unit Price</b></th>");
                strBuilder.Append("<th><b>Location</b></th>");
                strBuilder.Append("<th><b>Sum</b></th>");
                strBuilder.Append("</tr>");

                string saleDate = dates[i];
                var sales = 
                    from sale in supermarketDb.Sales
                    join product in supermarketDb.Products
                    on sale.ProductId equals product.ID
                    where sale.Date.SaleDate == saleDate
                    select new
                    {
                        Product = product.ProductName,
                        Quantity = sale.Quantity,
                        UnitPrice = sale.UnitPrice,
                        LocationName = sale.Location.Name,
                        Sum = sale.Sum
                    };

                decimal totalSum = 0;
                foreach (var sale in sales)
                {
                    totalSum = totalSum + sale.Sum;
                    strBuilder.Append("<tr>");
                    strBuilder.AppendFormat("<td>{0}</td>", sale.Product);
                    strBuilder.AppendFormat("<td>{0}</td>", sale.Quantity);
                    strBuilder.AppendFormat("<td>{0}</td>", sale.UnitPrice);
                    strBuilder.AppendFormat("<td>{0}</td>", sale.LocationName);
                    strBuilder.AppendFormat("<td>{0}</td>", sale.Sum);
                    strBuilder.Append("</tr>");
                }

                grandTotal = grandTotal + totalSum;

                strBuilder.Append("<tr>");
                strBuilder.AppendFormat("<td colspan='4' align=right>Total Sum for {0}: </td>", dates[i]);
                strBuilder.AppendFormat("<td>{0}</td>", totalSum);
                strBuilder.Append("</tr>");
                strBuilder.Append("</table>");

                if (i != 2)
                {
                    strBuilder.Append("<br />");
                }
            }

            strBuilder.Append("<table border='1'>");
            strBuilder.Append("<tr>");
            strBuilder.Append("<td colspan='4' align=right>Grand total: </td>");
            strBuilder.AppendFormat("<td>{0}</td>", grandTotal);
            strBuilder.Append("</tr>");
            strBuilder.Append("</table>");

            PDFBuilder.HtmlToPdfBuilder builder = new PDFBuilder.HtmlToPdfBuilder(PageSize.LETTER);
            PDFBuilder.HtmlPdfPage page = builder.AddPage();
            page.AppendHtml(strBuilder.ToString());

            byte[] file = builder.RenderPdf();
            string tempFolder = "../../../PDFReports/";
            string tempFileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + ".pdf";
            if (Helpers.DirectoryExist(tempFolder))
            {
                if (!File.Exists(tempFolder + tempFileName))
                    File.WriteAllBytes(tempFolder + tempFileName, file);
            }
        }
    }
}
