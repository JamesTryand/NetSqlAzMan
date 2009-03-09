using System;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NetSqlAzManWebConsole.Objects
{
    public static class CookieProtectionHelperWrapper
    {

        private static MethodInfo _encode;
        private static MethodInfo _decode;

        static CookieProtectionHelperWrapper()
        {
            // obtaining a reference to System.Providers assembly
            Assembly systemWeb = typeof(HttpContext).Assembly;
            if (systemWeb == null)
            {
                throw new InvalidOperationException(
                    "Unable to load System.Web.");
            }
            // obtaining a reference to the internal class CookieProtectionHelper
            Type cookieProtectionHelper = systemWeb.GetType(
                    "System.Web.Security.CookieProtectionHelper");
            if (cookieProtectionHelper == null)
            {
                throw new InvalidOperationException(
                    "Unable to get the internal class CookieProtectionHelper.");
            }
            // obtaining references to the methods of CookieProtectionHelper class
            _encode = cookieProtectionHelper.GetMethod(
                    "Encode", BindingFlags.NonPublic | BindingFlags.Static);
            _decode = cookieProtectionHelper.GetMethod(
                    "Decode", BindingFlags.NonPublic | BindingFlags.Static);

            if (_encode == null || _decode == null)
            {
                throw new InvalidOperationException(
                    "Unable to get the methods to invoke.");
            }
        }

        public static string Encode(CookieProtection cookieProtection,
                                    byte[] buf, int count)
        {
            return (string)_encode.Invoke(null,
                    new object[] { cookieProtection, buf, count });
        }

        public static byte[] Decode(CookieProtection cookieProtection,
                                    string data)
        {
            return (byte[])_decode.Invoke(null,
                    new object[] { cookieProtection, data });
        }
    }
}
