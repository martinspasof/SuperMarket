using System;
using System.Collections.Generic;
using Supermarket.Data;
using System.Data.OleDb;
using System.Linq;
using System.Data;
using System.IO;

namespace Supermarket.Client
{
    public static class ExcelReader
    {
        private const string ReportsPath = @"..\..\..\ExcelReports";
        private static readonly List<string> filesPaths = new List<string>();

        public static void ImportExcelReports()
        {
            GenerateFilesPaths(ReportsPath, "*.xls");
            ReadExcelFiles();
        }

        private static void ReadExcelFiles()
        {
            OleDbConnectionStringBuilder csbuilder = new OleDbConnectionStringBuilder();
            csbuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
            csbuilder.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES");

            foreach (string filePath in filesPaths)
            {
                csbuilder.DataSource = filePath;
                DataTable dt = new DataTable("newtable");
                FillData(csbuilder, dt);

                string reportDate = GetDateFromFileName(filePath);
                string reportLocation = dt.Rows[0].ItemArray[0].ToString();

                Location location = new Location() { Name = reportLocation };
                Date date = new Date() { SaleDate = reportDate };

                int reportsCount = dt.Rows.Count - 1;
                for (int i = 2; i < reportsCount; i++)
                {
                    string saleProductId = dt.Rows[i].ItemArray[0].ToString();
                    string saleQuantity = dt.Rows[i].ItemArray[1].ToString();
                    string saleUnitPrice = dt.Rows[i].ItemArray[2].ToString();
                    string saleSum = dt.Rows[i].ItemArray[3].ToString();

                    if (!IsSaleReport(saleProductId))
                    {
                        break;
                    }

                    Sale sale = new Sale()
                    {
                        Quantity = int.Parse(saleQuantity),
                        UnitPrice = decimal.Parse(saleUnitPrice),
                        Sum = decimal.Parse(saleSum),
                        ProductId = int.Parse(saleProductId)
                    };

                    InsertToDB(location, date, sale);
                }
            }
        }

        private static bool IsSaleReport(string saleProductId)
        {
            int isParsed;
            int.TryParse(saleProductId, out isParsed);

            if (isParsed != 0)
            {
                return true;
            }

            return false;
        }

        private static string GetDateFromFileName(string filePath)
        {
            int reportIndex = filePath.IndexOf(("Report-"));
            int reportLength = "Report-".Length;
            string date = filePath.Substring(reportIndex + reportLength, filePath.LastIndexOf(".") - reportIndex - reportLength);
            return date;
        }

        private static void FillData(OleDbConnectionStringBuilder csbuilder, DataTable dt)
        {
            using (var connection = new OleDbConnection(csbuilder.ConnectionString))
            {
                connection.Open();
                string selectSql = @"SELECT * FROM [Sales$]";

                using (var adapter = new OleDbDataAdapter(selectSql, connection))
                {
                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);
                }

                connection.Close();
            }
        }

        private static void InsertToDB(Location location, Date date, Sale sale)
        {
            SuperMarketContext supermarketDb = new SuperMarketContext();

            int dateId = supermarketDb.Dates.Where(x => x.SaleDate == date.SaleDate).Select(x => x.DateId).FirstOrDefault();
            int locationId = supermarketDb.Locations.Where(x => x.Name == location.Name).Select(x => x.LocationId).FirstOrDefault();

            if ((dateId == 0) && (locationId == 0))
            {
                supermarketDb.Dates.Add(new Date() { SaleDate = date.SaleDate });
                supermarketDb.Locations.Add(new Location() { Name = location.Name });
            }
            else if (dateId == 0)
            {
                supermarketDb.Dates.Add(new Date() { SaleDate = date.SaleDate });
                sale.LocationId = locationId;
            }
            else if (locationId == 0)
            {
                supermarketDb.Locations.Add(new Location() { Name = location.Name });
                sale.DateId = dateId;
            }
            else
            {
                sale.DateId = dateId;
                sale.LocationId = locationId;
            }

            supermarketDb.Sales.Add(sale);
            supermarketDb.SaveChanges();
        }

        private static void GenerateFilesPaths(string currentPath, string fileExtension)
        {
            try
            {
                string[] currentDirFiles = Directory.GetFiles(currentPath, fileExtension);
                filesPaths.AddRange(currentDirFiles);

                string[] curretDirDirectories = Directory.GetDirectories(currentPath);
                foreach (var dir in curretDirDirectories)
                {
                    GenerateFilesPaths(dir, fileExtension);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Do nothing
            }
        }
    }
}
