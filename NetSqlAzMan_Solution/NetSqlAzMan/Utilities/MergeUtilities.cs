using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSqlAzMan.Utilities
{
    /// <summary>
    /// Merge Utilities
    /// </summary>
    public static class MergeUtilities
    {
        /// <summary>
        /// Determines whether the specified merge options is on.
        /// </summary>
        /// <param name="mergeOptions">The merge options.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        /// 	<c>true</c> if the specified merge options is on; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOn(SqlAzManMergeOptions mergeOptions, SqlAzManMergeOptions option)
        {
            return (option & mergeOptions) == option;
        }

        internal static void changeSid(XmlElement node, string oldSid, string newSid)
        {
            if (node != null)
            {
                if (node.Attributes["Sid"] != null && node.Attributes["Sid"].Value == oldSid)
                {
                    node.Attributes["Sid"].Value = newSid;
                }
                foreach (var child in node.ChildNodes)
                {
                    if (child as XmlElement!=null)
                        MergeUtilities.changeSid((XmlElement)child, oldSid, newSid);
                }
            }
        }
    }
}
