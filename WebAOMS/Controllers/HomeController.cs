using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebAOMS.Models;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using WebAOMS.Mod;
namespace WebAOMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Guest") == true)
            {
                ViewBag.Title = "Guest";
                ViewBag.Title_mini = "Guest";
                ViewBag.rightSidebar_title = "AOMS";
                ViewBag.menuid = "a60";
                return View("guest");
            }
            else
            {
                ViewBag.Title = "Home";
                ViewBag.Title_mini = "Home";
                ViewBag.rightSidebar_title = "AOMS";
            }
            return View();
        }

        public ActionResult guest()
        {
            ViewBag.Title = "Guest";
            ViewBag.Title_mini = "Guest";
            ViewBag.rightSidebar_title = "AOMS";
            ViewBag.menuid = "a60";
            return View();
        }

        //public void SaveImage(string filename, ImageFormat format)
        //{

        //    using (WebClient webClient = new WebClient())
        //    {
        //        byte[] data = webClient.DownloadData(filename);

        //        using (MemoryStream mem = new MemoryStream(data))
        //        {
        //            using (var yourImage = Image.FromStream(mem))
        //            {
        //                // If you want it as Png
        //                yourImage.Save("path_to_your_file.png", ImageFormat.Png);

        //                // If you want it as Jpeg
        //                //yourImage.Save("path_to_your_file.jpg", ImageFormat.Jpeg);
        //            }
        //            //mem.WriteTo(Server.MapPath("Content/UserImage"));
        //        }


        //    }
        //}
        //public ActionResult _rigthsidebar() {
        //    return View();
        //}

        //public void SaveImage(string filename, ImageFormat format)
        //{

        //    using (WebClient webClient = new WebClient())
        //    {
        //        byte[] data = webClient.DownloadData(filename);

        //        using (MemoryStream mem = new MemoryStream(data))
        //        {
        //            using (var yourImage = Image.FromStream(mem))
        //            {
        //                // If you want it as Png
        //                yourImage.Save("path_to_your_file.png", ImageFormat.Png);

        //                // If you want it as Jpeg
        //                //yourImage.Save("path_to_your_file.jpg", ImageFormat.Jpeg);
        //            }
        //            //mem.WriteTo(Server.MapPath("Content/UserImage"));
        //        }


        //    }
        //}
        //public ActionResult _rigthsidebar() {
        //    return View();
        //}
        public ActionResult About()
        {
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.userID = currentUser.UserID;

            

            

            ViewBag.Message = User.Identity.GetUserId().ToString() + "Your application description page.";

            return View();
        }

        [AuthorizeAdminOrOwnerOfPost]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}