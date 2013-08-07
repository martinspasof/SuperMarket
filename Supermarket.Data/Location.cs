using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Supermarket.Data
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public Location()
        {
            this.Sales = new HashSet<Sale>();
        }
    }
}