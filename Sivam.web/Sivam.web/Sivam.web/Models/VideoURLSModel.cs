using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sivam.web.Models
{
    public class VideoURLSModel
    {
        public int RecId { get; set; }
        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile")]
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}