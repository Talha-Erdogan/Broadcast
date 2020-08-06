using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Broadcast
{
    public class Broadcast
    {
        public int Id { get; set; }
        public string TitleTR { get; set; }
        public string TitleEN { get; set; }
        public string DescriptionTR { get; set; }
        public string DescriptionEN { get; set; }
        public string ImageFilePath { get; set; }
        public string VideoFileUrl { get; set; }
        public DateTime ValidationEndDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int ModifiedBy { get; set; }
        public int BroadcastTypeId { get; set; }
    }
}
