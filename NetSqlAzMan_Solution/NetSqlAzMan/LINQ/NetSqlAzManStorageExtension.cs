using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace NetSqlAzMan.LINQ
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NetSqlAzManStorageDataContext
    {
        partial void OnCreated()
        {
            this.ObjectTrackingEnabled = true;
        }
    }
}
