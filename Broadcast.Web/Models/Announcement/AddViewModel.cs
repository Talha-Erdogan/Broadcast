using Broadcast.Web.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.Web.Models.Announcement
{
    public class AddViewModel
    {
        public int Id { get; set; }

        [Required]
        public int BroadcastTypeId { get; set; }

        [StringLength(500)]
        public string TitleTR { get; set; }

        [StringLength(500)]
        public string TitleEN { get; set; }

        public string DescriptionTR { get; set; }
        public string DescriptionEN { get; set; }

        public IFormFile ImageFilePath { get; set; }

        public string VideoFileUrl { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ValidationEndDateTime { get; set; }

        public bool IsActive { get; set; }

        public List<SelectListItem> BroadcastTypeSelectList { get; set; }

        // others
        public string DisplayLanguage { get; set; }
        public string SubmitType { get; set; }

        //Herhangi bir hata oldugunda, sessiondaki resmin eklenmesi işlevidir.
        public string SessionImageFileName { get; set; }//sessionda tutulan resmin ismi
        public ImageInformation ImageInformation { get; set; } //resim bilgileri
        public string SessionGuid { get; set; } //resmi eklerken, session guid ile ekliyoruz ki, bellekte kalan image verisi tekrar tekrar eklenmesin
    }
}
