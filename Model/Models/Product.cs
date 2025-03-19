using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }

        public int? GroupID { get; set; }
        [ForeignKey(nameof(GroupID))]
        public ProductGroup ProductGroup { get; set; }

        public ICollection<BasketPosition> BasketPositions { get; set; }
        public ICollection<OrderPosition> OrderPositions { get; set; }
    }
}
