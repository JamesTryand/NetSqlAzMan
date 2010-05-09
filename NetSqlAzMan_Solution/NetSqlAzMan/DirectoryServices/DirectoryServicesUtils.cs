using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace NetSqlAzMan.DirectoryServices
{
    /// <summary>
    /// DirectoryServicesUtils class. Provides methods to manipulate AD Objects.
    /// </summary>
    public static class DirectoryServicesUtils 
    {
        internal static string rootDsePath;
        internal static string userName;
        internal static string password;

        #region IsAGroup
        const int NO_ERROR = 0;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        /// <summary>
        /// Lookups the account sid.
        /// </summary>
        /// <param name="lpSystemName">Name of the lp system.</param>
        /// <param name="Sid">The sid.</param>
        /// <param name="lpName">Name of the lp.</param>
        /// <param name="cchName">Name of the CCH.</param>
        /// <param name="ReferencedDomainName">Name of the referenced domain.</param>
        /// <param name="cchReferencedDomainName">Name of the CCH referenced domain.</param>
        /// <param name="peUse">The pe use.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountSid(
          string lpSystemName,
          [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
          System.Text.StringBuilder lpName,
          ref uint cchName,
          System.Text.StringBuilder ReferencedDomainName,
          ref uint cchReferencedDomainName,
          out SID_NAME_USE peUse);

        /// <summary>
        /// Determines whether the specified sid is group.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <returns>
        /// 	<c>true</c> if the specified sid is group; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsGroup(Byte[] sid)
        {
            StringBuilder name = new StringBuilder();
            UInt32 cchName = (UInt32)name.Capacity;
            StringBuilder referencedDomainName = new StringBuilder();
            UInt32 cchReferencedDomainName = (UInt32)referencedDomainName.Capacity;
            SID_NAME_USE sidUse;
            Int32 err = NO_ERROR;
            if (!LookupAccountSid(null, sid, name, ref cchName,
                referencedDomainName, ref cchReferencedDomainName, out sidUse))
            {
                err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                if (err == ERROR_INSUFFICIENT_BUFFER)
                {
                    name.EnsureCapacity((int)cchName);
                    referencedDomainName.EnsureCapacity((int)cchReferencedDomainName);
                    err = NO_ERROR;
                    if (!LookupAccountSid(null, sid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                    {
                        err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    }
                }
            }
            if (err != 0)
            {
                throw new COMException("Errore nella lettura delle proprietà del SID", err);
            }
            if (sidUse == SID_NAME_USE.SidTypeUser)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Determines whether the specified login is group.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>
        /// 	<c>true</c> if the specified login is group; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsGroup(String login)
        {
            NTAccount nta = new NTAccount(login);
            SecurityIdentifier sid = (SecurityIdentifier)nta.Translate(typeof(SecurityIdentifier));
            Byte[] sidByte = new Byte[sid.BinaryLength];
            sid.GetBinaryForm(sidByte, 0);
            return DirectoryServicesUtils.IsGroup(sidByte);
        }
        #endregion IsAGroup

        static DirectoryServicesUtils()
        {
            try
            {
                DirectoryServicesUtils.userName = null;
                DirectoryServicesUtils.password = null;
                if (String.IsNullOrEmpty(DirectoryServicesUtils.rootDsePath))
                {
                    DirectoryEntry rootDSE = DirectoryServicesUtils.newDirectoryEntry("LDAP://RootDSE");
                    DirectoryServicesUtils.rootDsePath = (string)(rootDSE.Properties["defaultNamingContext"][0]);
                    if (DirectoryServicesUtils.rootDsePath.ToUpper().StartsWith("LDAP://"))
                    {
                        DirectoryServicesUtils.rootDsePath = DirectoryServicesUtils.rootDsePath.Substring(7);
                    }
                }
            }
            catch (Exception ex)
            {
                DirectoryServicesUtils.rootDsePath = "RootDSE";
                new NetSqlAzMan.Logging.LoggingUtility().WriteError(null, "Cannot find RootDSE path. LDAP Queries should be fails !\r\n" + ex.Message);
            }
        }
        /// <summary>
        /// Converts the owner to string.
        /// </summary>
        /// <param name="sidBytes">The owner bytes.</param>
        /// <returns></returns>
        public static string ConvertSidToString(byte[] sidBytes)
        {
            return new System.Security.Principal.SecurityIdentifier(sidBytes, 0).ToString();
        }

        /// <summary>
        /// Converts the string to owner.
        /// </summary>
        /// <param name="sid">The owner.</param>
        /// <returns></returns>
        public static byte[] ConvertStringToSid(string sid)
        {
            byte[] result = new byte[28];
            new System.Security.Principal.SecurityIdentifier(sid).GetBinaryForm(result, 0);
            return result;
        }

        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        public static void GetMemberInfo(string sid, out string memberName, out bool isLocal)
        {
            try
            {
                SecurityIdentifier SID = new SecurityIdentifier(sid);
                //SearchResult sr = null;

                NTAccount nta = ((NTAccount)SID.Translate(typeof(NTAccount)));
                isLocal = (nta.Value.StartsWith(Environment.MachineName));
                memberName = nta.Value;
                //if (SID.AccountDomainSid.Equals(SID)) //!=null
                //{
                //    DirectoryEntry root = DirectoryServicesUtils.newDirectoryEntry("LDAP://" + SqlAzManStorage.RootDSEPath);
                //    DirectorySearcher ds = new DirectorySearcher(root, String.Format("(&(objectSid={0}))", sid));
                //    sr = ds.FindOne();
                //}
                //if (sr != null)
                //{
                //    DirectoryEntry de = sr.GetDirectoryEntry();
                //    isLocal = false;
                //    memberName = (string)de.Properties["samaccountname"].Value;
                //    if (String.IsNullOrEmpty(memberName))
                //    {
                //        isLocal = false;
                //        //NTAccount nta = (NTAccount)SID.Translate(typeof(NTAccount));
                //        memberName = nta.Value;
                //    }
                //}
                //else
                //{
                //    isLocal = true;
                //    //NTAccount nta = (NTAccount)SID.Translate(typeof(NTAccount));
                //    memberName = nta.Value;
                //}
            }
            catch (Exception ex)
            {
                memberName = sid;
                isLocal = false;
                new NetSqlAzMan.Logging.LoggingUtility().WriteWarning(null, ex.Message + "\r\nSid: " + sid);
            }
        }

        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="isAGroup">if set to <c>true</c> [is A group].</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        public static void GetMemberInfo(string sid, out string memberName, out bool isAGroup, out bool isLocal)
        {
            memberName = sid;
            isAGroup = true;
            isLocal = false;
            try
            {
                DirectoryServicesUtils.GetMemberInfo(sid, out memberName, out isLocal);
                isAGroup = DirectoryServicesUtils.IsGroup(DirectoryServicesUtils.ConvertStringToSid(sid));
            }
            catch (Exception ex)
            {
                new NetSqlAzMan.Logging.LoggingUtility().WriteWarning(null, ex.Message + "\r\nSid: " + sid);
            }
        }
        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <param name="lDapQuery">The l dap query.</param>
        /// <returns></returns>
        public static SearchResultCollection ExecuteLDAPQuery(string lDapQuery)
        {
            try
            {
                DirectoryEntry root = DirectoryServicesUtils.newDirectoryEntry("LDAP://" + (DirectoryServicesUtils.GetRootDSEPart(lDapQuery) ?? SqlAzManStorage.RootDSEPath));
                root.RefreshCache();
                DirectorySearcher searcher = new DirectorySearcher(root, DirectoryServicesUtils.GetLDAPQueryPart(lDapQuery), new string[] { "objectSid" });
                return searcher.FindAll();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <param name="lDapQuery">The l dap query.</param>
        /// <returns></returns>
        public static bool TestLDAPQuery(string lDapQuery)
        {
            try
            {
                if (String.IsNullOrEmpty(lDapQuery))
                    return true;
                if (String.IsNullOrEmpty(lDapQuery.Trim()))
                    return true;
                DirectoryEntry root = DirectoryServicesUtils.newDirectoryEntry("LDAP://" + (DirectoryServicesUtils.GetRootDSEPart(lDapQuery) ?? SqlAzManStorage.RootDSEPath));
                root.RefreshCache();
                DirectorySearcher searcher = new DirectorySearcher(root, DirectoryServicesUtils.GetLDAPQueryPart(lDapQuery), new string[] { "objectSid" });
                searcher.FindOne();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the root DSE part.
        /// </summary>
        /// <param name="LDAPQuery">The LDAP query.</param>
        /// <returns></returns>
        public static string GetRootDSEPart(string LDAPQuery)
        {
            string rootDSEPrefix = "[RootDSE:";
            if (!String.IsNullOrEmpty(LDAPQuery) && LDAPQuery.Trim().StartsWith(rootDSEPrefix, StringComparison.CurrentCultureIgnoreCase))
            {
                LDAPQuery = LDAPQuery.Substring(LDAPQuery.IndexOf(rootDSEPrefix, StringComparison.CurrentCultureIgnoreCase)+rootDSEPrefix.Length);
                LDAPQuery = LDAPQuery.Substring(0, LDAPQuery.IndexOf(']'));
                return LDAPQuery;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Return the LDAP Query portion of a NetSqlAzMan LDAPQuery
        /// </summary>
        /// <param name="LDAPQuery">The NetSqlAzMan LDAP Query</param>
        /// <returns>the LDAP Query portion</returns>
        public static string GetLDAPQueryPart(string LDAPQuery)
        {
            string rootDSEPrefix = "[RootDSE:";
            if (!String.IsNullOrEmpty(LDAPQuery) && LDAPQuery.Trim().StartsWith(rootDSEPrefix, StringComparison.CurrentCultureIgnoreCase))
            {
                LDAPQuery = LDAPQuery.Substring(LDAPQuery.IndexOf(']')+1);
                return LDAPQuery;
            }
            else
            {
                return LDAPQuery;
            }
        }

        /// <summary>
        /// Sets the active directory look up credential.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void SetActiveDirectoryLookUpCredential(string username, string password)
        {
            DirectoryServicesUtils.userName = username;
            DirectoryServicesUtils.password = password;
        }

        /// <summary>
        /// News the directory entry.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static DirectoryEntry newDirectoryEntry(string path)
        {
            if (!String.IsNullOrEmpty(DirectoryServicesUtils.userName) && !String.IsNullOrEmpty(DirectoryServicesUtils.password))
                return new DirectoryEntry(path, DirectoryServicesUtils.userName, DirectoryServicesUtils.password);
            else
                return new DirectoryEntry(path);
        }

        /// <summary>
        /// Attributes the values multi string.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="objectDn">The object dn.</param>
        /// <param name="valuesCollection">The values collection.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ArrayList AttributeValuesMultiString(string attributeName, string objectDn, ArrayList valuesCollection, bool recursive)
        {
            DirectoryEntry ent = new DirectoryEntry(objectDn);
            PropertyValueCollection ValueCollection = ent.Properties[attributeName];
            IEnumerator en = ValueCollection.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current != null)
                {
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, "LDAP://" +
                            en.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }

        /// <summary>
        /// Groupses the specified user dn.
        /// </summary>
        /// <param name="userDn">The user dn.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ArrayList Groups(string userDn, bool recursive)
        {
            ArrayList groupMemberships = new ArrayList();
            return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive);
        }

        /// <summary>
        /// Gets all domain users.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllDomainUsers()
        {
            try
            {
                List<string> upns = new List<string>();
                DirectoryEntry root = DirectoryServicesUtils.newDirectoryEntry("LDAP://" + SqlAzManStorage.RootDSEPath);
                root.RefreshCache();
                DirectorySearcher ds = new DirectorySearcher(root, String.Format("(&(objectClass={0}))", "user"));
                ds.PropertiesToLoad.Add("userPrincipalName");
                SearchResultCollection src = ds.FindAll();
                foreach (SearchResult sr in src)
                {
                    if (sr.Properties["userPrincipalName"] != null && sr.Properties["userPrincipalName"].Count > 0)
                    {
                        string upn = sr.Properties["userPrincipalName"][0].ToString();
                        upns.Add(upn);
                    }
                }
                upns.Sort();
                return upns.ToArray();
            }
            catch
            {
                return new string[0];
            }
        }
    }
}
