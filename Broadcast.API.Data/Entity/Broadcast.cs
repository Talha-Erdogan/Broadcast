using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Broadcast.API.Data.Entity
{
    [Table("Broadcast")]
    public class Broadcast
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(500)]
        public string TitleTR { get; set; }
        [StringLength(500)]
        public string TitleEN { get; set; }

        public string DescriptionTR { get; set; }
        public string DescriptionEN { get; set; }

        [StringLength(500)]
        public string ImageFilePath { get; set; }
        [StringLength(500)]
        public string VideoFileUrl { get; set; }

        [Required]
        public DateTime ValidationEndDateTime { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int ModifiedBy { get; set; }

        public int BroadcastTypeId { get; set; }

    }
}
