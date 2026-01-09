using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAOMS.Models;
using System.IO;
using WebAOMS.Base;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;

namespace WebAOMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private fmisEntities fmisdb = new fmisEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //public string getUserID() {
        //    var currentUserId = User.Identity.GetUserId();
        //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    var currentUser = manager.FindById(User.Identity.GetUserId());
        //    string userid = currentUser.UserID.ToString();
        //    return userid;
        //}
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, int? ISModule)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            string query = @"SELECT TOP 1 [IS_name], [IS_name_abbr], [IS_name_account]
                     FROM [fmis].[Accounting].[tbl_l_ISModule]
                     WHERE IS_Module = @ISModule";

            using (SqlConnection conn = new SqlConnection(fmisConn))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ISModule", ISModule ?? 0);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ViewBag.IS_name = reader["IS_name"].ToString();
                    ViewBag.IS_name_abbr = reader["IS_name_abbr"].ToString();
                    ViewBag.IS_name_account = reader["IS_name_account"].ToString();
                }
                else
                {
                    ViewBag.IS_name = "Accounting Operation Management System";
                    ViewBag.IS_name_abbr = "AOMS";
                    ViewBag.IS_name_account = " with your AOMS Account";
                }
            }

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //PGAS

           model.Email = model.Email.Replace("@pgas.gov", "").Replace("@pgas.ph", "");
            model.Email = model.Email.Replace("@pgzn.gov.ph", "").Replace("@pgzn.ph", "");

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ModelState.AddModelError("", "You need to confirm your email at zimbra.");
                    return View(model);
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    USER.Set(user.Id, 1);
                    IEnumerable<w_UserMenu> _menu = fmisdb.w_UserMenu.Where(M => M.userid_ID == user.Id).OrderBy(o=>o.Ordering);
                    USERMENU.SetUserMenu(_menu, user.Id);
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    USER.Set(user.Id, 1);
                    IEnumerable<w_UserMenu> _menux = fmisdb.w_UserMenu.Where(M => M.userid_ID == user.Id).OrderBy(o => o.Ordering);
                    USERMENU.SetUserMenu(_menux, user.Id);
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Register_outsidePGAS()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Confirm_email_to_zimbra()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterHRISUser()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterHRISUser(RegisterByPgasViewModel model)
        {
            //if (model.Email.Contains("@pgas.ph") != true)
            //{
            //    model.Email = model.Email + "@pgas.ph";
            //}
            //if (model.Email.Contains("@pgzn.ph") != true)
            //{
            //    model.Email = model.Email + "@pgzn.gov.ph";
            //}

            fmisEntities fmisdb = new fmisEntities();
            if (ModelState.IsValid)
            {

                var pgasUser = fmisdb.ufn_getPgasUserByEmail(model.Email).FirstOrDefault();
                if ((object) pgasUser == null)
                {
                    ModelState.AddModelError("", "Email Address are not registered on HRIS");
                }
                else
                { 
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserID = Convert.ToInt32(pgasUser.SwipEmployeeID)};

                    var result = await UserManager.CreateAsync(user, pgasUser.passcode);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                        StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/ConfirmEmail.html"));
                        string s = sr.ReadToEnd();
                        sr.Close();
                        //Send an email with this link
                        ISfn.Addroles(Convert.ToInt32(pgasUser.SwipEmployeeID), 9);
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                       await UserManager.SendEmailAsync(user.Id, "Confirm your account", s.Replace("{{action_url}}", callbackUrl).Replace("{{name}}", model.Email).Replace("[Product Name]", "AOMS"));

                        ISfn.AddUserMenu(Convert.ToInt16(pgasUser.SwipEmployeeID), 5);
                        return RedirectToAction("Confirm_email_to_zimbra", "Account");
                    }
                    AddErrors(result);
                }
                
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public async Task<ActionResult> RegisterDGSignUser(RegisterByPgasViewModel model)
        {


            fmisEntities fmisdb = new fmisEntities();
            if (ModelState.IsValid)
            {

                var pgasUser = fmisdb.ufn_getPgasUserByEmail(model.Email).FirstOrDefault();
                if ((object)pgasUser == null)
                {
                    ModelState.AddModelError("", "Email Address are not registered on HRIS");
                }
                else
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserID = Convert.ToInt16(pgasUser.SwipEmployeeID) };

                    var result = await UserManager.CreateAsync(user, pgasUser.passcode);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                        StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/ConfirmEmail.html"));
                        string s = sr.ReadToEnd();
                        sr.Close();
                        //Send an email with this link
                        ISfn.Addroles(Convert.ToInt32(pgasUser.SwipEmployeeID), 9);
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", s.Replace("{{action_url}}", callbackUrl).Replace("{{name}}", model.Email).Replace("[Product Name]", "AOMS"));

                        //ISfn.AddUserMenu(Convert.ToInt16(pgasUser.SwipEmployeeID), 5);
                        return RedirectToAction("Confirm_email_to_zimbra", "Account");
                    }
                    AddErrors(result);
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register_outsidePGAS(RegisterByPgasViewModel model)
        {
            if (model.Email.Contains("@pgas.ph") != true)
            {
                model.Email = model.Email + "@pgas.ph";
            }

            fmisEntities fmisdb = new fmisEntities();
            if (ModelState.IsValid)
            {

 
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserID = Convert.ToInt16(model.UserID) };

                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                        StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/ConfirmEmail.html"));
                        string s = sr.ReadToEnd();
                        sr.Close();
                        //Send an email with this link
                        ISfn.Addroles(Convert.ToInt16(model.UserID), 9);
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", s.Replace("{{action_url}}", callbackUrl).Replace("{{name}}", model.Email).Replace("[Product Name]", "AOMS"));

                        //ISfn.AddUserMenu(Convert.ToInt16(pgasUser.SwipEmployeeID), 5);
                        return RedirectToAction("Confirm_email_to_zimbra", "Account");
                    }
                    AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //recon
        [AllowAnonymous]
        public ActionResult RegisterHRISUser_recon()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterHRISUser_recon(RegisterByPgasViewModel model)
        {
            fmisEntities fmisdb = new fmisEntities();
            if (ModelState.IsValid)
            {

                var pgasUser = fmisdb.ufn_getPgasUserByEmail(model.Email).Single();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserID = Convert.ToInt16(pgasUser.SwipEmployeeID) };

                var result = await UserManager.CreateAsync(user, pgasUser.passcode);

                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/ConfirmEmail.html"));
                    string s = sr.ReadToEnd();
                    sr.Close();
                    ISfn.Addroles(Convert.ToInt16(pgasUser.SwipEmployeeID), 9);

                    //Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", s.Replace("{{action_url}}", callbackUrl).Replace("{{name}}", model.Email).Replace("[Product Name]", "AOMS"));
                    
                    //ISfn.AddUserMenu(Convert.ToInt16(pgasUser.SwipEmployeeID), 5);
                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,UserID= model.UserID };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    ISfn.Addroles(Convert.ToInt32(model.UserID), 9);
                    //StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/ConfirmEmail.html"));
                    //string s = sr.ReadToEnd();
                    ////s.Replace("#name", "123456").Replace("#Family", "123456");
                    //sr.Close();
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", s.Replace("{{action_url}}", callbackUrl).Replace("{{name}}", model.Email).Replace("[Product Name]", "AOMS"));//"Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>"

                    //await UserManager.AddToRoleAsync(user.Id, "Controller");


                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                if (await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link

                    StreamReader sr = new StreamReader(HttpContext.Server.MapPath("~/Views/Shared/Reset_Password.html"));
                    string s = sr.ReadToEnd();
                    //s.Replace("#name", "123456").Replace("#Family", "123456");
                    sr.Close();

                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Reset Your AOMS Account Password",s.Replace("{{action_url}}",callbackUrl).Replace("{{name}}",model.Email).Replace("[Product Name]","AOMS"));//"Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>"
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                   
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
                         
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError("", "Invalid Email Address");
            }
            else
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
            }
            
            
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {       
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
         
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        #region profile

        public ActionResult UserProfile() {
            var user = UserManager.FindById(User.Identity.GetUserId());

            fmisEntities fmis = new fmisEntities();


            var employee = fmis.vw_Employee_Concatname.Single(m => m.SwipEmployeeID == user.UserID.ToString());
            ViewBag.empname = employee.NameFML;
            ViewBag.position = employee.Pos_Name;
            if (System.IO.File.Exists(Server.MapPath(Url.Content("~/Content/UserImage/400x400/") + user.UserID + ".png")) == true)
            {
                ViewBag.pictureLocation = Url.Content("~/Content/UserImage/400x400/") + user.UserID + ".png?" + DateTime.Now.ToString(@"dd\hh\mm\ss");
            }
            else
            {
                ViewBag.pictureLocation = Url.Content("~/Content/UserImage/400x400/NoPicture.png?");
            }

            ViewBag.office = employee.Office;
            ViewBag.status = employee.empStat_id;
            ViewBag.birthdate = employee.Birthdate;
            ViewBag.birthplace = employee.BirthPlace;
            return View();
        }
        public ActionResult partial_changePicture()
        {
            return PartialView();
        }
        public JsonResult UploadImage50(string imageData)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var uploadResult = new Dictionary<string, bool>();
            var buffer = Convert.FromBase64String(imageData);
            //MySession.Current._pngtoPrint = data;
            Image image;
            using (Stream sr = new MemoryStream(buffer))
            {
                image = Image.FromStream(sr);
            }
            image.Save(Server.MapPath("~/Content/UserImage/50x50/" + user.UserID.ToString() + ".png"));
            uploadResult.Add(user.UserID.ToString() + ".png", true);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadImage400(string imageData)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var uploadResult = new Dictionary<string, bool>();
            var buffer = Convert.FromBase64String(imageData);
            //MySession.Current._pngtoPrint = data;
            Image image;
            using (Stream sr = new MemoryStream(buffer))
            {
                image = Image.FromStream(sr);
            }            
            image.Save(Server.MapPath("~/Content/UserImage/400x400/" + user.UserID.ToString() + ".png"));
            uploadResult.Add(user.UserID.ToString() + ".png", true);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}