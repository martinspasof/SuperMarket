using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Supermarket.Data
{
    [Table("Sales")]
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [Column("Sum")]
        public decimal Sum { get; set; }

        //[ForeignKey("Location")]
        [Column("LocationId")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        //[ForeignKey("Date")]
        [Column("DateId")]
        public int DateId { get; set; }
        public virtual Date Date { get; set; }
    }
}