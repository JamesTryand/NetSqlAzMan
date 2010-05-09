using System.Web;
using System.Web.Security;

namespace NetSqlAzManWebConsole.Objects
{
    public static class HttpSecureCookie
    {
        public static HttpCookie Encode(HttpCookie cookie)
        {
            return Encode(cookie, CookieProtection.All);
        }

        public static HttpCookie Encode(HttpCookie cookie,
                      CookieProtection cookieProtection)
        {
            HttpCookie encodedCookie = CloneCookie(cookie);
            encodedCookie.Value = MachineKeyCryptography.Encode(cookie.Value, cookieProtection);
            return encodedCookie;
        }

        public static HttpCookie Decode(HttpCookie cookie)
        {
            return Decode(cookie, CookieProtection.All);
        }

        public static HttpCookie Decode(HttpCookie cookie,
                      CookieProtection cookieProtection)
        {
            HttpCookie decodedCookie = CloneCookie(cookie);
            decodedCookie.Value = MachineKeyCryptography.Decode(cookie.Value, cookieProtection);
            return decodedCookie;
        }

        public static HttpCookie CloneCookie(HttpCookie cookie)
        {
            HttpCookie clonedCookie = new HttpCookie(cookie.Name, cookie.Value);
            clonedCookie.Domain = cookie.Domain;
            clonedCookie.Expires = cookie.Expires;
            clonedCookie.HttpOnly = cookie.HttpOnly;
            clonedCookie.Path = cookie.Path;
            clonedCookie.Secure = cookie.Secure;

            return clonedCookie;
        }
    }
}
