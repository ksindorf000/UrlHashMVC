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
    [Authorize]
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Current User
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();
            //User Specific Sonnets
            ViewBag.sonnetList = db.Bookmarks
                .Where(b => b.Owner.Id == userId)
                .OrderByDescending(b => b.Created)
                .ToList();
            return View();
        }


        // DETAILS: User from Route
        //Using an Attribute Route
        //Route(pattern)
        [Route("User/{userName}")]
        public ActionResult Detail(string userName)
        {
            ApplicationUser reqUser = db.Users
               .Where(u => u.UserName == userName)
               .FirstOrDefault();
            ViewBag.sonnetList = db.Bookmarks
                .Where(b => b.Owner.Id == reqUser.Id)
                .OrderByDescending(b => b.Created)
                .ToList();
            return View(reqUser);
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

            }

            return Redirect("Index");
        }

        //GET: All Sonnets
        public ActionResult AllSonnets()
        {
            ViewBag.sonnetList = db.Bookmarks.OrderByDescending(m => m.Created).ToList();
            return View();
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