using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAOMS.Models;
namespace WebAOMS.Controllers
{
    public class CommunicationController : Controller
    {
        // GET: Communication
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Create()
        //{
        //    var model = new NoticeViewModel
        //    {
        //        SignatoryList = db.tbl_l_Signatories
        //            .Select(x => new SelectListItem
        //            {
        //                Value = x.SignatoryID.ToString(),
        //                Text = x.SignatoryName
        //            }).ToList()
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Create(NoticeViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Save to DB
        //        // Redirect or show success message
        //    }

        //    // Reload signatory list if validation fails
        //    model.SignatoryList = db.tbl_l_Signatories
        //        .Select(x => new SelectListItem
        //        {
        //            Value = x.SignatoryID.ToString(),
        //            Text = x.SignatoryName
        //        }).ToList();

        //    return View(model);
        //}

    }
}