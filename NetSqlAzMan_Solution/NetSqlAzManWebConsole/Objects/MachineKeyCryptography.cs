using System;
using System.Text;
using System.Web.Security;

namespace NetSqlAzManWebConsole.Objects
{
    public static class MachineKeyCryptography
    {
        public static string Encode(string text, CookieProtection cookieProtection)
        {
            if (string.IsNullOrEmpty(text) || cookieProtection == CookieProtection.None)
            {
                return text;
            }
            byte[] buf = Encoding.UTF8.GetBytes(text);
            return CookieProtectionHelperWrapper.Encode(cookieProtection, buf, buf.Length);
        }

        public static string Decode(string text, CookieProtection cookieProtection)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            byte[] buf;
            try
            {
                buf = CookieProtectionHelperWrapper.Decode(cookieProtection, text);
            }
            catch (Exception ex)
            {
                throw new System.Security.Cryptography.CryptographicException(
                    "Unable to decode the text", ex.InnerException);
            }
            if (buf == null || buf.Length == 0)
            {
                throw new System.Security.Cryptography.CryptographicException(
                    "Unable to decode the text");
            }
            return Encoding.UTF8.GetString(buf, 0, buf.Length);
        }
    }
}
