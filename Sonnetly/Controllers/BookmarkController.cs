using Microsoft.AspNet.Identity;
using Sonnetly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sonnetly.Controllers
{
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Bookmark
        public ActionResult Index()
        {
            return View();
        }

        // CREATE: Bookmark
        public ActionResult Create()
        {
            return View();
        }

        // CREATE/POST: Bookmark
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Desc,OriginalUrl,NewUrl,NumClicks,OwnerId,Created")] Bookmarks newBM)
        {
            if (ModelState.IsValid)
            {
                newBM.Created = DateTime.Now;
                newBM.OwnerId = User.Identity.GetUserId();
                newBM.NewUrl = Helpers.EncryptURL.AndGo(newBM.OriginalUrl, newBM.OwnerId);

                db.Bookmarks.Add(newBM);
                db.SaveChanges();

                RedirectToAction("Detail", new { id = newBM.Id });
            }

            return View();
        }


        

    }
}