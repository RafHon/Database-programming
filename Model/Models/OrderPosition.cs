using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class OrderPosition
    {
        
        public int OrderID { get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order Order { get; set; }

        public int ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
