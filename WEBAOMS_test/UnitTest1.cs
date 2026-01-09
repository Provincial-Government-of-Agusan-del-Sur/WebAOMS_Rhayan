using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WEBAOMS_test.epsws;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
namespace WEBAOMS_test
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        //public static void Main()
        //{
        //    // trust sender
        //    System.Net.ServicePointManager.ServerCertificateValidationCallback
        //                    = ((sender, cert, chain, errors) => cert.Subject.Contains("https://192.168.101.56/epsws/service.asmx"));

        //    epsws.serviceSoapClient eps = new serviceSoapClient();
        //    DataTable rec = eps.PODetails("9223123");
        //    foreach (DataRow rw in rec.Rows)
        //    {
        //        Console.WriteLine(rw[0].ToString());
        //    }
        //}
        //static void sMain1(string[] args)
        //{
        //    Console.Title = "A simple speed test connection for your app";

        //    // the URL to download a file from
        //    Uri URL = new Uri(
        //    "http://puresourcecode.com/file.axd?file=/SpeedTest/1024kb.txt"
        //    );
        //    WebClient wc = new WebClient();

        //    Console.WriteLine("Simple speedtest");
        //    Console.WriteLine("----------------");
        //    Console.WriteLine("Will test your download rate. " +
        //                      "Press any key to begin.");
        //    Console.ReadKey();

        //    Console.WriteLine("\nDownloading file: 1024kb.txt...");
        //    Console.WriteLine("From http://puresourcecode.com");
        //    Console.WriteLine("Note: This file will automatically " +
        //                      "be deleted after the test.");

        //    // get current tickcount 
        //    double starttime = Environment.TickCount;

        //    // download file from the specified URL, 
        //    // and save it to C:\speedtest.txt
        //    // in your project change the path of the following line
        //    wc.DownloadFile(URL, @"C:\speedtest.txt");

        //    // get current tickcount
        //    double endtime = Environment.TickCount;

        //    // how many seconds did it take?
        //    // we are calculating this by subtracting starttime from
        //    // endtime and dividing by 1000 (since the tickcount is in 
        //    // miliseconds 1000 ms = 1 sec)
        //    double secs = Math.Floor(endtime - starttime) / 1000;

        //    // calculate download rate in kb per sec.
        //    // this is done by dividing 1024 by the number of seconds it
        //    // took to download the file (1024 bytes = 1 kilobyte)
        //    double kbsec = Math.Round(1024 / secs);

        //    Console.WriteLine("\nCompleted. Statistics:\n");

        //    Console.WriteLine("1mb download: \t{0} secs", secs);
        //    Console.WriteLine("Download rate: \t{0} kb/sec", kbsec);

        //    Console.WriteLine("\nPress any key to exit...");
        //    Console.Read();
        //    Console.WriteLine("Deleting file...");
        //    try
        //    {
        //        // delete downloaded file
        //        System.IO.File.Delete(@"C:\speedtest.txt");
        //        Console.WriteLine("Done.");
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Couldn't delete download file.");
        //        Console.WriteLine("To delete the file yourself.");
        //        Console.ReadKey();
        //    }
        //}
        private static void loadlist()
        {
            //string str = "1,234,46,78,5,8,4,9";
            //List<string> supplierid_list = str.Split(',').ToList();
            //for (var i = 0; i < supplierid_list.Count; i++)
            //{
            //    Console.WriteLine("Amount is {0} and type is {1}", supplierid_list[i], supplierid_list[i]);
            //}

        }  
        private static void SendEmail()
        {
            
            string subject = "Test Email";
            string body = "This is a test email.";

            using (MailMessage mail = new MailMessage())
            {
                string emailServer = "mail.pgas.ph";

                mail.From = new MailAddress("aoms.support@pgas.ph", "AOMS Support");
                mail.To.Add(new MailAddress("marpaul.ajero@pgas.ph"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false; // Set to true if you are sending HTML email
                Console.WriteLine("Sending messages, please wait...!");
                using (SmtpClient smtp = new SmtpClient(emailServer))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("aoms.support@pgas.ph", "Qwer123488*");

                  
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully.");
             
                }
            }

        }
        private static void SendEmailToGmail()
        {
            var fromAddress = new MailAddress("aheroinfotech@gmail.com", "ahero infotect");
            var toAddress = new MailAddress("onerrorgotopaul@gmai.com", "Mar Paul");
            const string fromPassword = "fromPassword";
            const string subject = "Subject";
            const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);

            }
        }
        static void Main(string[] args)
        {

            SendEmailToGmail();
            //string apiToken = "5275020740:AAGB7sYWxOZzHui4EjXyUVXbZlMf3jCA8LE";
            //string chatId = "@Ahero_IT_Services_bot";
            //string text = "Hello world!";
            ////var bot = new Api(apiToken);
            ////var t = await bot.SendTextMessage("@channelname or chat_id", "text message");


            //var botClient = new Telegram.Bot.TelegramBotClient(apiToken);
            //var t = botClient.SendTextMessageAsync(chatId,text);
            // //Console.WriteLine( t.Result.ToString());
            ////var me =  botClient.GetMeAsync().Result;

            ////var bot = new Telegram.Bot.TelegramBotClient(apiToken);
            ////var result = bot.SendTextMessageAsync(chatId, text);
            //////urlString = String.Format(urlString, apiToken, chatId, text);

            //////WebRequest request = WebRequest.Create(urlString);
            //////Stream rs = request.GetResponse().GetResponseStream();
            //////StreamReader reader = new StreamReader(rs);
            //////string line = "";
            //////StringBuilder sb = new StringBuilder();
            //////while (line != null)
            //////{
            //////    line = reader.ReadLine();
            //////    if (line != null)
            //////        sb.Append(line);
            //////}
            //////string response = sb.ToString();


            ////Message message = await botClient.SendTextMessageAsync(
            ////    chatId: chatId,
            ////    text: "Trying *all the parameters* of `sendMessage` method",
            ////    parseMode: ParseMode.MarkdownV2,
            ////    disableNotification: true,
            ////    replyToMessageId: update.Message.MessageId,
            ////    replyMarkup: new InlineKeyboardMarkup(
            ////        InlineKeyboardButton.WithUrl(
            ////            "Check sendMessage method",
            ////            "https://core.telegram.org/bots/api#sendmessage")),
            ////cancellationToken: cancellationToken);
            //// Do what you want with response

        }


    }
}
