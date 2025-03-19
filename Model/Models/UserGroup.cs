using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserGroup
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
