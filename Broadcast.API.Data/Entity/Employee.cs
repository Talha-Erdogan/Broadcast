﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Broadcast.API.Data.Entity
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [StringLength(50)]
        public int ID { get; set; }

        [StringLength(50)]
        public string TRNationalId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        public int SexId { get; set; }

    }
}
