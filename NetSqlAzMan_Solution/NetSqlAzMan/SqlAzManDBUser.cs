using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan
{
    /// <summary>
    /// SqlAzManDBUser class for custom DB User.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManDBUser : IAzManDBUser
    {
        #region Fields
        private IAzManSid customSid;
        private string userName;
        private Dictionary<string, object> customColumns;
        #endregion Fields
        #region Constructors
        internal SqlAzManDBUser(IAzManSid customSid, string userName)
        {
            this.customSid = customSid;
            this.userName = userName;
            this.customColumns = new Dictionary<string, object>();
        }
        internal SqlAzManDBUser(DataRow DBUserDataRow)
        {
            this.customSid = new SqlAzManSID((byte[])DBUserDataRow["DBUserSid"], true);
            this.userName = (string)DBUserDataRow["DBUserName"];
            this.customColumns = new Dictionary<string, object>();
            foreach (DataColumn dc in DBUserDataRow.Table.Columns)
            {
                if (String.Compare(dc.ColumnName, "DBUserSid", true) != 0
                    &&
                    String.Compare(dc.ColumnName, "DBUserName", true) != 0)
                {
                    this.customColumns.Add(dc.ColumnName, DBUserDataRow[dc]);
                }
            }
        }

        #endregion Constructors
        #region IAzManDBUser Members

        /// <summary>
        /// Custom Unique identifier of the DB User
        /// </summary>
        /// <value></value>
        public IAzManSid CustomSid
        {
            get
            {
                return this.customSid;
            }
        }

        /// <summary>
        /// Username of the DB User
        /// </summary>
        /// <value></value>
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }
        /// <summary>
        /// Gets the custom columns.
        /// </summary>
        /// <value>The custom columns.</value>
        public Dictionary<string, object> CustomColumns
        {
            get
            {
                if (this.customColumns == null)
                    this.customColumns = new Dictionary<string, object>();
                return this.customColumns;
            }
        }
        #endregion
    }
}

