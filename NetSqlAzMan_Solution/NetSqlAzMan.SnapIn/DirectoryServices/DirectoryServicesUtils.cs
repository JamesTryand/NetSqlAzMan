using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Text;
using ObjectPickerHelper2Lib;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;
using System.Security.Principal;
using NetSqlAzMan.Logging;

namespace NetSqlAzMan.SnapIn.DirectoryServices
{
    /// <summary>
    /// DirectoryServicesUtils class. Provides methods to manipulate AD Objects.
    /// </summary>
    public static class DirectoryServicesUtils
    {
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
        /// Gets the sid info.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="memberName">Name of the sid.</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        public static void GetMemberInfo(string sid, out string memberName, out bool isLocal)
        {
            NetSqlAzMan.DirectoryServices.DirectoryServicesUtils.GetMemberInfo(sid, out memberName, out isLocal);
        }

        /// <summary>
        /// Gets the sid info.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="memberName">Name of the sid.</param>
        /// <param name="isAGroup">if set to <c>true</c> [is A group].</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        public static void GetMemberInfo(string sid, out string memberName, out bool isAGroup, out bool isLocal)
        {
            NetSqlAzMan.DirectoryServices.DirectoryServicesUtils.GetMemberInfo(sid, out memberName, out isAGroup, out isLocal);
        }

        /// <summary>
        /// ADs the object picker show dialog.
        /// </summary>
        /// <param name="handle">The owner handle.</param>
        /// <param name="showLocalUsersAndGroups">if set to <c>true</c> [show local users and groups].</param>
        /// <returns></returns>
        public static ADObject[] ADObjectPickerShowDialog(IntPtr handle, bool showLocalUsersAndGroups)
        {
            return DirectoryServicesUtils.ADObjectPickerShowDialog(handle, showLocalUsersAndGroups, false, true);
        }

        /// <summary>
        /// ADs the object picker show dialog.
        /// </summary>
        /// <param name="handle">The owner handle.</param>
        /// <param name="showLocalUsersAndGroups">if set to <c>true</c> [show local users and groups].</param>
        /// <param name="showOnlyUsers">if set to <c>true</c> [show only users].</param>
        /// <param name="multipleSelection">if set to <c>true</c> [multiple selection].</param>
        /// <returns></returns>
        public static ADObject[] ADObjectPickerShowDialog(IntPtr handle, bool showLocalUsersAndGroups, bool showOnlyUsers, bool multipleSelection)
        {
            try
            {
                // Initialize 1st search scope			

                uint flType = 0;

                flType = flType |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE |
                    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE;
                //DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_WORKGROUP;

                if (showLocalUsersAndGroups)
                    flType = flType | DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER;


                uint flScope =
                    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP |
                    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS |
                    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT |
                    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE |
                    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH; // Starting !?;

                if (!showOnlyUsers)
                    flScope = flScope | DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS;


                uint flBothModes =
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS;

                if (!showOnlyUsers)
                    flBothModes = flBothModes |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_WELL_KNOWN_PRINCIPALS;

                uint flDownlevel =
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ANONYMOUS |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_BATCH |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_DIALUP |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_INTERACTIVE |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SERVICE |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SYSTEM |
                    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER |
                    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS;
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_WORLD;

                if (!showOnlyUsers)
                {
                    flDownlevel = flDownlevel
                        | DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS
                        | DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS
                        | DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS;
                }



                ADObjectPickerClass cadObjectPicker = new ADObjectPickerClass();
                cadObjectPicker.InitInfo_OptionFlags = DSOP_INIT_INFO_FLAGS.DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK;
                if (multipleSelection)
                {
                    cadObjectPicker.InitInfo_OptionFlags = cadObjectPicker.InitInfo_OptionFlags
                        | DSOP_INIT_INFO_FLAGS.DSOP_FLAG_MULTISELECT;
                }

                cadObjectPicker.ScopeTypeFlags = flType;
                cadObjectPicker.ScopeFlags = flScope;
                cadObjectPicker.UplevelFilterFlags_Both = flBothModes;
                cadObjectPicker.DownLevelFilterFlags = flDownlevel;
                cadObjectPicker.InvokeDialog(handle.ToInt32());
                ADObjectColl result = (ADObjectColl)cadObjectPicker.ADObjectsColl;
                ADObject[] results = new ADObject[result.Count];
                for (uint j = 1; j <= result.Count; j++)
                {
                    try
                    {
                        int i = (int)j;
                        ADObjectInfo info = (ADObjectInfo)result.Item(i);
                        results[j - 1] = new ADObject();
                        results[j - 1].ADSPath = info.ADPath;
                        results[j - 1].ClassName = info.Class;
                        results[j - 1].Name = info.Name;
                        results[j - 1].UPN = info.UPN;
                    }
                    catch
                    {
                        continue;
                    }
                }
                return results;
            }
            catch (System.ArgumentException)
            {
                return new ADObject[0];
            }
        }

        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <param name="lDapQuery">The l dap query.</param>
        /// <returns></returns>
        public static SearchResultCollection ExecuteLDAPQuery(string lDapQuery)
        {
            DirectoryEntry root = new DirectoryEntry("LDAP://" + SqlAzManStorage.RootDSEPath);
            root.RefreshCache();
            DirectorySearcher searcher = new DirectorySearcher(root, lDapQuery, new string[] { "objectSid" });
            return searcher.FindAll();
        }
    }
}
