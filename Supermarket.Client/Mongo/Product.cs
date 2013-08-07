using System;
using MongoDB.Bson;

namespace Supermarket.Client.Mongo
{
    public class Product
    {
        public ObjectId _id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; }

        public int TotalQuantitySold { get; set; }

        public decimal TotalIncomes { get; set; }
    }
}
