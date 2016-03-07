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
using Microsoft.AspNet.Identity;
using MarkdownManagerNew.Viewmodels;

namespace MarkdownManagerNew.Controllers
{
    [Authorize(Roles="User")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext(); //Ta bort sen
        Repository repo = new Repository();
        
        private ApplicationUser GetCurrentUser()
        {
            return repo.GetUser(User.Identity.GetUserId());
        }
        // GET: User
        
        public ActionResult Index()
        {
            return View(repo.GetUserDocuments(GetCurrentUser()));
            //return View(repo.listAllDocuments());
        }

        [HttpGet]
        public ActionResult CreateDocument()
        {
            Document model = new Document();
            return View(model);
        }

        [HttpPost]
        //public ActionResult CreateGroup(List<string> groupMembers, string title, string description, CreateGroupViewModel viewModel)
        public ActionResult CreateGroup(CreateGroupViewModel viewModel)
        {
            // ändra parameters till:  List<ApplicationUser> groupMembers, List<Document> documents, string name, string description
            var user = GetCurrentUser();
            //repo.CreateGroup(groupMembers, user, title, description, viewModel);
            repo.CreateGroup(viewModel, user);
            ViewBag.Test = "A new group has been created!";
            return View("Index");
        }


        [HttpGet]
        public ActionResult CreateGroup()
        {
            ViewBag.Test = "No group has been created";
            CreateGroupViewModel viewModel = new CreateGroupViewModel();

            List<ApplicationUser> tempUserList = repo.ListUsersToCreateGroup();
            var checkBoxListItems = new List<CheckBoxListItem>();

            foreach (var user in tempUserList)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = user.Id,
                    Display = user.LastName + "," + user.FirstName,
                    IsChecked = false
                });
            }
            viewModel.CheckBoxUsers = checkBoxListItems;

            return View(viewModel);
        }

        // GET: User/Details/5
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

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
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

        // GET: User/Edit/5
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

        // POST: User/Edit/5
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
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
