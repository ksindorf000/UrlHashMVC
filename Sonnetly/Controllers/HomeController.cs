using Sonnetly.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sonnetly.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.sonnetList = db.Bookmarks.OrderByDescending(b => b.Created).ToList();
            return View();
        }
        
        
    }
}