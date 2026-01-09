using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Net;
using System.Dynamic;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Collections.Generic;
using System;

namespace WebAOMS.Mod
{
    public static class removeInjection
    {
        public static string AntiInject(this string str)
        {
            if (str == null)
            {
                str = "";
            }


            return str.Replace("'", "''").Replace("--", "");
        }
    }
}