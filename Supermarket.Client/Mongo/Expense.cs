using System;
using MongoDB.Bson;

namespace Supermarket.Client.Mongo
{
    public class Expense
    {
        public ObjectId _id { get; set; }

        public string VendorName { get; set; }

        public string Month { get; set; }

        public string Value { get; set; }
    }
}
