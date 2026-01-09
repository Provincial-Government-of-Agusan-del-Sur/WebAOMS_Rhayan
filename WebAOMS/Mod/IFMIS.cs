using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.IO;
using System.Net;
using Kendo.Mvc.UI;
using System.Web.Security;
using System.Web.Script.Serialization;
using WebAOMS.Models;
using System.Dynamic;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security.Cryptography;
using WebAOMS.Mod;

namespace WebAOMS.Base
{

    // They types of user we have on the site
    [Serializable]
    [Flags]
    public enum UserRole
    {
        Guest = 0,
        User = 1,
        SuperUser = 2,
        Admin = 4
    }
    
    public static class DatatableToJson
    {
        public static List<Dictionary<string, object>> DTToList(this DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }
    }
    public static class IFMISEntity
        {
            public static fmisEntities ifmisdb = new fmisEntities();
            //public static IEnumerable<vw_UserMenu> MenuItems;  
            public static IEnumerable<w_UserMenu> MenuItems;
        }
      
        public struct rpt_SubGeneralPay_Para
        {
            public string year { get; set; }
            public string SessionVariable { get; set; }
            public string SessionVariable2 { get; set; }
          //  public static string themeName { get { return HttpContext.Current.Session["tblThemeStyle"] != null ? ((vw_webAppsTheme)HttpContext.Current.Session["tblThemeStyle"]).wptheme_Name : ""; } }
        }
        public struct grid
        {
            public Int64 eid { get; set; }
            public string EmpName { get; set; }
            public string PolicyNo { get; set; }
            //  public static string themeName { get { return HttpContext.Current.Session["tblThemeStyle"] != null ? ((vw_webAppsTheme)HttpContext.Current.Session["tblThemeStyle"]).wptheme_Name : ""; } }
        }

        public struct USER
        {
            public static void Set(string Id,long roleID)
            {
                HttpCookie cookie = new HttpCookie(".Auserinfo");
                DataTable rec;
                rec = ISfn.ExecuteDataset("SELECT * from Accounting.vw_AspNetUsers_details  where [Id] = '" + Id + "'");
                if (rec.Rows.Count > 0)
                {
                    cookie.Values["0x8a"] = ISfn.EncryptStr(rec.Rows[0]["eid"].ToString());
                    cookie.Values["0x7d"] = ISfn.EncryptStr(rec.Rows[0]["SwipeId"].ToString());
                    cookie.Values["0x87"] = ISfn.EncryptStr(rec.Rows[0]["EmpName"].ToString());
                    cookie.Values["0x85"] = ISfn.EncryptStr(rec.Rows[0]["NameFML"].ToString());
                    cookie.Values["0x5e"] = ISfn.EncryptStr(rec.Rows[0]["Department"].ToString());
                    cookie.Values["0x56"] = ISfn.EncryptStr("1");
                    cookie.Values["0x35"] = ISfn.EncryptStr(rec.Rows[0]["OfficeName"].ToString());
                    cookie.Values["0xe3"] = ISfn.EncryptStr(rec.Rows[0]["OfficeAbbr"].ToString());
                    cookie.Values["0xab"] = ISfn.EncryptStr(rec.Rows[0]["Status"].ToString());
                    cookie.Values["0x76"] = ISfn.EncryptStr(rec.Rows[0]["Position"].ToString());
                    cookie.Values["0x7"] = ISfn.EncryptStr(roleID.ToString());
                    cookie.Values["0xg7"] = ISfn.EncryptStr(rec.Rows[0]["telephone"].ToString());
                    cookie.Values["0xf5"] = ISfn.EncryptStr("1");
                cookie.Values["0xl9"] = ISfn.EncryptStr(rec.Rows[0]["Id"].ToString());
                HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Values["0x8a"] = ISfn.EncryptStr(0.ToString());
                    cookie.Values["0x7d"] = ISfn.EncryptStr("0");
                    cookie.Values["0x87"] = ISfn.EncryptStr("");
                    cookie.Values["0x85"] = ISfn.EncryptStr("");
                    cookie.Values["0x5e"] = ISfn.EncryptStr("");
                    cookie.Values["0x56"] = ISfn.EncryptStr("1");
                    cookie.Values["0x35"] = ISfn.EncryptStr("");
                    cookie.Values["0xe3"] = ISfn.EncryptStr("");
                    cookie.Values["0xab"] = ISfn.EncryptStr("");
                    cookie.Values["0x76"] = ISfn.EncryptStr("");
                    cookie.Values["0x7"] = ISfn.EncryptStr("");
                    cookie.Values["0xg7"] = ISfn.EncryptStr("");
                    cookie.Values["0xf5"] = ISfn.EncryptStr("1");
                    cookie.Values["0xl9"] = ISfn.EncryptStr("");
                    HttpContext.Current.Response.Cookies.Add(cookie);
            }
            }
        public static int C_eID
            {
                get
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? Convert.ToInt32(ISfn.DecryptString(ck.Values["0x8a"])) : -1;
                }
            }
            public static string C_telephone
            {
                get
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0xg7"]) : "";
                }
            }

            public static string C_swipeID {
                get 
                {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0x7d"]) : ""; 
                }
            }
            public static string C_Name {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0x87"]) : ""; 
                } 
            }
            public static string C_NameFML
            {
                get
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck != null ? ISfn.DecryptString(ck.Values["0x85"]) : "";
                }
            }
            public static int C_OfficeID {
                get {
                        HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                        return ck  != null ? Convert.ToInt32(ISfn.DecryptString(ck.Values["0x5e"])) : 0; 
                } 
            }
            public static int C_userTypeID {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? Convert.ToInt32(ISfn.DecryptString(ck.Values["0x56"])) : 0; 
                } 
            }
            public static string C_Office {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0x35"]) : ""; 
                } 
            }
            
            public static string C_OfficeCode {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0xe3"]) : ""; 
                } 
            }

            public static string C_Status {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0xab"]) : ""; 
                } 
            }

            public static string C_Position {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? ISfn.DecryptString(ck.Values["0x76"]) : " "; 
                } 
            }

            public static int C_RoleID {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? Convert.ToInt32(ISfn.DecryptString(ck.Values["0x7"])) : 0; 
                } 
            }

            public static int C_isLogin {
                get {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck  != null ? Convert.ToInt32(ISfn.DecryptString(ck.Values["0xf5"])) : 0; 
                } 
            }
            public static string C_picLink
            {
                get
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                    return ck != null ? ISfn.DecryptString(ck.Values["0xfd"]) + "?timestamp=" + DateTime.Now.ToString("yyyyMMddHHmmss") : "";
                }
            }
        public static string Id
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".Auserinfo");
                return ck != null ? ISfn.DecryptString(ck.Values["0xl9"]) : " ";
            }
        }
    }

        public struct USERMENU
        {

           public static int role_ID;
           public static void SetUserMenu(IEnumerable<w_UserMenu> items, string Id)
            {   
                IFMISEntity.MenuItems = items;
                string MenuItems = "";
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                MenuItems = serializer.Serialize((IEnumerable<w_UserMenu>)items);
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/UserContent/menu/") + Id + ".menu", MenuItems);
            }

           public static IEnumerable<w_UserMenu> items
           {
                get
                {
                    try
                    {
                    
                    return (IEnumerable<w_UserMenu>)(new JavaScriptSerializer().Deserialize(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/UserContent/menu/") + USER.Id + ".menu"), typeof(IEnumerable<w_UserMenu>)));
                    }
                    catch (Exception e)
                    {
                        FormsAuthentication.RedirectToLoginPage();
                        return (null);
                    }
                    
                }
            }
        }
    public class DYNAMICCLASS : DynamicObject
    {
        private IDictionary<string, object> _values;

        public DYNAMICCLASS(IDictionary<string, object> values)
        {
            _values = values;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_values.ContainsKey(binder.Name))
            {
                result = _values[binder.Name];
                return true;
            }
            result = null;
            return false;
        }
    }
    
    public static class DYNAMICMODEL
    {
        public static dynamic ToModel(this DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                #region SINGLE ROW

                if (dt.Rows.Count == 1)
                {
                    var values = new Dictionary<string, object>();
                    foreach (DataColumn cl in dt.Columns)
                    {
                        values.Add(cl.ColumnName, dt.Rows[0][cl.Ordinal]);
                    }

                    dynamic result = new DYNAMICCLASS(values);
                    return result;
                }
                #endregion

                #region MULTIPLE ROWS
                else
                {
                    List<dynamic> list = new List<dynamic>();
                    foreach (DataRow rw in dt.Rows)
                    {
                        var values = new Dictionary<string, object>();
                        foreach (DataColumn cl in dt.Columns)
                        {
                            values.Add(cl.ColumnName, rw[cl.Ordinal]);
                        }
                        list.Add(new DYNAMICCLASS(values));
                    }
                    return list;
                }
                #endregion
            }

            return null;
        }

    }
    public static class EncryptDecrypt
    {
        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        private static byte[] _salt = Encoding.ASCII.GetBytes("o680664+2kbM7c5");
        public static dynamic Encrypt(this string plainText)
        {
            string sharedSecret = "simfi#Pgas@HRMIS#2016";
            string outStr = null;
            if (plainText != "")
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException("plainText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                // Encrypted string to return
                RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

                try
                {
                    // generate the key from the shared secret and the salt
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                    // Create a RijndaelManaged object
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        // prepend the IV
                        msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                        }
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
            }
            // Return the encrypted bytes from the memory stream.
            return outStr.Replace("EAAAA", "tryM0Decrypt");
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static dynamic Decrypt(this string cipherText)
        {
            cipherText = cipherText.Replace("tryMODecrypt", "EAAAA");
            string sharedSecret = "simfi#Pgas@HRMIS#2016";
            string plaintext = null;
            if (cipherText != "")
            {
                if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException("cipherText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                // Declare the RijndaelManaged object
                // used to decrypt the data.
                RijndaelManaged aesAlg = null;

                // Declare the string used to hold
                // the decrypted text.            
                try
                {
                    // generate the key from the shared secret and the salt
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                    // Create the streams used for decryption.                
                    byte[] bytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        // Create a RijndaelManaged object
                        // with the specified key and IV.
                        aesAlg = new RijndaelManaged();
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                        // Get the initialization vector from the encrypted stream
                        aesAlg.IV = ReadByteArray(msDecrypt);
                        // Create a decrytor to perform the stream transform.
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                finally
                {
                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
            }
            return plaintext;
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }
            return buffer;
        }
    }
    public struct USERMENU_public
    {
        public static int role_ID;
         
        public static void SetUserMenu_public()
        {
            fmisEntities fmisdb = new fmisEntities();
            string MenuItems = "";
            IEnumerable<w_UserMenu> _menu = fmisdb.w_UserMenu.Where(M => M.userid == 0).OrderBy(o => o.Ordering);
            IFMISEntity.MenuItems = _menu;
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            MenuItems = serializer.Serialize((IEnumerable<w_UserMenu>)_menu);
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/UserContent/menu/") +  "0.menu", MenuItems);
        }

        public static IEnumerable<w_UserMenu> items
        {
            get
            {
                try
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UserContent/menu/") + "0.menu") ==false)
                    {
                        SetUserMenu_public();
                    }

                    return (IEnumerable<w_UserMenu>)(new JavaScriptSerializer().Deserialize(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/UserContent/menu/") + "0.menu"), typeof(IEnumerable<w_UserMenu>)));
                }
                catch (Exception e)
                {
                    FormsAuthentication.RedirectToLoginPage();
                    return (null);
                }

            }
        }
    }
    //public struct ApplicationError
    //{
    //    public static IFMISEntities ifmisdb = new IFMISEntities();
    //    //public static IEnumerable<vw_UserMenu> MenuItems;  
    //    public static void SaveError(int errorCode = 0,string errorMessage = "",string errorClass = "")
    //    {
    //        C__error error = new C__error();
    //        error.errorMessage = errorMessage;
    //        error.UserID = 0;
    //        error.errorClass = errorClass;
    //        error.errorCode = errorCode;
    //        ifmisdb.C__error.AddObject(error);
    //        ifmisdb.SaveChanges();
    //        // Redirect to a landing page
    //        //Response.Redirect("home/landing");
    //    }
    //}

}