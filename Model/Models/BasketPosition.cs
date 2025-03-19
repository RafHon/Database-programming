using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class BasketPosition
    {
        public int ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        public int Amount { get; set; }
    }
}
