using Microsoft.AspNet.Identity;
using Sonnetly.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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



        // GET: Sonnet Details
        [Authorize]
        public ActionResult Detail(int? id)
        {
            var userId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sonnet = db.Bookmarks
                .Where(b => b.Id == id && b.OwnerId == userId)
                .FirstOrDefault();

            if (sonnet == null)
            {
                return HttpNotFound();
            }

            ViewBag.Clicks = db.Clicks
                .Where(c => c.BookmarkId == id)
                .OrderByDescending(c => c.Created)
                .ToList();

            return View(sonnet);
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
            string userName = User.Identity.GetUserName();
            if (userName == null)
            {
                userName = "Guest";
            }

            //Add click to db
            sonnet.NumClicks += 1;
            db.Entry(sonnet).State = EntityState.Modified;

            ClicksLog click = new ClicksLog
            {
                BookmarkId = sonnet.Id,
                Created = DateTime.Now,
                UserName = userName
            };
            db.Clicks.Add(click);
            db.SaveChanges();

            return new RedirectResult(sonnet.OriginalUrl);
        }

        // EDIT: Sonnet
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();

            Bookmarks bookmark = db.Bookmarks
                .Where(b => b.OwnerId == userId)
                .Where(b => b.Id == id)
                .FirstOrDefault();

            if (bookmark == null)
            {
                return HttpNotFound();
            }

            return View(bookmark);
        }

        // POST: Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Desc,OriginalUrl,NewUrl,NumClicks,OwnerId,Created")] Bookmarks sonnet)
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                db.Entry(sonnet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sonnet);
        }

        // DELETE: Sonnet
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmarks sonnet = db.Bookmarks.Find(id);

            if (sonnet == null)
            {
                return HttpNotFound();
            }

            return View(sonnet);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookmarks sonnet = db.Bookmarks.Find(id);
            db.Bookmarks.Remove(sonnet);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}