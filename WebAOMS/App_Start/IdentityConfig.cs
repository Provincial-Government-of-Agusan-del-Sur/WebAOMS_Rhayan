using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebAOMS.Models;
using WebAOMS.wsifmis;
using System.Net.Mail;
using System.Net;
namespace WebAOMS
{
    public class EmailService : IIdentityMessageService
    {
        //public Task SendAsync(IdentityMessage message)
        //{
        //    // Plug in your email service here to send an email.
        //    MailMessage mgs = new MailMessage();
        //    mgs.From = new MailAddress("aoms.support@pgas.ph", "aoms support");

        //    mgs.To.Add(new MailAddress(message.Destination));

        //    mgs.Subject = message.Subject;
        //    mgs.Body = message.Body;
        //    mgs.IsBodyHtml = true;

        //    SmtpClient client = new SmtpClient();
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.Host = "mail.pgas.ph";
        //    client.EnableSsl = true;
        //    //client.UseDefaultCredentials = false;

        //    client.Credentials = new System.Net.NetworkCredential("aoms.support@pgas.ph", "Qwer123488*");
        //    client.Send(mgs);

        //    return Task.FromResult(0);
        //}
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.

            using (MailMessage mail = new MailMessage())
            {
                string emailServer = "mail.pgas.ph";

                mail.From = new MailAddress("aoms.support@pgas.ph", "AOMS Support");
                mail.To.Add(new MailAddress(message.Destination));
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true; // Set to true if you are sending HTML email

                using (SmtpClient smtp = new SmtpClient(emailServer))
                {
                    //error fix for "A call to SSPI failed, function requested not supported"
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //end error fx
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("aoms.support@pgas.ph", "Qwer123488*");
                    smtp.Send(mail);
                }
            }
            return Task.FromResult(0);
        }
     
    }

    public class SmsService : IIdentityMessageService     
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            //SMSWebServiceSoapClient sms = new SMSWebServiceSoapClient();
            //sms.SEND_SMS(message.Destination, message.Body, 0);

           WebServiceSoapClient sms = new wsifmis.WebServiceSoapClient();
            sms.send_sms(message.Destination, message.Body,7,1);

            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static object UserManager { get; internal set; }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(2);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your AOMS security code is {0}"
            });

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser> 
            {
                Subject = "AOMS Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
