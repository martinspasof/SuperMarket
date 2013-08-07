using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Supermarket.Data
{
    [Table("Dates")]
    public class Date
    {
        [Key]
        public int DateId { get; set; }

        //[Required]
        public string SaleDate { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public Date()
        {
            this.Sales = new HashSet<Sale>();
        }
    }
}