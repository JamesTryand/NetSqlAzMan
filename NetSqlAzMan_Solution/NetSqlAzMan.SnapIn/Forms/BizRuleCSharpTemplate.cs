using System;
using System.Security.Principal;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace MyApplication.BizRules
{
    public sealed class BizRule : IAzManBizRule
    {
        public BizRule()
        { }

        public bool Execute(Hashtable contextParameters, WindowsIdentity identity, IAzManItem ownerItem)
        {
            return true;
        }
    }
}
