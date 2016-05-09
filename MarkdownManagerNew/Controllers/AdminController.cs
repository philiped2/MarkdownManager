using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarkdownManagerNew.Models;
using MarkdownManagerNew.Repositories;
using MarkdownManagerNew.Viewmodels;
using Microsoft.AspNet.Identity;

namespace MarkdownManagerNew.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Repository repo = new Repository();

        private ApplicationUser GetCurrentUser()
        {
            return repo.GetUser(User.Identity.GetUserId());
        }

        public ActionResult Index(string message)
        {
            repo.DeleteOldArchivedDocuments();

            ViewBag.message = message;
            return View(GetCurrentUser());
        }

        // GET: Admin
        public ActionResult ShowDocuments()
        {
            return View(repo.GetAllDocuments());
        }

        public ActionResult ShowGroups()
        {
            return View(repo.GetAllGroups());
        }

        public ActionResult ShowUsers()
        {
            return View(repo.GetAllUsers());
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            CreateUserViewModel model = new CreateUserViewModel();

            foreach (var group in repo.GetAllGroups())
            {
                model.Groups.Add(new CheckBoxListGroup()
                {
                    ID = group.ID,
                    Display = group.Name,
                    IsChecked = false
                });
            }

            foreach (var doc in repo.GetAllDocuments())
            {
                model.Documents.Add(new CheckBoxListDocuments()
                {
                    ID = doc.ID,
                    Display = doc.Name,
                    IsChecked = false
                });
            }

            model.admin = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel model)
        {
            repo.CreateUser(model, GetCurrentUser());
            return RedirectToAction("Index", new { message = "User was created" });
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        public ActionResult GroupDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            //group.ChangeLog.Add("hejsan");
            return View(group);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Name,Markdown,DateCreated,LastChanged,CreatorId")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }

            return View(document);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Markdown,DateCreated,LastChanged,CreatorId")] Document document)
        {

            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", new { message = "Document was changed!" });
            }
            return View(document);
        }

        public ActionResult EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup([Bind(Include = "Id,Description,Name,ChangeLog,DateCreated,LastChanged,CreatorID")] Group group)
        {
            //DateTime timeChanged = DateTime.Now;
            ApplicationUser currentUser = GetCurrentUser();


            if (ModelState.IsValid)
            {
                
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                //db.Entry(group).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
                repo.LogChanges(group, currentUser);
                
                return RedirectToAction("Index", new { message = "Group was changed!" });
            }
            return View(group);
        }

        // GET: Admin/Delete/5
        public ActionResult DeleteDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);

            //List<Document> documents = db.Groups.Where(x => x.Documents.Where(x => x.ID == document.ID).Single());

            //List<Group> groups = db.Groups.Any(x => x.Documents.Where(x => x.ID == id).Single()));
            
            //foreach (Group group in db.Groups.Where(x => x.Documents.Where(x => x.ID == id).Single()))
            //{
                
            //}

            repo.ArchiveDocument(document);
            //db.Documents.Remove(document);
            //db.SaveChanges();
            return RedirectToAction("ShowDocuments");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOfGroupConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("ShowGroups");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ActionResult RestoreDocument(int? id)
        {
            Document document = db.Documents.Find(id);
            repo.RestoreArchivedDocument(document);
            return View("ShowDocuments", repo.GetAllDocuments());
        }

        public ActionResult DocumentDelTimeSetting()
        {
            return View();
        }

        public ActionResult GetSystemSettingsJson(string settingName)
        {
            if (settingName == "documentDelTime")
            {
                var settings = repo.GetDocumentDeleteTimeSettings();
                return Json(new { activated = settings.Activated, timeValue = settings.TimeValue, timeUnit = settings.TimeUnit, settingName = settingName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SetArchiveDeleteSettings(bool activated, int timeValue, string timeUnit)
        {
            if (timeValue > 0)
            {
                repo.SetDocumentDeleteTimeSettings(activated, timeValue, timeUnit);
                return Json(new { message = "Inställningar ändrade" });
            }
            else
            {
                return Json(new { message = "Inställnigar godkänns inte" });
            }

        }
    }
}
