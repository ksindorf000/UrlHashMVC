using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sonnetly.Models
{
    public class Favorites
    {
        public int Id { get; set; }

        public int BookmarkId { get; set; }

        [ForeignKey("BookmarkId")]
        public virtual Bookmarks Bookmark { get; set; }

        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
        
    }
}