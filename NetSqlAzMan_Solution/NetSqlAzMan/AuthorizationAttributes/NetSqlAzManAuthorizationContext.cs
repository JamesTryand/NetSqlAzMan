using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using NetSqlAzMan.Interfaces;
using System.Reflection;
using System.Collections;
using System.ServiceModel;

namespace NetSqlAzMan
{
    /// <summary>
    /// NetSqlAzManAuthorizationContext Class.
    /// </summary>
    public class NetSqlAzManAuthorizationContext
    {
        #region Events
        /// <summary>
        /// Occurs when [before check access].
        /// </summary>
        public event BeforeCheckAccessHandler BeforeCheckAccess;
        /// <summary>
        /// Occurs when [after check access].
        /// </summary>
        public event AfterCheckAccessHandler AfterCheckAccess;
        #endregion Events
        #region Fields & Properties
        internal string _storageConnectionString;
        internal EndpointAddress _WCFCacheServiceEndPoint;
        /// <summary>
        /// Gets or sets the name of the store.
        /// </summary>
        /// <value>The name of the store.</value>
        public string StoreName { get; set; }
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }

        internal WindowsIdentity _windowIdentity;
        internal IAzManDBUser _dbuserIdentity;

        /// <summary>
        /// Gets or sets the windows identity.
        /// </summary>
        /// <value>The windows identity.</value>
        public WindowsIdentity WindowsIdentity
        {
            get
            {
                return this._windowIdentity;
            }
            set
            {
                this._windowIdentity = value;
                this._dbuserIdentity = null;
            }
        }

        /// <summary>
        /// Gets or sets the DB user identity.
        /// </summary>
        /// <value>The DB user identity.</value>
        public IAzManDBUser DBUserIdentity
        {
            get
            {
                return this._dbuserIdentity;
            }
            set
            {
                this._dbuserIdentity = value;
                this._windowIdentity = null;
            }
        }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>The storage connection string.</value>
        public string StorageConnectionString
        {
            get
            {
                return this._storageConnectionString;
            }
            set
            {
                this._storageConnectionString = value;
                this._WCFCacheServiceEndPoint = null;
            }
        }

        /// <summary>
        /// Gets or sets the WCF cache service URL.
        /// </summary>
        /// <value>The WCF cache service URL.</value>
        public EndpointAddress WCFCacheServiceEndPoint
        {
            get
            {
                return this._WCFCacheServiceEndPoint;
            }
            set
            {
                this._WCFCacheServiceEndPoint = value;
                this._storageConnectionString = null;
            }
        }
        #endregion Fields & Properties
        #region Methods
        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        public NetSqlAzManAuthorizationContext(string storageConnectionString, string storeName, string applicationName, WindowsIdentity windowsIdentity)
        {
            this.StorageConnectionString = storageConnectionString;
            this.StoreName = storeName;
            this.ApplicationName = applicationName;
            this.WindowsIdentity = windowsIdentity;
        }
        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="dbUserIdentity">The db user identity.</param>
        public NetSqlAzManAuthorizationContext(string storageConnectionString, string storeName, string applicationName, IAzManDBUser dbUserIdentity)
        {
            this.StorageConnectionString = storageConnectionString;
            this.StoreName = storeName;
            this.ApplicationName = applicationName;
            this.DBUserIdentity = dbUserIdentity;
        }
        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="WCFCacheServiceEndPoint">The WCF cache service end point.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        public NetSqlAzManAuthorizationContext(EndpointAddress WCFCacheServiceEndPoint, string storeName, string applicationName, WindowsIdentity windowsIdentity)
        {
            this.WCFCacheServiceEndPoint = WCFCacheServiceEndPoint;
            this.StoreName = storeName;
            this.ApplicationName = applicationName;
            this.WindowsIdentity = windowsIdentity;
        }
        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="WCFCacheServiceEndPoint">The WCF cache service end point.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="dbUserIdentity">The db user identity.</param>
        public NetSqlAzManAuthorizationContext(EndpointAddress WCFCacheServiceEndPoint, string storeName, string applicationName, IAzManDBUser dbUserIdentity)
        {
            this.WCFCacheServiceEndPoint = WCFCacheServiceEndPoint;
            this.StoreName = storeName;
            this.ApplicationName = applicationName;
            this.DBUserIdentity = dbUserIdentity;
        }
        private void internalCheckSecurity(object o, List<int> processedControls)
        {
            // Avoid stack overflow due to properties like Parent 
            // or TopLevelControl         
            if (o == null)
                return;

            int hash = o.GetHashCode();
            bool processed = false;
            foreach (int ctrlHash in processedControls)
            {
                if (ctrlHash == hash)
                {
                    processed = true;
                    break;
                }
            }

            if (processed == true)
                return;

            processedControls.Add(hash);

            System.Reflection.FieldInfo[] fields =
                o.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            foreach (System.Reflection.FieldInfo field in fields)
            {
                object[] attrs =
                    field.GetCustomAttributes(
                    typeof(NetSqlAzManAuthorizationAttribute), true);
                if (attrs.Length > 0)
                {
                    // We only allow one attribute (AllowMultiple = false)
                    NetSqlAzManAuthorizationAttribute attr = (NetSqlAzManAuthorizationAttribute)attrs[0];

                    if (this.BeforeCheckAccess != null)
                    {
                        this.BeforeCheckAccess(this, attr);
                    }
                    bool result = attr.HasAccess(this, field.Name, attr.ItemName);
                    if (this.AfterCheckAccess != null)
                    {
                        this.AfterCheckAccess(this, attr, ref result);
                    }
                    if (!result)
                    {
                        object ctrl = field.GetValue(o);
                        PropertyInfo propInfo = ctrl.GetType().GetProperty(attr.PropertyName);
                        if (propInfo != null)
                        {
                            propInfo.SetValue(ctrl, attr.PropertyValue, null);
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("Property {0}.{1}.{2} not found", o.GetType().Name, field.Name, attr.PropertyName));
                        }
                    }

                }

                if (field.GetType().IsClass)
                {
                    object childO = field.GetValue(o);

                    if (childO is System.ComponentModel.Component)
                    {
                        internalCheckSecurity(childO, processedControls);
                    }
                    else if (childO is IEnumerable)
                    {
                        foreach (object item in (IEnumerable)childO)
                        {
                            if (item is System.ComponentModel.Component)
                            {
                                internalCheckSecurity(item, processedControls);
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Checks the security.
        /// </summary>
        /// <param name="o">The o.</param>
        public void CheckSecurity(object o)
        {
            this.internalCheckSecurity(o, new List<int>());
        }
        #endregion Methods
    }
}
