using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sonnetly.Models
{
    public class ClicksLog
    {
        public int Id { get; set; }
        public int BookmarkId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Date Accessed")]
        public DateTime Created { get; set; }
    }
}