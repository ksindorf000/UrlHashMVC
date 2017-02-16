using Microsoft.AspNet.Identity;
using Sonnetly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sonnetly.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Current User
        [Authorize]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            ViewBag.sonnetList = db.Bookmarks
                .Where(b => b.Owner.Id == userId)
                .OrderByDescending(b => b.Created)
                .ToList();

            ViewBag.favsList = db.Favorites
                .Where(f => f.Owner.Id == userId)
                .OrderByDescending(f => f.Bookmark.Title)
                .ToList();

            return View(currentUser);
        }

        // GET: Sonnets from User in Route
        [Route("User/{userName}")]
        public ActionResult Index(string userName)
        {
            ApplicationUser targetUser = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
            string userId = targetUser.Id;

            ViewBag.sonnetList = db.Bookmarks
                .Where(b => b.Owner.UserName == userName)
                .OrderByDescending(b => b.Created)
                .ToList();

            return View(targetUser);
        }

        // GET: Current User
        [Authorize]
        public ActionResult Favorites(string userName)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();
            
            ViewBag.favsList = db.Favorites
                .Where(f => f.Owner.Id == userId)
                .OrderByDescending(f => f.Bookmark.Title)
                .ToList();

            return View(currentUser);
        }

    }
}