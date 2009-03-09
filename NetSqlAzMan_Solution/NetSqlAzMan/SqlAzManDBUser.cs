using System;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;
using System.Runtime.Serialization;

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
        #endregion Fields
        #region Constructors
        internal SqlAzManDBUser(IAzManSid customSid, string userName)
        {
            this.customSid = customSid;
            this.userName = userName;
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

        #endregion
    }
}

 