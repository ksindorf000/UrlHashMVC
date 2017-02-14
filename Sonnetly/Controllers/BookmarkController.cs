using Microsoft.AspNet.Identity;
using Sonnetly.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sonnetly.Controllers
{
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: All Sonnets
        public ActionResult Index()
        {            
            ViewBag.sonnetList = db.Bookmarks
                .OrderByDescending(b => b.Created)
                .ToList();

            return View();
        }
        
        // CREATE: Bookmark
        [Authorize]
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

            }

            return Redirect("Index");
        }
        
        //SEND: Redirect to original Url
        [Route("Sonnet/{NewUrl}")]
        public ActionResult SonnetRedirect(string newUrl)
        {            
            var sonnet = db.Bookmarks.Where(b => b.NewUrl == newUrl).FirstOrDefault();

            //Add click to db
            sonnet.NumClicks += 1;
            db.Entry(sonnet).State = EntityState.Modified;
            db.SaveChanges();

            return new RedirectResult(sonnet.OriginalUrl);
        }
    }
}