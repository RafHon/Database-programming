using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }

        public int? GroupID { get; set; }
        [ForeignKey(nameof(GroupID))]
        public UserGroup UserGroup { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<BasketPosition> BasketPositions { get; set; }
    }
}
