using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebAOMS.wsifmis;
using System.Data;
namespace Transfinder.Controllers
{
    public class SController : Controller
    {
        WebServiceSoapClient ws = new WebServiceSoapClient();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult result(string dvno) {

            if (dvno == null) {
                dvno = "000-00-00-0000";
            }
            System.Net.ServicePointManager.Expect100Continue = false;
            DataTable rec = ws.getTransactions(dvno.Replace("'","''").Replace("--","")).Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
                ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
                ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
                ViewBag.dvno = dvno;
            }
            return View("result", ws.getTransactions(dvno.Replace("'", "''").Replace("--", "")));
        }
        public ActionResult r(Int64 b = 0)
        {
            //DataTable rec = new DataTable();
            //Service1Client client = new Service1Client();
            //rec = client.GetTransDetails(dvno);
         
            System.Net.ServicePointManager.Expect100Continue = false;
            DataTable rec = ws.getTransactions_byBatchno(b).Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
                ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
                ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
                ViewBag.dvno = rec.Rows[0]["dvno"].ToString();
                ViewBag.batchno = b;
            }
            return View("r", ws.getTransactions_byBatchno(b));
        }
        public ActionResult _r(Int64 b)
        {
            //DataTable rec = new DataTable();
            //Service1Client client = new Service1Client();
            //rec = client.GetTransDetails(dvno);
       
            System.Net.ServicePointManager.Expect100Continue = false;
            DataTable rec = ws.getTransactions_byBatchno(b).Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
                ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
                ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
                ViewBag.dvno = rec.Rows[0]["dvno"].ToString();
                ViewBag.batchno = b;
            }
            return PartialView("_r", ws.getTransactions_byBatchno(b));
        }
        public ActionResult _result(string dvno)
        {
            //DataTable rec = new DataTable();
            //Service1Client client = new Service1Client();
            //rec = client.GetTransDetails(dvno);
            if (dvno == null)
            {
                dvno = "000-00-00-0000";
            }
            System.Net.ServicePointManager.Expect100Continue = false;
            DataTable rec = ws.getTransactions(dvno.Replace("'", "''").Replace("--", "")).Tables[0];
            if (rec.Rows.Count > 0)
            {
                ViewBag.claimantname = rec.Rows[0]["claimantname"].ToString();
                ViewBag.Particular = rec.Rows[0]["Particular"].ToString();
                ViewBag.GAmount = rec.Rows[0]["GAmount"].ToString();
                ViewBag.dvno = dvno;
            }
            return PartialView("_result", ws.getTransactions(dvno.Replace("'", "''").Replace("--", "")));
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}