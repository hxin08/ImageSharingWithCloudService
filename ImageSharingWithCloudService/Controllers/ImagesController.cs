using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.DAL;
using ImageSharingWithCloudService.Queues;

using System.Data;

namespace ImageSharingWithCloudService.Controllers
{
    public class ImagesController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [RequireHttps]
        public ActionResult Upload()
        {
            CheckAda();
            ViewBag.Message = "";
            SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttps]
        public ActionResult Upload(ImageView image,
                                   HttpPostedFileBase ImageFile)
        {
            CheckAda();

            TryUpdateModel(image);


            if (ModelState.IsValid)
            {
                // HttpCookie cookie = Request.Cookies.Get("ImageSharing");
                ApplicationUser userid = GetLoggedInUser();

                if (userid != null)
                {
                    ApplicationUser user = db.Users.SingleOrDefault(u => u.Id.Equals(userid.Id));
                    if (user != null)
                    {

                        //save image information in the database.

                        Image imageEntity = new Image();
                        imageEntity.Caption = image.Caption;
                        imageEntity.Description = image.Description;
                        imageEntity.DateTaken = image.DateTaken;
                        imageEntity.UserId = user.UserName;
                        imageEntity.Approved = false;
                        imageEntity.Valid = false;
                        imageEntity.TagId = image.TagId;
                        if (ImageFile != null && ImageFile.ContentLength > 0 && ImageFile.ContentType.Equals("image/jpeg"))
                        {


                            db.Images.Add(imageEntity);
                            db.SaveChanges();

                            ImageStorage.SaveFile(Server, ImageFile, imageEntity.Id);
                            QueueConnector.SendToQueue(imageEntity);

                            // image.Id = imageEntity.Id;
                            //return View("Details", image);
                            return RedirectToAction("Details", new { Id = imageEntity.Id });


                        }
                        else
                        {
                            ViewBag.Message = "No image file specified!";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "No such userid registered!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "No such userid registered!";
                    return View();
                }

            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form!";
                return View();
            }

        }

        [HttpGet]
        public ActionResult Query()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            CheckAda();

            {
                Image imageEntity = db.Images.Find(Id);
                if (imageEntity != null)
                {
                    ImageView imageView = new ImageView();
                    imageView.Id = imageEntity.Id;
                    imageView.Uri = ImageStorage.ImageURI(Url, imageEntity.Id);
                    imageView.Caption = imageEntity.Caption;
                    imageView.Description = imageEntity.Description;
                    imageView.DateTaken = imageEntity.DateTaken;
                    imageView.TagName = imageEntity.Tag.Name;
                    imageView.Userid = imageEntity.UserId;
                    
                    LogContext.AddLogEntry(GetLoggedInUser(), imageView);
                    
                    return View(imageView);
                }
                else
                {

                    return RedirectToAction("Error", "Home", new { errid = "Details" });
                }
            }

        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            CheckAda();
            Image imageEntity = db.Images.Find(Id);
            if (imageEntity != null)
            {

                ApplicationUser userid = GetLoggedInUser();
                if (imageEntity.UserId.Equals(userid.Id))
                {

                    ViewBag.Message = "";
                    ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", imageEntity.TagId);
                    ImageView image = new ImageView();
                    image.Id = imageEntity.Id;
                    image.Uri = ImageStorage.ImageURI(Url, imageEntity.Id);
                    image.TagId = imageEntity.Id;
                    image.Caption = imageEntity.Caption;
                    image.Description = imageEntity.Description;
                    image.DateTaken = imageEntity.DateTaken;
                    return View("Edit", image);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                }
            }
            else
            {

                return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageView image)
        {
            CheckAda();
            ApplicationUser userid = GetLoggedInUser();

            if (ModelState.IsValid)
            {
                Image imageEntity = db.Images.Find(image.Uri);
                if (imageEntity != null)
                {
                    if (imageEntity.UserId.Equals(userid.Id))
                    {
                        imageEntity.TagId = image.TagId;
                        imageEntity.Caption = image.Caption;
                        imageEntity.Description = image.Description;
                        imageEntity.DateTaken = image.DateTaken;
                        db.Entry(imageEntity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { Id = image.Uri });
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
                }
            }
            else
            {
                return View("Edit", image);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            CheckAda();
            ApplicationUser userid = GetLoggedInUser();
            Image imageEntity = db.Images.Find(Id);
            if (imageEntity != null)
            {

                if (imageEntity.UserId.Equals(userid.Id))
                {
                    //db.Entry(imageEntity).State = EntityState.Deleted;
                    db.Images.Remove(imageEntity);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "DeleteNotAuth" });
                }
            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "DeleteNotFound" });
            }
        }

        [HttpGet]
        public ActionResult ListAll()
        {
            CheckAda();
            IEnumerable<Image> images = ApprovedImages().ToList();

            ApplicationUser user = GetLoggedInUser();
            if (user != null)
            {
                ViewBag.Userid = user.UserName;
                return View(images);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpGet]
        public ActionResult ListByUser()
        {
            CheckAda();

            SelectList users = new SelectList(db.Users, "Id", "Userid", 1);

            return View(users);
        }

        [HttpGet]
        // [ValidateAntiForgeryToken]
        public ActionResult DoListByUser(int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser().UserName;
            ApplicationUser user = db.Users.Find(Id);
            if (user != null)
            {
                ViewBag.Userid = userid;

                return View("ListAll", ApprovedImages(user.Images).ToList());
            }
            else
            {

                return RedirectToAction("Error", "Home", new { erid = "ListByUser" });

            }
        }

        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);

            return View(tags);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult DoListByTag(int Id)
        {
            CheckAda();
            ApplicationUser user = GetLoggedInUser();
            if (user != null)
            {
               
                Tag tag = db.Tags.Find(Id);

                if (tag != null)
                {
                    ViewBag.Userid = user.UserName;

                    return View("ListAll", ApprovedImages(tag.Images).ToList());
                }
                else
                {

                    return RedirectToAction("Error", "Home", new { erid = "ListByTag" });

                }
            }
            else
            {
                return RedirectToAction("Error", "Home", new { erid = "ListByTag" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve()
        {
            CheckAda();
            ViewBag.Message = "";
            var db = new ApplicationDbContext();
            List<SelectItemView> model = new List<SelectItemView>();
            foreach (var u in db.Images)
            {
                if (u.Valid)
                {
                    if (!u.Approved)
                    {
                        model.Add(new SelectItemView(u.Id, u.Caption, u.Approved));

                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve(List<SelectItemView> model)
        {

            CheckAda();
            var db = new ApplicationDbContext();
            foreach (var imod in model)
            {

                Image image = db.Images.Find(imod.Id);
                if (imod.Checked)
                {
                    image.Approved = true;
                    QueueManager.SendImageEntityToQueue(image);
                }

                imod.Name = image.Caption;

            }

            db.SaveChanges();
            ViewBag.Message = "Images approved.";


            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public ActionResult ImageViews()
        {
            CheckAda();
            IEnumerable<LogEntry> entries = LogContext.Select();
            return View(entries);
        }
        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public ActionResult QueueMessages()
        {
            CheckAda();
            List<QueueMessage> messages = QueueManager.ReadFromQueue();
            
            return View(messages);
        }

    }
}
