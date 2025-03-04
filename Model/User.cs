﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(GroupID))]
        public int? GroupID { get; set; }

        public UserGroup Group { get; set; }
        public ICollection<BasketPosition> BasketPositions { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public enum UserType
    {
        Admin,
        Casual
    }

}
