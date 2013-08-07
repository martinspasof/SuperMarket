using System;
using System.IO.Compression;

namespace Supermarket.Client
{
    public static class ZipExtractor
    {
        public static void Extract()
        {
            string zipPath = @"..\..\..\Sample-Sales-Reports.zip";
            string extractPath = @"..\..\..\ExcelReports";

            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
            }
            catch (Exception)
            {
                Console.WriteLine("File Already Exists");
            }
        }
    }
}
