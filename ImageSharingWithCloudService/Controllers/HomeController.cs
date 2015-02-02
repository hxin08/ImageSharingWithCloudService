using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ImageSharingWithCloudService.Models;

namespace ImageSharingWithCloudService.Controllers
{
    public class HomeController : BaseController
    {

        //
        // GET: /Home/

        [AllowAnonymous]
        public ActionResult Index(String id = "Stranger")
        {
            CheckAda();
            ViewBag.Title = "Welcome!";
            ApplicationUser user = GetLoggedInUser();
            if (user == null)
            {
                ViewBag.Id = id;
            }
            else
            {
                ViewBag.Id = user.UserName;
            }
            return View();
        }

        public ActionResult Error(String errid = "Unspecified")
        {
            if ("Details".Equals(errid))
            {
                ViewBag.Message = "Problem with Details action!";

            }
            else
            {
                ViewBag.Message = "Unspecified error!";
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
