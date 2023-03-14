using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sivam.web.Models
{
    [Table("tblBanners")]
    public class BannerModel
    {
        [Key]
        public int Bid { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }

    //public class banner
    //{
    //    [Required(ErrorMessage = "Please enter title")]
    //    [Display(Name = "Title")]
    //    public string Title { get; set; }

    //    [Required(ErrorMessage = "Please choose image")]
    //    public string ImgUrl { get; set; }
    //    public bool IsActive { get; set; }
    //}
}