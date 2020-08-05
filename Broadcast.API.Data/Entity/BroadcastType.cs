using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Broadcast.API.Data.Entity
{
    [Table("BroadcastType")]
    public class BroadcastType
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(50)]
        public string NameTR { get; set; }

        [StringLength(50)]
        public string NameEN { get; set; }
    }
}
