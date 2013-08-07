using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket.Data
{
    public partial class Product
    {
        public Product()
        {
            this.sales = new HashSet<Sale>();
        }

        private ICollection<Sale> sales;

        public ICollection<Sale> Sales
        {
            get { return sales; }
            set { sales = value; }
        }
    }
}