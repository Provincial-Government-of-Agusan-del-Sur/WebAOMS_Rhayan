using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
namespace WebAOMS.Controllers
{
    public class TelegramBotController : Controller
    {
        // GET: TelegramBot
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult sendTelegram() {
            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            string apiToken = "my_bot_api_token";
            string chatId = "@my_channel_name";
            string text = "Hello world!";
            urlString = String.Format(urlString, apiToken, chatId, text);
            WebRequest request = WebRequest.Create(urlString);
            Stream rs = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(rs);
            string line = "";
            StringBuilder sb = new StringBuilder();
            while (line != null)
            {
                line = reader.ReadLine();
                if (line != null)
                    sb.Append(line);
            }
            string response = sb.ToString();
            // Do what you want with response
            return Content(response);
        }
    }
}