using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAOMS.Controllers
{
    public class MoneyManagerController : Controller
    {
        // GET: MoneyManager
        [Authorize(Roles ="MoneyManager,Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}