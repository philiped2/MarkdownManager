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


        public ActionResult Index(string message)
        {
            //List<Document> ArchivedDocumentsToDelete = repo.DeleteOldArchivedDocuments();
            //foreach (Document)

            repo.DeleteOldArchivedDocuments();

            ViewBag.message = "";
            AllDocumentsViewModel documentViewmodel = new AllDocumentsViewModel();
            documentViewmodel.CurrentUser = GetCurrentUser();
            documentViewmodel.Documents = repo.GetAuthorisedUserDocuments(GetCurrentUser());

            List<int> usersGroupDocumentRightsById = new List<int>();
            documentViewmodel.DocumentWithEditRightsById = usersGroupDocumentRightsById;
            documentViewmodel.DocumentWithEditRightsById = repo.ListUsersGroupDocumentRights(GetCurrentUser(), usersGroupDocumentRightsById);

            return View(documentViewmodel);
            //return View(repo.GetUserDocuments(GetCurrentUser())); senaste använda
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
            return RedirectToAction("Index", repo.GetAuthorisedUserDocuments(GetCurrentUser()));
        }

        public ActionResult GetUsersJson(string keyword)
        {
            var currentUserID = GetCurrentUser().Id;
            var result = repo.GetUsersByName(keyword, currentUserID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAuthGroupsJson(string keyword)
        {
            var currentUser = GetCurrentUser();
            var result = repo.GetAuthGroupsByName(keyword, currentUser);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTagsJson (string tagLabel, List<string> selectedTags)
        {
            var result = repo.GetTagsByName(tagLabel);
            if (selectedTags != null)
            {
                foreach (string tag in selectedTags)
                {
                    result.Remove(tag);
                }
            }

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
        public ActionResult CreateGroup2()
        {
            return View(new Group());
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

        public ActionResult GetDocumentFormDataJson(int ID)
        {
            var documentToGet = repo.GetDocumentById(ID, GetCurrentUser()); //Also checks if user is auth to get document
            var userDocumentRights = repo.getDocumentUserDocumentRights(ID);
            var groupDocumentRights = repo.getDocumentGroupDocumentRights(ID);
            if (documentToGet == null)
            {
                return View(new { message = "Ett problem uppstod när dokumentet skulle hämtas. Du kan sakna rättigheter för åtgärden."});
            }
            else //description  markdown tags users,  groups
            {
                return Json(new { name = documentToGet.Name, description = documentToGet.Description, markdown = documentToGet.Markdown, users = userDocumentRights, groups = groupDocumentRights, tags = documentToGet.Tags.Select(t => t.Label) }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult CreateDocumentJson(string name, string description, string markdown, List<string> tags, List<UserListModel> users, List<GroupListModel> groups)
        {
            repo.CreateDocument2( name, description, markdown, tags, users, groups, GetCurrentUser());
            
            return Json(new { message = "Document created!"});
        }

        [HttpPost]
        public ActionResult EditDocumentJson(int Id, string name, string description, string markdown, List<string> tags, List<UserListModel> users, List<GroupListModel> groups)
        {
            repo.EditDocument(Id, name, description, markdown, tags, users, groups, GetCurrentUser());

            return Json(new { message = "Document created!" });
        }

        [HttpPost]
        public ActionResult CreateGroupJson(string name, string description, List<UserListModel> users)
        {
            repo.CreateGroup(name, description, users, GetCurrentUser());
            return Json(new { message = "Group created!" });
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult CreateDocument(CreateDocumentViewModel viewModel, HttpPostedFileBase upload, string[] selectedFiles)
        //{
        //    //dynamic fileArray = JsonConvert.DeserializeObject(viewModel.FilesJson);

        //    //foreach (var file in fileArray)
        //    //{
        //    //    Console.WriteLine(file.Filename); 
        //    //}

        //    if (Request.Form["CreateFile"] != null)
        //    {
        //        var file = repo.CreateFile(upload, GetCurrentUser());
        //        viewModel.Files.Add(file);
        //        TempData["viewModel"] = viewModel;
                
        //        //return RedirectToAction("CreateDocument", file);
        //        return RedirectToAction("CreateDocument", new { files = viewModel.Files });
        //    }
        //    else if (Request.Form["CreateDocument"] != null)
        //    {
        //        //List<File> fileList = repo.CreateFileListFromJson(viewModel.FilesJson, GetCurrentUser());

        //        var user = GetCurrentUser();
        //        repo.CreateDocument(viewModel, user);
        //        return RedirectToAction("Index", repo.GetUserDocuments(GetCurrentUser()));
        //    }

        //    else
        //    {
        //        return View("Index");
        //    }


        //}


        //[HttpPost]
        //public ActionResult CreateGroup(CreateGroupViewModel viewModel)
        //{
        //    var user = GetCurrentUser();
        //    repo.CreateGroup(viewModel, user);
        //    ViewBag.Test = "A new group has been created!";
        //    return View("Index", repo.GetUserDocuments(GetCurrentUser()));
            //}


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

            List<Document> tempDocumentList = repo.GetAuthorisedUserDocuments(GetCurrentUser());
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

        //[HttpPost]
        //public ActionResult NewCreateGroup(CreateGroupViewModel viewModel)
        //{
        //    var user = GetCurrentUser();
        //    repo.CreateGroup(viewModel, user);
        //    ViewBag.Test = "A new group has been created!";
        //    return View("Index", repo.GetUserDocuments(GetCurrentUser()));
        //}

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

            List<Document> tempDocumentList = repo.GetAuthorisedUserDocuments(GetCurrentUser());
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
                //(Model.CurrentUser.UserDocumentRights.Any(x => x.document.ID == item.ID && x.CanWrite == true) || User.IsInRole("Admin"))
            else if (currentUser.UserDocumentRights.Any(x => x.document.ID == document.ID && x.CanWrite == true) || db.UserGroupRights.Any(x => x.UserId == currentUser.Id &&
                        db.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                            g.DocumentId == id)))
            {
                //return View(document);
                return View(new Document() { ID = id.Value });
            }

            else
            {
                return HttpNotFound();
            }
            //return View(document);
        }


        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Description,Name,Markdown,DateCreated,LastChanged,CreatorID")] Document document)
        //{
        //    ApplicationUser currentUser = GetCurrentUser();

        //    if (ModelState.IsValid)
        //    {
        //        repo.LogDocumentChanges(document, currentUser);
        //        db.Entry(document).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
            
        //    return View(document);
        //}
            
        ////[HttpPost]
        ////public ActionResult EditGroup(EditGroupViewModel viewModel)
        ////{
        ////    //var user = GetCurrentUser();
        ////    repo.EditGroup2(viewModel);
        ////    //return View("Index", repo.GetUserDocuments(GetCurrentUser()));
        ////    return View("Index");
        ////}

        //public ActionResult EditGroup(int? id)
        //{
        //    EditGroupViewModel viewModel = new EditGroupViewModel();
        //    List<ApplicationUser> tempUserList = repo.ListUsersToCreateGroup();
        //    var checkBoxListItems = new List<CheckBoxListUser>();
        //    Group groupToEdit = db.Groups.Where(x => x.ID == id).Single();
        //    viewModel.GroupToEdit = groupToEdit;
        //    foreach (var user in tempUserList)
        //    {
        //        checkBoxListItems.Add(new CheckBoxListUser()
        //        {
        //            ID = user.Id,
        //            Display = user.LastName + "," + user.FirstName,
        //            IsChecked = false,
        //            UserToDelete = false,

        //            CanEdit = false,
        //            IsGroupAdmin = false
        //        });
        //    }
        //    viewModel.CheckBoxUsers = checkBoxListItems;

        //    List<Document> tempDocumentList = repo.GetUserDocuments(GetCurrentUser());
        //    var checkBoxListDocuments = new List<CheckBoxListDocuments>();

        //    foreach (var document in tempDocumentList)
        //    {
        //        checkBoxListDocuments.Add(new CheckBoxListDocuments()
        //        {
        //            ID = document.ID,
        //            Display = document.Name,
        //            IsChecked = false,
        //            DocumentToDelete = false
        //        });
        //    }

        //    viewModel.CheckBoxDocuments = checkBoxListDocuments;

        //    var currentUser = GetCurrentUser();
        //    GroupRight usersGroupRights;
        //    Group group = db.Groups.Find(id);
        //    if (currentUser.GroupRights.Any(x => x.GroupId == group.ID))
        //    {
        //        usersGroupRights = currentUser.GroupRights.Where(x => x.GroupId == group.ID).Single();
        //    }
        //    else
        //    {
        //        usersGroupRights = new GroupRight();
        //    }
        //    //GroupRight usersGroupRights = currentUser.GroupRights.Where(x => x.GroupName == group.Name).Single();
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //Group group = db.Groups.Find(id);
        //    //if (group == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //if (currentUser.GroupRights.Any(x => x.GroupName == group.Name || x.CanEdit == true))
        //    //{
        //    //    return View(viewModel);
        //    //}

        //    //if (usersGroupRights.IsGroupAdmin == true) // senaste funktionen för att begränsa rättigheter innan disable button skapades i viewn
        //    //{
        //    //    return View(viewModel);
        //    //}
        //    //else
        //    //{
        //    //    return HttpNotFound();
        //    //}

            //    return View(viewModel);
            //}




        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            ApplicationUser currentUser = GetCurrentUser();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            else if (currentUser.UserDocumentRights.Any(x => x.document.ID == document.ID && x.CanWrite == true) || db.UserGroupRights.Any(x => x.UserId == currentUser.Id &&
                        db.GroupDocumentRights.Any(g => g.GroupId == x.GroupId &&
                            g.DocumentId == id)))
            {
                return View(document);
            }

            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult MyGroups()
        {
            repo.GetUserGroups(GetCurrentUser());
            ViewGroupsViewModel ViewGroupViewModel = new ViewGroupsViewModel();
            ViewGroupViewModel.CurrentUser = GetCurrentUser();
            ViewGroupViewModel.UsersGroups = repo.GetUserGroups(GetCurrentUser());

            return View(ViewGroupViewModel);
            //return View(repo.GetUserGroups(GetCurrentUser()));
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            repo.ArchiveDocument(document); // arkiverar dokument
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
