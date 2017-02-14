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
            return View(currentUser);
        }

        // DETAILS: User from Route
        [Route("User/{userNameX}")]
        public ActionResult Index(string userNameX)
        {
            ApplicationUser targetUser = db.Users.Where(u => u.UserName == userNameX).FirstOrDefault();
            string userId = targetUser.Id;

            ViewBag.sonnetList = db.Bookmarks
                .Where(b => b.Owner.UserName == userNameX)
                .OrderByDescending(b => b.Created)
                .ToList();

            return View(targetUser);
        }        
    }
}