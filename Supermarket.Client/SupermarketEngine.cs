using System;
using System.Linq;
using Supermarket.Data;
using Supermarket.Data.Migrations;
using System.Data.Entity;

namespace Supermarket.Client
{
    // Import from MySQL to SQL Server
    // Import from Excel (zip) to SQL Server
    // Import from XML to SQL Server
    // Import from XML to MandoDB

    // Export to PDF from SQL Server
    // Export to XML from SQL Server
    // Export to JSON from MongoDB
    // Export Excel from SQLite

    public class SupermarketEngine
    {
        public static void Main()
        {
            // 1.1. Load MySQL DB to SQL Server
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SuperMarketContext, Configuration>());
            SuperMarketContext context = new SuperMarketContext();
            context.Database.Initialize(true);

            // 1.2 Unzip and import excel sales reports to SQL Server
            ZipExtractor.Extract();
            ExcelReader.ImportExcelReports();
            
            // 2. Generate PDF from SQL Server
            //PDFGenerator.PDFWriter.Export();

            // 3. Generate XML from SQL Server
            //XMLWriter.GenerateReports();
            
            // 4. Load product reports from SQL Server to MongoDB and Generate JSON files
            //MongoDbEngine.ImportProductReportsFromSqlServer();
            //MongoDbEngine.GenerateJsonReports();

            // 5. Import XML expenses fle to MongoDB and SQL Server
            //MongoDbEngine.ImportExpensesReportsFromXml();            
            //SqlServerEngine.ImportExpensesFromXml();
        }
    }
}
