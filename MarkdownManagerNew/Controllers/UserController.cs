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
using System.Web.Helpers;
using Newtonsoft.Json;

namespace MarkdownManagerNew.Controllers
{
    [Authorize(Roles = "User")]
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
        public ActionResult CreateTag()
        {
            Tag model = new Tag();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTag(Tag model)
        {
            repo.CreateTag(model);
            return RedirectToAction("Index", repo.GetUserDocuments(GetCurrentUser()));
        }

        public ActionResult GetTagsJson (string tagLabel)
        {
            var result = repo.GetTagsByName(tagLabel);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsersJson(string userName)
        {
            var result = repo.GetUserByName(userName);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TestPost(CreateDocumentViewModel viewModel, string[] tagList)
        {
            return View("Index");
        }

        [HttpGet]
        public ActionResult CreateDocument2()
        {
            CreateDocumentViewModel2 model = new CreateDocumentViewModel2();

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateDocument(List<File> files)
        {
            CreateDocumentViewModel viewModel = (CreateDocumentViewModel)TempData["viewModel"];
            HttpPostedFileBase file = (HttpPostedFileBase)TempData["fileToAdd"];

            CreateDocumentViewModel model = new CreateDocumentViewModel();

            if (viewModel != null)
            {
                model = viewModel;
                TempData["viewModel"] = null;
                return View(model);
            }
            

            var groups = repo.GetAllGroups();

            foreach (var user in repo.GetAllUsers())
            {
                model.CheckboxUsers.Add(new CheckBoxListUser()
                {
                    ID = user.Id,
                    Display = user.LastName + "," + user.FirstName,
                    IsChecked = false
                });
            }

            foreach (var group in repo.GetAllGroups())
            {
                model.CheckboxGroups.Add(new CheckBoxListGroup()
                {
                    ID = group.ID,
                    Display = group.Name,
                    IsChecked = false
                });
            }

            foreach (var tag in repo.GetAllTags())
            {
                model.CheckboxTags.Add(new CheckBoxListTags()
                {
                    ID = tag.ID,
                    Display = tag.Label,
                    IsChecked = false
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateDocument(CreateDocumentViewModel viewModel, HttpPostedFileBase upload, string[] selectedFiles)
        {
            //dynamic fileArray = JsonConvert.DeserializeObject(viewModel.FilesJson);

            //foreach (var file in fileArray)
            //{
            //    Console.WriteLine(file.Filename); 
            //}

            if (Request.Form["CreateFile"] != null)
            {
                var file = repo.CreateFile(upload, GetCurrentUser());
                viewModel.Files.Add(file);
                TempData["viewModel"] = viewModel;

                //return RedirectToAction("CreateDocument", file);
                return RedirectToAction("CreateDocument", new { files = viewModel.Files });
            }
            else if (Request.Form["CreateDocument"] != null)
            {
                //List<File> fileList = repo.CreateFileListFromJson(viewModel.FilesJson, GetCurrentUser());

                var user = GetCurrentUser();
                repo.CreateDocument(viewModel, user);
                return RedirectToAction("Index", repo.GetUserDocuments(GetCurrentUser()));
            }

            else
            {
                return View("Index");
            }

            
        }


        [HttpPost]
        public ActionResult CreateGroup(CreateGroupViewModel viewModel)
        {
            var user = GetCurrentUser();
            repo.CreateGroup(viewModel, user);
            ViewBag.Test = "A new group has been created!";
            return View("Index", repo.GetUserDocuments(GetCurrentUser()));
        }


        [HttpGet]
        public ActionResult CreateGroup()
        {
            ViewBag.Test = "No group has been created";
            CreateGroupViewModel viewModel = new CreateGroupViewModel();

            List<ApplicationUser> tempUserList = repo.ListUsersToCreateGroup();
            var checkBoxListItems = new List<CheckBoxListUser>();

            foreach (var user in tempUserList)
            {
                checkBoxListItems.Add(new CheckBoxListUser()
                {
                    ID = user.Id,
                    Display = user.LastName + "," + user.FirstName,
                    IsChecked = false,

                    CanEdit = false,
                    IsGroupAdmin = false
                });
            }
            viewModel.CheckBoxUsers = checkBoxListItems;

            List<Document> tempDocumentList = repo.GetUserDocuments(GetCurrentUser());
            var checkBoxListDocuments = new List<CheckBoxListDocuments>();

            foreach (var document in tempDocumentList)
            {
                checkBoxListDocuments.Add(new CheckBoxListDocuments()
                {
                    ID = document.ID,
                    Display = document.Name,
                    IsChecked = false
                });
            }

            viewModel.CheckBoxDocuments = checkBoxListDocuments;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult NewCreateGroup(CreateGroupViewModel viewModel)
        {
            var user = GetCurrentUser();
            repo.CreateGroup(viewModel, user);
            ViewBag.Test = "A new group has been created!";
            return View("Index", repo.GetUserDocuments(GetCurrentUser()));
        }

        [HttpGet]
        public ActionResult NewCreateGroup()
        {
            ViewBag.Test = "No group has been created";
            CreateGroupViewModel viewModel = new CreateGroupViewModel();

            List<ApplicationUser> tempUserList = repo.ListUsersToCreateGroup();
            var checkBoxListItems = new List<CheckBoxListUser>();

            foreach (var user in tempUserList)
            {
                checkBoxListItems.Add(new CheckBoxListUser()
                {
                    ID = user.Id,
                    Display = user.LastName + "," + user.FirstName,
                    IsChecked = false,

                    CanEdit = false,
                    IsGroupAdmin = false
                });
            }
            viewModel.CheckBoxUsers = checkBoxListItems;

            List<Document> tempDocumentList = repo.GetUserDocuments(GetCurrentUser());
            var checkBoxListDocuments = new List<CheckBoxListDocuments>();

            foreach (var document in tempDocumentList)
            {
                checkBoxListDocuments.Add(new CheckBoxListDocuments()
                {
                    ID = document.ID,
                    Display = document.Name,
                    IsChecked = false
                });
            }

            viewModel.CheckBoxDocuments = checkBoxListDocuments;

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
            var currentUser = GetCurrentUser();
            Document document = db.Documents.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            //else if (currentUser.DocumentRights.Any(x => x.document.ID == document.ID))
            else
            {
                return View(document);
            }
            //return HttpNotFound();
            //return View(document);
        }


        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Markdown,DateCreated,LastChanged,CreatorID")] Document document)
        {
            ApplicationUser currentUser = GetCurrentUser();

            if (ModelState.IsValid)
            {
                repo.LogDocumentChanges(document, currentUser);
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(document);
        }

        [HttpPost]
        public ActionResult EditGroup(EditGroupViewModel viewModel)
        {
            //var user = GetCurrentUser();
            repo.EditGroup2(viewModel);
            //return View("Index", repo.GetUserDocuments(GetCurrentUser()));
            return View("Index");
        }

        public ActionResult EditGroup(int? id)
        {
            EditGroupViewModel viewModel = new EditGroupViewModel();
            List<ApplicationUser> tempUserList = repo.ListUsersToCreateGroup();
            var checkBoxListItems = new List<CheckBoxListUser>();
            Group groupToEdit = db.Groups.Where(x => x.ID == id).Single();
            viewModel.GroupToEdit = groupToEdit;
            foreach (var user in tempUserList)
            {
                checkBoxListItems.Add(new CheckBoxListUser()
                {
                    ID = user.Id,
                    Display = user.LastName + "," + user.FirstName,
                    IsChecked = false,
                    UserToDelete = false,

                    CanEdit = false,
                    IsGroupAdmin = false
                });
            }
            viewModel.CheckBoxUsers = checkBoxListItems;

            List<Document> tempDocumentList = repo.GetUserDocuments(GetCurrentUser());
            var checkBoxListDocuments = new List<CheckBoxListDocuments>();

            foreach (var document in tempDocumentList)
            {
                checkBoxListDocuments.Add(new CheckBoxListDocuments()
                {
                    ID = document.ID,
                    Display = document.Name,
                    IsChecked = false,
                    DocumentToDelete = false
                });
            }

            viewModel.CheckBoxDocuments = checkBoxListDocuments;

            var currentUser = GetCurrentUser();
            GroupRight usersGroupRights;
            Group group = db.Groups.Find(id);
            if (currentUser.GroupRights.Any(x => x.GroupName == group.Name))
            {
                usersGroupRights = currentUser.GroupRights.Where(x => x.GroupName == group.Name).Single();
            }
            else
            {
                usersGroupRights = new GroupRight();
            }
            //GroupRight usersGroupRights = currentUser.GroupRights.Where(x => x.GroupName == group.Name).Single();
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Group group = db.Groups.Find(id);
            //if (group == null)
            //{
            //    return HttpNotFound();
            //}
            //if (currentUser.GroupRights.Any(x => x.GroupName == group.Name || x.CanEdit == true))
            //{
            //    return View(viewModel);
            //}

            if (usersGroupRights.CanEdit == true)
            {
                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }

            //return View(viewModel);
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

        public ActionResult MyGroups()
        {
            return View(repo.GetUserGroups(GetCurrentUser()));
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            repo.ArchiveDocument(document);
            //db.Documents.Remove(document);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateFile(CreateDocumentViewModel viewModel, HttpPostedFileBase upload)
        {
            var file = repo.CreateFile(upload, GetCurrentUser());
            viewModel.Files.Add(file);
            TempData["viewModel"] = viewModel;

            //return RedirectToAction("CreateDocument", file);
            return RedirectToAction("CreateDocument", new { files = viewModel.Files });
            
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
