using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models.Broadcasts
{
    public class AddRequestModel
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
    public class UpdateStatusRequestModel
    {
        /// <summary>
        /// Broadcast status: active or passive
        /// </summary>
        public bool IsActive { get; set; }
    }
}
