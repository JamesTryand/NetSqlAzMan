using System.Collections.Generic;
using System.Security.Principal;

namespace NetSqlAzMan.Cache
{
    /// <summary>
    /// WindowsIdentity extension methods.
    /// </summary>
    public static class WindowsIdentityExtensions
    {
        /// <summary>
        /// Gets the user binary sid.
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <returns></returns>
        public static string GetUserBinarySSid(this WindowsIdentity windowsIdentity)
        {
            return windowsIdentity.User.Value;
        }

        /// <summary>
        /// Gets the groups binary sid.
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <returns></returns>
        public static string[] GetGroupsBinarySSid(this WindowsIdentity windowsIdentity)
        {
            List<string> result = new List<string>();
            foreach (SecurityIdentifier sid in windowsIdentity.Groups)
            { 
                result.Add(sid.Value);
            }
            return result.ToArray();
        }
    }
}
