using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey(nameof(UserID))]
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; }
        public ICollection<OrderPosition> OrderPositions { get; set; }
    }

}
