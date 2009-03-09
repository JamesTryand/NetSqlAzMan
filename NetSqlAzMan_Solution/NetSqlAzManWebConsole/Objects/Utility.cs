using System;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NetSqlAzManWebConsole
{
    public static class Utility
    {
        #region Delegates
        internal delegate void ExecuteWithImpersonationDelegate(WindowsIdentity identity);
        #endregion Delegates
        #region Methods
        internal static string PipeEncode(string value)
        {
            return value.Replace("|", "||");
        }
        internal static string QuoteJScriptString(string value, bool forUrl)
        {
            StringBuilder builder = null;
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '%':
                        {
                            if (!forUrl)
                            {
                                break;
                            }
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 6);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append("%25");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '\'':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append(@"\'");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '\\':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append(@"\\");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '\t':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append(@"\t");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '\n':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append(@"\n");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '\r':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append(@"\r");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                    case '"':
                        {
                            if (builder == null)
                            {
                                builder = new StringBuilder(value.Length + 5);
                            }
                            if (count > 0)
                            {
                                builder.Append(value, startIndex, count);
                            }
                            builder.Append("\\\"");
                            startIndex = i + 1;
                            count = 0;
                            continue;
                        }
                }
                count++;
            }
            if (builder == null)
            {
                return value;
            }
            if (count > 0)
            {
                builder.Append(value, startIndex, count);
            }
            return builder.ToString();
        }
        internal static void ExecuteWithImpersonation(WindowsIdentity identity, ExecuteWithImpersonationDelegate code)
        {
            //http://msdn.microsoft.com/msdnmag/issues/05/09/SecurityBriefs/
            // Temporarily impersonate the original user.
            using (WindowsImpersonationContext wic = identity.Impersonate())
            {
                try
                {
                    // Access resources while impersonating.
                    code.Invoke(identity);
                }
                finally
                {
                    // Revert impersonation.
                    wic.Undo();
                }
            }
        }
        internal static DirectoryEntry NewDirectoryEntry(string path)
        {

            //if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Active Directory LookUp Username"]))
            //{
            //    System.Security.Principal.WindowsImpersonationContext wi = ((WindowsIdentity)System.Providers.HttpContext.Current.User.Identity).Impersonate();
            //    try
            //    {
            //        return new DirectoryEntry(path);
            //    }
            //    finally
            //    {
            //        wi.Undo();
            //    }
            //}
            //else
            {
                return new DirectoryEntry(
                    path,
                    ConfigurationManager.AppSettings["Active Directory LookUp Username"],
                    ConfigurationManager.AppSettings["Active Directory LookUp Password"]);
            }
        }
        #endregion Methods
    }
}
