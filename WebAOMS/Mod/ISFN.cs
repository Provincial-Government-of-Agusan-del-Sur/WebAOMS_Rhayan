using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Kendo.Mvc.Extensions;
using System.Configuration;
using System.IO;
using System.Net;
using System.Dynamic;
//using Kendo.Mvc.UI;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Net.Mail;
using WebAOMS.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Data.SqlClient;
using WebAOMS.Mod;
using System.Threading.Tasks;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;

using System.Windows.Forms;

namespace WebAOMS.Base
{
    public static class SerDeserJson
    {
        public static string SerializeJSON<T>(T obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(obj);
        }


        public static T DeserializeJSON<T>(string obj, int recursionDepth = 100)
        {
            string jsonString = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Includes/Json/") + obj);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            //serializer.RecursionLimit = recursionDepth;
            return serializer.Deserialize<T>(jsonString);
        }


        public static void WriteJson(string Filename, string JsonItem)
        {

            string file = HttpContext.Current.Server.MapPath("~/Includes/Json/") + Filename;
            if (Directory.Exists(Path.GetDirectoryName(file)))
            {
                File.Delete(file);
            }
            StreamWriter menufile = new StreamWriter(HttpContext.Current.Server.MapPath("~/Includes/Json/") + Filename, true);
            menufile.WriteLine(JsonItem);
            menufile.Flush();
            menufile.Close();
            menufile.Dispose();
        }

    }
    public static class ISfn
    {
        public static string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public static string ifmisdb = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();


        public static void LogData(string userName, string action, int id, string newData, string oldData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(fmisConn))
                {
                    //Open the connection
                    conn.Open();

                    //Create a new SqlCommand to execute the SQL query
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LogTable (UserName, Action, RecordId, NewData, OldData) VALUES (@UserName, @Action, @RecordId, @NewData, @OldData)", conn))
                    {
                        //Add parameters to the SqlCommand
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Action", action);
                        cmd.Parameters.AddWithValue("@RecordId", id);
                        cmd.Parameters.AddWithValue("@NewData", newData);
                        cmd.Parameters.AddWithValue("@OldData", oldData);

                        //Execute the SQL query
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception e ) {
                    
            }
            //Create a new SqlConnection using the connection string from the Web.config file
            
        }

        public static string warning(string str)
        {
            return "toastr.warning('"+ str +"','')";
        }

        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        
        public static string gen_GUID()
        {
            return Guid.NewGuid().ToString().Substring(24, 12);
        }
        public static Image QRGen(string input, int qrlevel)
        {

            string toenc = input;

            MessagingToolkit.QRCode.Codec.QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();

            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; // - Using LOW for more storage

            qe.QRCodeVersion = qrlevel;

            System.Drawing.Bitmap bm = qe.Encode(toenc);

            return bm;

        }
        public static Byte[] createPDFBinary(Telerik.Reporting.Processing.RenderingResult result)
        {
            MemoryStream ms = new MemoryStream(result.DocumentBytes);

            BinaryReader br = new BinaryReader(ms);
            Byte[] bytes = br.ReadBytes((Int32)ms.Length);

            return bytes;
        }


        public static void SaveQRcode(string input, int qrlevel,string filename)
        {
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Content/QR/") + filename + ".jpeg") == false)
            {
                string toenc = input;

                MessagingToolkit.QRCode.Codec.QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();

                qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

                qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; // - Using LOW for more storage

                qe.QRCodeVersion = qrlevel;
            
                System.Drawing.Bitmap bm = qe.Encode(toenc);

                var memStream = new MemoryStream();
                bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Save Jpeg stream into file
                using (var fileStream = new FileStream(HttpContext.Current.Server.MapPath("~/Content/QR/") + filename + ".jpeg", FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    memStream.Position = 0;
                    memStream.CopyTo(fileStream);
                }
            }
        }

        public static string errorLog(Int64 ErrorCode = 0,string ErrorMessage = "" ,string ErrorClass = "",int UserID = 0)
        {
            //string errorMessage = OleDbHelper.ExecuteScalar(cstr, CommandType.Text, "execute IFMIS.dbo.sp_errorLog " + ErrorCode + ",'" + ErrorMessage.Replace("'", "''") + "'," + UserID + ",'" + ErrorClass + "'").ToString();
            return ErrorMessage;
        }
        public static Boolean  checkAmountIfQualify(int eid,decimal amount)
        {
            Decimal netamount = 0;
            bool r =false;
            netamount = Convert.ToDecimal(OleDbHelper.ExecuteScalar(ifmisdb, CommandType.Text, "select epay.fn_getNetTakeHomePay("+ eid +") as netpay"));
            if (netamount - amount > 0)
            {
                r = true;
            }
            return r;
        }
        public static int CompanyGenerationType
        {
            get
            {
                int r = 0;
                r = Convert.ToInt16(OleDbHelper.ExecuteScalar(ifmisdb, CommandType.Text, "SELECT value FROM [pmis].[epay].[tbl_l_Parameter] where settingname = 'CompanyGenerationType'"));
            
                return r;
            }
        }
        public static string getCompanyName
        {
            get
            {
                string companyname;
                companyname = OleDbHelper.ExecuteScalar(ifmisdb, CommandType.Text, "SELECT CompanyName FROM [pmis].[dbo].[tbl_Payroll_report_Signatory] where userid =" + USER.C_eID + "").ToString();
                return companyname;
            }
        }
        public static void ActionLog(string actionName)
        {
            //string errorMessage = OleDbHelper.ExecuteScalar(cstr, CommandType.Text, "execute IFMIS.dbo.sp_errorLog " + ErrorCode + ",'" + ErrorMessage.Replace("'", "''").AntiInject() + "'," + UserID + ",'" + ErrorClass.AntiInject() + "'").ToString();
            ISfn.ExcecuteNoneQuery("insert into epay.[tbl_t_ActionLog]([ActionName],[userid]) values ('" + actionName.AntiInject() + "','" + USER.C_eID.ToString() + "')");
        }
        public static string ExcecuteNoneQuery(string qry)
        {
            OleDbHelper.ExecuteNonQuery(ifmisdb, CommandType.Text,
                        qry);
            return "1";
        }
        public static DataTable ToDatatable(string qry="",string para1="",string paraVal1="")
        {
            DataTable rec = new DataTable();
            string cmdStr = qry;
            SqlConnection connection = new SqlConnection(fmisConn);
            
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue(para1, paraVal1);

                connection.Open();
                command.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return rec;
        }

        public static int checkIfRegenerateReport(string refno)
        {
            Int16 isRegenerate = 0;
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_Check_If_Regenerate_report] @refno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@refno", refno);
                connection.Open();
                command.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            isRegenerate = Convert.ToInt16(rec.Rows[0]["isRegenerate"]);

            if (!File.Exists(ISfn.xmlpath(1)+ refno) && isRegenerate ==0)
            {
                isRegenerate = 1;
            }
            return isRegenerate;
        }
        public static int checkIfRegenerateReport_custom(string refno,int reportid)
        {
            Int16 isRegenerate = 0;
            DataTable rec = new DataTable();
            string cmdStr = "execute [Accounting].[usp_Check_If_Regenerate_report_custom] @refno";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@refno", refno);
                connection.Open();
                command.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

            isRegenerate = Convert.ToInt16(rec.Rows[0]["isRegenerate"]);

            if (!File.Exists(ISfn.xmlpath(reportid) + refno) && isRegenerate == 0)
            {
                isRegenerate = 1;
            }
            return isRegenerate;
        }
        public static DataTable reportSignatory(int userid,int reportID)
        {
            DataTable rec = new DataTable();
            string cmdStr = "SELECT * FROM [fmis].[Accounting].[tbl_l_report_Signatory] where userid = 0";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {                
                connection.Open();
                command.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }

           return rec;
        }
        public static DataSet ToDataset(string qry = "", string para1 = "", string paraVal1 = "")
        {
            DataSet rec = new DataSet();
            string cmdStr = qry;
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue(para1, paraVal1);
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(rec);
                connection.Close();
                da.Dispose();
            }
            return rec;
        }

        public static Int16 ToExecute2P(string qry = "", string para1 = "", string paraVal1 = "", string para2 = "", Int32 paraVal2=0)
        {
            Int16 r = 0;
            DataTable rec = new DataTable();
            string cmdStr = qry;
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue(para1, paraVal1);
                command.Parameters.AddWithValue(para2, paraVal2);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                r = 6;
            }
            return r;
        }
        //await sample
        //public static async Task<int> HandleFileAsync() {
        //    string file = @"C:\programs\enable1.txt";
        //    Console.WriteLine("HandleFile enter");
        //    int count = 0;

        //    // Read in the specified file.
        //    // ... Use async StreamReader method.
        //    using (StreamReader reader = new StreamReader(file))
        //    {
        //        string v = await reader.ReadToEndAsync();

        //        // ... Process the file data somehow.
        //        count += v.Length;

        //        // ... A slow-running computation.
        //        //     Dummy code.
        //        for (int i = 0; i < 10000; i++)
        //        {
        //            int x = v.GetHashCode();
        //            if (x == 0)
        //            {
        //                count--;
        //            }
        //        }
        //    }
        //    Console.WriteLine("HandleFile exit");
        //    return count;
        //}
    
        public static void Addroles(int userid,int roleid)
        {
            string r = "failed";
            DataTable rec = new DataTable();
            string cmdStr = "execute Tracking.[usp_add_role_menu] @userid, @roleid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@roleid", roleid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                r = "success";
            }
        }
        public static void AddUserMenu(int userid, int roleid)
        {
            string r = "failed";
            DataTable rec = new DataTable();
            string cmdStr = "execute Tracking.[usp_add_user_menu] @userid, @roleid";
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@roleid", roleid);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                r = "success";
            }
            // return r;
        }
        public static string ToExecute3P(string qry = "", string para1 = "", string paraVal1 = "", string para2 = "", Int32 paraVal2 = 0, string para3 = "", string paraVal3 = "")
        {
            string r = "";
            DataTable rec = new DataTable();
            string cmdStr = qry;
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue(para1, paraVal1);
                command.Parameters.AddWithValue(para2, paraVal2);
                command.Parameters.AddWithValue(para3, paraVal3);
                connection.Open();
                r = command.ExecuteScalar().ToString();
                connection.Close();
            }
            return r;
        }

        public static string ToStringp(string qry = "", string para1 = "", string paraVal1 = "")
        {
            string r = "";
            DataTable rec = new DataTable();
            string cmdStr = qry;
            SqlConnection connection = new SqlConnection(fmisConn);

            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue(para1, paraVal1);
                connection.Open();
                r = command.ExecuteScalar().ToString();
                connection.Close();
            }
            return r;
        }


        public static DataTable ExecuteDataset(string qry)
        {
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, qry).Tables[0];
                return rec;
        }
        public static DataSet ExecDataset(string qry)
        {
            DataSet rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), System.Data.CommandType.Text, qry);
            return rec;
        }
        public static string ExecScalar(string qry)
        {
            string r;
            r = OleDbHelper.ExecuteScalar(ifmisdb, CommandType.Text,
                       qry).ToString();
            return r;
        }
        public static Boolean CheckIfExist(string qry = "")
        {
            Boolean chk = false;
            DataTable result;
            result = OleDbHelper.ExecuteDataset(ifmisdb, CommandType.Text, qry).Tables[0];
            if (result.Rows.Count > 0)
            {
                chk = true;
            }
            return chk;
        }
        public static string get_PayrollReportVersion(int maintenanceID=0)
        {
            string version = "0";
            DataTable result;
            result = OleDbHelper.ExecuteDataset(ifmisdb, CommandType.Text, "select value from dbo.tbl_payroll_settings where maintenanceID = "+maintenanceID+"").Tables[0];
            if (result.Rows.Count > 0)
            {
                version = result.Rows[0]["value"].ToString();
            }
            return version;
        }
        public static string Get_EmptypeByEmpStatusID(int employementstatusid)
        { 
            string r;
            if (employementstatusid == 567)
            {
                r = "Casual/JOB-Order/Contract of Service";
            }
            else
            {
                r = ISfn.ExecScalar("SELECT [description] FROM [pmis].[dbo].[refEmploymentStatus] where [employmentstatus_id] = "+employementstatusid).ToString();
            }
        return r;
        }

        public static string Get_employee_phoneno(int eid)
        {
            string r;
            
                r = ISfn.ExecScalar("SELECT telephone from pmis.dbo.vw_employee_Concatname where eid = "+eid+"").ToString();
            return r; 
        }
        public static DataSet getDataset(string qry)
        {
            return OleDbHelper.ExecuteDataset(ifmisdb, CommandType.Text,
                        qry);
        }
        public static Boolean CheckIfObligatedAmountOk(double amount = 0)
        {
            Boolean chk = false;
            DataTable result;

            result = OleDbHelper.ExecuteDataset(ifmisdb, CommandType.Text, "").Tables[0];
            if (result.Rows.Count > 0)
            {
                chk = true;
            }
            return chk;
        }
        public static void LogReportGuid(int batchnoOrOfficeID = 0,string guid = "",int reportID = 0)
        {
            string qry = "insert into [IFMIS].[dbo].[tbl_Report_guid] ([batchno],[guid],[reportID],userID) values ("+batchnoOrOfficeID+",'"+guid+"',"+reportID+","+USER.C_eID+")";
             OleDbHelper.ExecuteNonQuery(ifmisdb, CommandType.Text, qry);
        }
        public static string newguid()
        {
            string new_guid = Guid.NewGuid().ToString().Substring(24, 12);
            return new_guid;
        }
        public static Int32 CreateRegularPayBatchno(long officeID = 0,int year = 0,int month = 0)
        {
            string officeYearmonth = year.ToString().Substring(2, 2) + month.ToString("D2") + officeID.ToString();
            return Convert.ToInt32(officeYearmonth);
        }

        public static string UrlStr(string url)
        {
            UrlHelper UH = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return UH.Content(url).ToString();
        }

        public static string xmlpath(int reportid)
        {
            string xmlPath;
            if (reportid == 20)
            {
                xmlPath = "~/DataXml/RAAOU/";
            }
            else
            {
                xmlPath = "~/xmlreport/";
            }
            
            return HttpContext.Current.Server.MapPath(xmlPath);
        }
        public static string xmlpath_menu(Int16 userid)
        {
            return HttpContext.Current.Server.MapPath("~/xmlpath_menu/" + userid.ToString());
        }
        public static string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
        public static DataTable ConvertXMLToDatatable(string Name, string XMLString)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(XMLString));
            DataTable Dt = new DataTable(Name);
            try
            {

                XmlNode NodoEstructura = doc.FirstChild.FirstChild;
                //  Table structure (columns definition) 
                foreach (XmlNode columna in NodoEstructura.ChildNodes)
                {
                    Dt.Columns.Add(columna.Name, typeof(String));
                }

                XmlNode Filas = doc.FirstChild;
                //  Data Rows 
                foreach (XmlNode Fila in Filas.ChildNodes)
                {
                    List<string> Valores = new List<string>();
                    foreach (XmlNode Columna in Fila.ChildNodes)
                    {
                        Valores.Add(Columna.InnerText);
                    }
                    Dt.Rows.Add(Valores.ToArray());
                }
            } catch(Exception)
            {

            }
        return Dt;
    }

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        private static byte[] _salt = Encoding.ASCII.GetBytes("o680664+2kbM7c5");
        public static string EncryptStr(string plainText, string sharedSecret = "simfi")
        {
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
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static string DecryptString(string cipherText, string sharedSecret = "simfi")
        {
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
        // By using this method we can convert datatable to xml


        internal static void ExcecuteNoneQuery(object p)
        {
            throw new NotImplementedException();
        }
    }
    public static class stringConvertion
    {
        public static List<int> StringToListInt(this string str)
        {
            string[] words = str.Split(',');
            List<int> lst = new List<int>();
            foreach (string j in words)
            {
                lst.Add(Convert.ToInt16(j));
            }
            return lst;
        }
        public static List<string> StringToList(this string str)
        {
            string[] words = str.Split(',');
            List<string> lst = new List<string>();
            foreach (string j in words)
            {
                lst.Add(j);
            }
            return lst;
        }
    }

}