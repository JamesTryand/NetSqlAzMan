using System;
using System.Collections;
using System.Reflection.Emit;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.DirectoryServices;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using NetSqlAzMan;
using NetSqlAzMan.CodeDom;
using NetSqlAzMan.DirectoryServices;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Utilities;


namespace NetSqlAzMan
{
    /// <summary>
    /// Represents an AzManItem stored on Sql Server.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManItem : IAzManItem
    {
        #region Fields
        [NonSerialized()]
        internal NetSqlAzManStorageDataContext db;
        private int itemId;
        private ItemType itemType;
        private IAzManApplication application;
        private string name;
        private string bizRuleSource;
        private BizRuleSourceLanguage? bizRuleSourceLanguage;
        private string description;
        private static HybridDictionary bizRuleAssemblyCache = new HybridDictionary();
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<string, IAzManItem> members;
        internal Dictionary<string, IAzManItem> itemsWhereIAmAMember;
        internal Dictionary<string, IAzManAttribute<IAzManItem>> attributes;
        internal List<IAzManAuthorization> authorizations;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Deleted.
        /// </summary>
        public event ItemDeletedDelegate ItemDeleted;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Updated.
        /// </summary>
        public event ItemUpdatedDelegate ItemUpdated;
        /// <summary>
        /// Occurs after a Biz Rule has been Updated.
        /// </summary>
        public event BizRuleUpdatedDelegate BizRuleUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Renamed.
        /// </summary>
        public event ItemRenamedDelegate ItemRenamed;
        /// <summary>
        /// Occurs after an Authorization object has been Created.
        /// </summary>
        public event AuthorizationCreatedDelegate AuthorizationCreated;
        /// <summary>
        /// Occurs after a Delegate has been Created.
        /// </summary>
        public event DelegateCreatedDelegate DelegateCreated;
        /// <summary>
        /// Occurs after a Delegate has been Deleted.
        /// </summary>
        public event DelegateDeletedDelegate DelegateDeleted;
        /// <summary>
        /// Occurs after an Item object has been Added as a member Item.
        /// </summary>
        public event MemberAddedDelegate MemberAdded;
        /// <summary>
        /// Occurs after an Item object has been Removed as a member Item.
        /// </summary>
        public event MemberRemovedDelegate MemberRemoved;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManItem> ItemAttributeCreated;
        #endregion Events
        #region Private Event Raisers
        private void raiseBizRuleUpdated(IAzManItem item, string oldBizRule)
        { 
            if (this.BizRuleUpdated != null)
                this.BizRuleUpdated(item, oldBizRule);
        }
        private void raiseItemDeleted(IAzManApplication applicationContainer, string itemName, ItemType itemType)
        {
            if (this.ItemDeleted != null)
                this.ItemDeleted(applicationContainer, itemName, itemType);
        }
        private void raiseItemUpdated(IAzManItem item, string oldDescription)
        {
            if (this.ItemUpdated != null)
                this.ItemUpdated(item, oldDescription);
        }
       
        private void raiseItemRenamed(IAzManItem item, string oldName)
        {
            if (this.ItemRenamed != null)
                this.ItemRenamed(item, oldName);
        }
        private void raiseAuthorizationCreated(IAzManItem item, IAzManAuthorization authorizationCreated)
        {
            if (this.AuthorizationCreated != null)
                this.AuthorizationCreated(item, authorizationCreated);
        }
        private void raiseDelegateCreated(IAzManItem item, IAzManAuthorization delegationCreated)
        {
            if (this.DelegateCreated != null)
                this.DelegateCreated(item, delegationCreated);
        }
        private void raiseDelegateDeleted(IAzManItem item, IAzManSid delegatingUserSid, IAzManSid delegateUserSid, RestrictedAuthorizationType authorizationType)
        {
            if (this.DelegateDeleted != null)
                this.DelegateDeleted(item, delegatingUserSid, delegateUserSid, authorizationType);
        }
        private void raiseMemberAdded(IAzManItem item, IAzManItem member)
        {
            if (this.MemberAdded != null)
                this.MemberAdded(item, member);
        }
        private void raiseMemberRemoved(IAzManItem item, IAzManItem member)
        {
            if (this.MemberRemoved != null)
                this.MemberRemoved(item, member);
        }
        private void raiseItemAttributeCreated(IAzManItem owner, IAzManAttribute<IAzManItem> attributeCreated)
        {
            if (this.ItemAttributeCreated != null)
                this.ItemAttributeCreated(owner, attributeCreated);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManItem(NetSqlAzManStorageDataContext db, IAzManApplication application, int itemId, string name, string description, ItemType itemType, string bizRule, BizRuleSourceLanguage? bizRuleScriptLanguage, SqlAzManENS ens)
        {
            this.db = db;
            this.itemId = itemId;
            this.application = application;
            this.name = name;
            this.bizRuleSource = bizRule;
            this.bizRuleSourceLanguage = bizRuleScriptLanguage;
            this.description = description;
            this.itemType = itemType;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManItem Members
        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <value>The authorizations.</value>
        public System.Collections.ObjectModel.ReadOnlyCollection<IAzManAuthorization> Authorizations
        {
            get
            {
                if (this.authorizations == null)
                {
                    this.authorizations = new List<IAzManAuthorization>(this.GetAuthorizations());
                }
                return this.authorizations.AsReadOnly();
            }
        }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public Dictionary<string, IAzManAttribute<IAzManItem>> Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new Dictionary<string, IAzManAttribute<IAzManItem>>();
                    foreach (IAzManAttribute<IAzManItem> i in this.GetAttributes())
                    {
                        this.attributes.Add(i.Key, i);
                    }
                }
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        public Dictionary<string, IAzManItem> Members
        {
            get
            {
                if (this.members == null)
                {
                    this.GetMembers();
                }
                return this.members;
            }
        }
        /// <summary>
        /// Gets the items where I am A member.
        /// </summary>
        /// <value>The items where I am A member.</value>
        public Dictionary<string, IAzManItem> ItemsWhereIAmAMember
        {
            get
            {
                if (this.itemsWhereIAmAMember == null)
                {
                    this.GetItemsWhereIAmAMember();
                }
                return this.itemsWhereIAmAMember;
            }
        }
        /// <summary>
        /// Gets the itemName id.
        /// </summary>
        /// <value>The itemName id.</value>
        int IAzManItem.ItemId
        {
            get
            {
                return this.itemId;
            }
        }

        /// <summary>
        /// Gets the type of the itemName.
        /// </summary>
        /// <value>The type of the itemName.</value>
        public ItemType ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IAzManApplication Application
        {
            get
            {
                return this.application;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the biz rule.
        /// </summary>
        /// <value>The biz rule.</value>
        public string BizRuleSource
        {
            get
            {
                return this.bizRuleSource;
            }
        }

        /// <summary>
        /// Gets the biz rule source language.
        /// </summary>
        /// <value>The biz rule script language.</value>
        public BizRuleSourceLanguage? BizRuleSourceLanguage
        {
            get
            {
                return this.bizRuleSourceLanguage;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Updates the specified itemName description.
        /// </summary>
        /// <param name="newItemDescription">The new itemName description.</param>
        public void Update(string newItemDescription)
        {
            string oldDescription = this.description;
            this.db.ItemUpdate(this.name, newItemDescription, (byte)this.itemType, this.itemId, this.application.ApplicationId);
            this.description = newItemDescription;
            //Update cached items
            var cacheItems = ((SqlAzManApplication)this.application).items;
            if (cacheItems != null && cacheItems.ContainsKey(this.name))
                ((SqlAzManItem)cacheItems[this.name]).description = newItemDescription;
            this.description = newItemDescription;
            this.raiseItemUpdated(this, oldDescription);
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <returns></returns>
        public IAzManItem[] GetMembers()
        {
            var dt = from v in this.db.ItemsHierarchyView
                     where v.ItemId == this.itemId
                     select v.MemberName;
            IAzManItem[] result = new IAzManItem[dt.Count()];
            int index=0;
            this.members = new Dictionary<string, IAzManItem>();
            foreach (var row in dt)
            {
                result[index] = this.application.Items[row];
                this.members.Add(result[index].Name, result[index]);
                index++;
            }
            return result;
        }

        /// <summary>
        /// Gets the Items where I'am a member.
        /// </summary>
        /// <returns></returns>
        public IAzManItem[] GetItemsWhereIAmAMember()
        {
            var dt = from v in this.db.ItemsHierarchyView
                     where v.MemberItemId == this.itemId
                     select v.Name;
            IAzManItem[] result = new IAzManItem[dt.Count()];
            int index = 0;
            this.itemsWhereIAmAMember = new Dictionary<string, IAzManItem>();
            foreach (var row in dt)
            {
                result[index] = this.application.Items[row];
                this.itemsWhereIAmAMember.Add(result[index].Name, result[index]);
                index++;
            }
            return result;
        }

        private bool detectLoop(IAzManItem memberToAdd)
        {
            bool loopDetected = false;
            IAzManItem[] membersOfItemToAdd = memberToAdd.GetMembers();
            foreach (IAzManItem member in membersOfItemToAdd)
            {
                if (member.Name == this.name)
                {
                    return true;
                }
                else
                {
                    if (this.detectLoop(member))
                    {
                        loopDetected = true;
                    }
                }
            }
            return loopDetected;
        }

        /// <summary>
        /// Determines whether an Item can be a member of a parent Item.
        /// </summary>
        /// <param name="parentItem">The parent itemName.</param>
        /// <param name="childItem">The child itemName.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can be an itemName A son of] the specified parent itemName; otherwise, <c>false</c>.
        /// </returns>
        public static bool MembershipAllowed(IAzManItem parentItem, IAzManItem childItem)
        {
            //Parent Item
            switch (parentItem.ItemType)
            { 
                case ItemType.Role:
                    return true; //All child of all types are allowed.
                case ItemType.Task:
                    switch (childItem.ItemType)
                    { 
                        case ItemType.Task:
                        case ItemType.Operation:
                            return true;
                        default:
                            return false;
                    }
                case ItemType.Operation:
                    switch (childItem.ItemType)
                    {
                        case ItemType.Operation:
                            return true;
                        default:
                            return false;
                    }
            }
            return false;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="member">The member.</param>
        public void AddMember(IAzManItem member)
        {
            //Membership type check
            if (!SqlAzManItem.MembershipAllowed(this, member))
                throw new SqlAzManItemException(this, String.Format("Membership not allowed. Cannot add an item of type {0} to an item of type {1}.", member.ItemType, this.itemType));
            //Loop detection
            if (this.detectLoop(member))
                throw new SqlAzManItemException(this, String.Format("Cannot add '{0}' as a member. A loop has been detected.", member.Name));
            this.db.ItemsHierarchyInsert(member.ItemId, this.itemId, this.application.ApplicationId);
            //Update cached item members
            if (this.members!=null && !this.members.ContainsKey(member.Name))
                this.members.Add(member.Name, member);
            this.raiseMemberAdded(this, member);
        }

        /// <summary>
        /// Removes the member.
        /// </summary>
        /// <param name="member">The member.</param>
        public void RemoveMember(IAzManItem member)
        {
            //Membership type check
            if (!SqlAzManItem.MembershipAllowed(this, member))
                throw new SqlAzManItemException(this, String.Format("Membership not allowed. Cannot add an item of type {0} to an item of type {1}.", member.ItemType, this.itemType));
            if (this.db.ItemsHierarchy().Any(r=>r.ItemId == member.ItemId && r.MemberOfItemId == this.itemId))
            {
                this.db.ItemsHierarchyDelete(member.ItemId, this.itemId, this.application.ApplicationId);
                //Invalidate cached members
                if (this.members!=null && this.members.ContainsKey(member.Name))
                    this.members.Remove(member.Name);
                this.raiseMemberRemoved(this, member);
            }
        }

        private void changeItemNameEverywhere(Dictionary<string, IAzManItem> members, string oldItemName, string newItemName)
        {
            if (members == null || members.Count == 0)
                return;
            if (members.ContainsKey(oldItemName))
            {
                var oldItem = members[oldItemName];
                members.Remove(oldItemName);
                ((SqlAzManItem)oldItem).name = newItemName;
                members.Add(newItemName, oldItem);
            }
            foreach (var key in members.Keys)
            { 
                this.changeItemNameEverywhere(members[key].Members, oldItemName, newItemName);
            }
        }

        private void removeMemberEverywhere(Dictionary<string, IAzManItem> members, string oldItemName)
        {
            if (members == null || members.Count == 0)
                return;
            if (members.ContainsKey(oldItemName))
            {
                members.Remove(oldItemName);
            }
            foreach (var key in members.Keys)
            {
                this.removeMemberEverywhere(members[key].Members, oldItemName);
            }
        }

        /// <summary>
        /// Renames the specified itemName with a new itemName name.
        /// </summary>
        /// <param name="newItemName">New name of the itemName.</param>
        public void Rename(string newItemName)
        {
            try
            {
                if (this.name != newItemName)
                {
                    string oldName = this.name;
                    this.db.ItemUpdate(newItemName, this.description, (byte)this.itemType, this.itemId, this.application.ApplicationId);
                    this.name = newItemName;
                    //Update cached items
                    if (this.application.Items.ContainsKey(oldName))
                    {
                        var oldItem = this.application.Items[oldName];
                        this.application.Items.Remove(oldName);
                        ((SqlAzManItem)oldItem).name = newItemName;
                        this.application.Items.Add(newItemName, oldItem);
                    }
                    foreach (var item in this.application.Items.Values)
                    {
                        this.changeItemNameEverywhere(((SqlAzManItem)item).members, oldName, newItemName);
                    }
                    this.raiseItemRenamed(this, oldName);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw new SqlAzManItemException(this, "An Item with the same name already exists.");
                else
                    throw sqlex;
            }
        }

        /// <summary>
        /// Loads the biz rule compiled assembly.
        /// </summary>
        /// <returns></returns>
        public Assembly LoadBizRuleAssembly()
        {
            object cachedBizRule;
            lock (SqlAzManItem.bizRuleAssemblyCache)
            {
                cachedBizRule = SqlAzManItem.bizRuleAssemblyCache[this.itemId];
            }
            if (cachedBizRule == null)
            {
                var item = (from t in this.db.Items() where t.ItemId == this.itemId select t).FirstOrDefault();
                if (item!=null && !item.BizRuleId.HasValue)
                {
                    return null;
                }
                var b = (from br in this.db.BizRules()
                        where br.BizRuleId == item.BizRuleId
                        select br).First();
                Assembly result = Assembly.Load(b.CompiledAssembly.ToArray(), null, this.GetType().Assembly.Evidence);
                try
                {
                    //Try to load Types
                    Type[] types = result.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException)
                {
                    //Stored assembly has a reference to an older version of NetSqlAzMan.dll
                    //Must be recompiled recompiled
                    this.compileBizRuleAssembly(b.BizRuleSource, (BizRuleSourceLanguage)b.BizRuleLanguage, out result);
                    //And ... If I Am a Manager
                    if (this.application.IAmManager)
                    {
                        //... rewrite compiled assembly into the Storage.
                        this.ReloadBizRule(b.BizRuleSource, (BizRuleSourceLanguage)b.BizRuleLanguage);
                    }
                }
                //Put compiled assembly into the assembly cache.
                lock (SqlAzManItem.bizRuleAssemblyCache)
                {
                    SqlAzManItem.bizRuleAssemblyCache.Add(this.itemId, result);
                }
                return result;
            }
            else
            {
                //Get compiled assembly from assembly cache.
                return (Assembly)cachedBizRule;
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string s = args.Name;
            return null;
        }

        /// <summary>
        /// Clears the biz rule.
        /// </summary>
        public void ClearBizRule()
        {
            bool txBeginned = ((SqlAzManStorage)(this.application.Store.Storage)).internalBeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                ItemsResult item = (from tf in this.db.Items()
                                   where tf.ItemId == this.itemId
                                   select tf).FirstOrDefault();
                int? bizRuleId = null;
                if (item!=null && item.BizRuleId.HasValue) bizRuleId = item.BizRuleId;
                string oldBizRule = this.bizRuleSource;
                this.db.ClearBizRule(this.itemId, this.application.ApplicationId);
                this.bizRuleSource = String.Empty;
                this.bizRuleSourceLanguage = null;
                if (bizRuleId.HasValue)
                {
                    this.db.BizRuleDelete(bizRuleId.Value, this.application.ApplicationId);
                }
                this.raiseBizRuleUpdated(this, oldBizRule);
                if (txBeginned) ((SqlAzManStorage)(this.application.Store.Storage)).internalCommitTransaction();
            }
            catch
            {
                if (txBeginned) ((SqlAzManStorage)(this.application.Store.Storage)).internalRollBackTransaction();
                throw;
            }
        }

        private byte[] compileBizRuleAssembly(string bizRule, BizRuleSourceLanguage bizRuleLanguage, out Assembly compiledAssembly)
        {
            CodeDomProvider provider = null;
            if (bizRuleLanguage == NetSqlAzMan.BizRuleSourceLanguage.CSharp)
                provider = new Microsoft.CSharp.CSharpCodeProvider();
            else
                provider = new Microsoft.VisualBasic.VBCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = false;
            cp.GenerateExecutable = false;
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Data.OracleClient.dll");
            cp.ReferencedAssemblies.Add("System.DirectoryServices.dll");
            cp.ReferencedAssemblies.Add("System.Messaging.dll");
            cp.ReferencedAssemblies.Add("System.Security.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("System.Xml.dll");
            Assembly netsqlazmanAassembly = this.GetType().Assembly;
            if (netsqlazmanAassembly.CodeBase.StartsWith("file:///"))
            {
                cp.ReferencedAssemblies.Add(netsqlazmanAassembly.CodeBase.Substring(8).Replace('/','\\'));
            }
            else
            {
                cp.ReferencedAssemblies.Add(netsqlazmanAassembly.Location);
            }
            

            CompilerResults cr = null;
            MemoryStream ms = null;
            try
            {
                cp.CompilerOptions += " /filealign:4096 /optimize";
                cr = provider.CompileAssemblyFromSource(cp, bizRule);
                if (cr.Errors.Count > 0)
                {
                    throw new Exception("Compiler Error");
                }
                compiledAssembly = cr.CompiledAssembly;
                int implementsIAzManBizRuleInterfaceCount = 0;
                foreach (Type t in compiledAssembly.GetTypes())
                {
                    Type[] interfaces = t.FindInterfaces(new TypeFilter(
                        delegate(Type typeObj, Object criteriaObj)
                        {
                            if (typeObj.ToString() == criteriaObj.ToString())
                                return true;
                            else
                                return false;
                        }), typeof(NetSqlAzMan.Interfaces.IAzManBizRule).FullName);
                    implementsIAzManBizRuleInterfaceCount += interfaces.Length;
                }
                if (implementsIAzManBizRuleInterfaceCount==0)
                { 
                    throw new Exception("There must be at least a type that implements NetSqlAzMan.Interfaces.IAzManBizRule interface");
                }
                else if (implementsIAzManBizRuleInterfaceCount > 1)
                {
                    throw new Exception("Too many types that implements NetSqlAzMan.Interfaces.IAzManBizRule interface. Maximum allowed is 1.");
                }
                string fullPathAssembly = compiledAssembly.Location;
                FileStream file = File.OpenRead(fullPathAssembly);
                FileInfo fi = new FileInfo(fullPathAssembly);
                byte[] buffer = new byte[(int)fi.Length];
                file.Read(buffer, 0, buffer.Length);
                file.Close();
                file.Dispose();
                return buffer;
            }
            catch (Exception ex)
            {
                StringBuilder outputMessages = new StringBuilder();
                foreach (string msg in cr.Output)
                {
                    outputMessages = outputMessages.AppendFormat("{0}\r\n", msg);
                }
                throw new Exception(String.Format("Biz Rule compile exception.\r\n{0}\r\nOutput:{1}\r\n", ex.Message, outputMessages), ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }

        }

        /// <summary>
        /// Reloads the biz rule.
        /// </summary>
        /// <param name="bizRule">The biz rule.</param>
        /// <param name="bizRuleLanguage">The biz rule language.</param>
        public void ReloadBizRule(string bizRule, BizRuleSourceLanguage bizRuleLanguage)
        {
            bool txBeginned = ((SqlAzManStorage)(this.application.Store.Storage)).internalBeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                string oldBizRule = this.bizRuleSource;
                this.ClearBizRule();
                Assembly compiledAssembly;
                byte[] binaryAssembly = this.compileBizRuleAssembly(bizRule, bizRuleLanguage, out compiledAssembly);
                int id = this.db.BizRuleInsert(bizRule, (byte)bizRuleLanguage, binaryAssembly);
                int bizRuleId = id;
                this.db.ReloadBizRule(this.itemId, bizRuleId, this.application.ApplicationId);
                this.bizRuleSource = bizRule;
                this.bizRuleSourceLanguage = bizRuleLanguage;
                this.raiseBizRuleUpdated(this, oldBizRule);
                if (txBeginned) ((SqlAzManStorage)(this.application.Store.Storage)).internalCommitTransaction();
            }
            catch
            {
                if (txBeginned) ((SqlAzManStorage)(this.application.Store.Storage)).internalRollBackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Deletes this Item.
        /// </summary>
        public void Delete()
        {
            this.db.ItemDelete(this.itemId, this.application.ApplicationId);
            this.raiseItemDeleted(this.application, this.name, this.itemType);
            //Invalidate cached items
            if (this.application.Items.ContainsKey(this.name))
                this.application.Items.Remove(this.name);
            foreach (var item in this.application.Items.Values)
            {
                this.removeMemberEverywhere(((SqlAzManItem)item).members, this.name);
            }
        }

        /// <summary>
        /// Determines whether [has child items].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has child items]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMembers()
        {
            return this.db.ItemsHierarchy().Any(p=>p.MemberOfItemId == this.itemId);
        }

        /// <summary>
        /// Creates the authorization.
        /// </summary>
        /// <param name="owner">The owner owner.</param>
        /// <param name="ownerSidWhereDefined">The owner sid where defined.</param>
        /// <param name="sid">The object owner.</param>
        /// <param name="sidWhereDefined">The object owner where defined.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns></returns>
        public IAzManAuthorization CreateAuthorization(IAzManSid owner, WhereDefined ownerSidWhereDefined, IAzManSid sid, WhereDefined sidWhereDefined, AuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo)
        {
            //DateTime range check
            if (validFrom.HasValue && validTo.HasValue)
            {
                if (validFrom.Value > validTo.Value)
                    throw new InvalidOperationException("ValidFrom cannot be greater then ValidTo if supplied.");
            }
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && sidWhereDefined == WhereDefined.Local)
            {
                throw new SqlAzManItemException(this, "Cannot create an Authorization on members defined on local in Administrator Mode");
            }
            var existing = (from aut in this.db.Authorizations()
                           where aut.ItemId == this.itemId && aut.OwnerSid == owner.BinaryValue && aut.OwnerSidWhereDefined == (byte)ownerSidWhereDefined && aut.ObjectSid == sid.BinaryValue && aut.AuthorizationType == (byte)authorizationType && aut.ValidFrom == validFrom && aut.ValidTo == validTo
                           select aut).FirstOrDefault();
            if (existing == null)
            {
                int id = this.db.AuthorizationInsert(this.itemId, owner.BinaryValue, (byte)ownerSidWhereDefined, sid.BinaryValue, (byte)sidWhereDefined, (byte)authorizationType, (validFrom.HasValue ? validFrom.Value : new DateTime?()), (validTo.HasValue ? validTo.Value : new DateTime?()), this.application.ApplicationId);
                IAzManAuthorization result = new SqlAzManAuthorization(this.db, this, id, owner, ownerSidWhereDefined, sid, sidWhereDefined, authorizationType, validFrom, validTo, this.ens);
                this.raiseAuthorizationCreated(this, result);
                this.ens.AddPublisher(result);
                this.authorizations = null; //Force cache refresh
                return result;
            }
            else
            { 
                IAzManAuthorization result = new SqlAzManAuthorization(this.db, this, existing.ItemId.Value, new SqlAzManSID(existing.OwnerSid.ToArray()), (WhereDefined)existing.OwnerSidWhereDefined, new SqlAzManSID(existing.ObjectSid.ToArray()), (WhereDefined)existing.ObjectSidWhereDefined, (AuthorizationType)existing.AuthorizationType.Value, existing.ValidFrom, existing.ValidTo, this.ens);
                return result;
            }
        }

        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IAzManAuthorization[] GetAuthorizations(AuthorizationType type)
        {

            var auths = from tf in this.db.Authorizations()
                        where
                        (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && tf.ObjectSidWhereDefined != (byte)WhereDefined.Local
                        ||
                        this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                        &&
                        tf.ItemId == this.itemId && tf.AuthorizationType == (byte)(type)
                        select tf;
            int index = 0;
            IAzManAuthorization[] authorizations = new SqlAzManAuthorization[auths.Count()];
            foreach (var row in auths)
            {
                authorizations[index] = new SqlAzManAuthorization(this.db, this, row.AuthorizationId.Value, new SqlAzManSID(row.OwnerSid.ToArray(), row.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.OwnerSidWhereDefined, new SqlAzManSID(row.ObjectSid.ToArray(), row.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.ObjectSidWhereDefined, (AuthorizationType)row.AuthorizationType, (row.ValidFrom.HasValue ? row.ValidFrom : new DateTime?()), (row.ValidTo.HasValue ? row.ValidTo : new DateTime?()), this.ens);
                this.ens.AddPublisher(authorizations[index]);
                index++;
            }
            return authorizations;
        }

        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <returns></returns>
        public IAzManAuthorization[] GetAuthorizations()
        {
            var auths = from tf in this.db.Authorizations()
                        where
                        (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && tf.ObjectSidWhereDefined != (byte)WhereDefined.Local
                        ||
                        this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                        &&
                        tf.ItemId == this.itemId
                        select tf;
            int index = 0;
            IAzManAuthorization[] authorizations = new SqlAzManAuthorization[auths.Count()];
            foreach (var row in auths)
            {
                authorizations[index] = new SqlAzManAuthorization(this.db, this, row.AuthorizationId.Value, new SqlAzManSID(row.OwnerSid.ToArray(), row.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.OwnerSidWhereDefined, new SqlAzManSID(row.ObjectSid.ToArray(), row.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.ObjectSidWhereDefined, (AuthorizationType)row.AuthorizationType, row.ValidFrom, row.ValidTo, this.ens);
                this.ens.AddPublisher(authorizations[index]);
                index++;
            }
            return authorizations;
        }

        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <param name="owner">The owner Sid.</param>
        /// <param name="Sid">The member sid.</param>
        /// <returns></returns>
        public IAzManAuthorization[] GetAuthorizations(IAzManSid owner, IAzManSid Sid)
        {
            var auths = from tf in this.db.Authorizations()
                        where
                        (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && tf.ObjectSidWhereDefined != (byte)WhereDefined.Local
                        ||
                        this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                        &&
                        tf.ItemId == this.itemId && tf.OwnerSid == owner.BinaryValue && tf.ObjectSid == Sid.BinaryValue
                        select tf;
            int index = 0;
            IAzManAuthorization[] authorizations = new SqlAzManAuthorization[auths.Count()];
            foreach (var row in auths)
            {
                authorizations[index] = new SqlAzManAuthorization(this.db, this, row.AuthorizationId.Value, new SqlAzManSID(row.OwnerSid.ToArray(), row.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.OwnerSidWhereDefined, new SqlAzManSID(row.ObjectSid.ToArray(), row.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.ObjectSidWhereDefined, (AuthorizationType)row.AuthorizationType, row.ValidFrom, row.ValidTo, this.ens);
                this.ens.AddPublisher(authorizations[index]);
                index++;
            }
            return authorizations;
        }

        /// <summary>
        /// Gets the authorizations by SID.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        public IAzManAuthorization[] GetAuthorizationsOfMember(IAzManSid sid)
        {
            var aom = from tf in this.db.Authorizations()
                      where
                      (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && tf.ObjectSidWhereDefined != (byte)WhereDefined.Local
                      ||
                      this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                      &&
                      tf.ItemId == this.itemId && tf.ObjectSid == sid.BinaryValue
                      select tf;
            int index = 0;
            IAzManAuthorization[] authorizations = new SqlAzManAuthorization[aom.Count()];
            foreach (var row in aom)
            {
                authorizations[index] = new SqlAzManAuthorization(this.db, this, row.AuthorizationId.Value, new SqlAzManSID(row.OwnerSid.ToArray(), row.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.OwnerSidWhereDefined, new SqlAzManSID(row.ObjectSid.ToArray(), row.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.ObjectSidWhereDefined, (AuthorizationType)row.AuthorizationType, row.ValidFrom, row.ValidTo, this.ens);
                this.ens.AddPublisher(authorizations[index]);
                index++;
            }
            return authorizations;
        }

        /// <summary>
        /// Gets the authorizations by Owner SID.
        /// </summary>
        /// <param name="owner">The owner Sid.</param>
        /// <returns></returns>
        public IAzManAuthorization[] GetAuthorizationsOfOwner(IAzManSid owner)
        {
            var auths = from tf in this.db.Authorizations()
                        where
                        (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && tf.ObjectSidWhereDefined != (byte)WhereDefined.Local
                        ||
                        this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                        &&
                        tf.ItemId == this.itemId && tf.OwnerSid == owner.BinaryValue
                        select tf;
            int index = 0;
            IAzManAuthorization[] authorizations = new SqlAzManAuthorization[auths.Count()];
            foreach (var row in auths)
            {
                authorizations[index] = new SqlAzManAuthorization(this.db, this, row.AuthorizationId.Value, new SqlAzManSID(row.OwnerSid.ToArray(), row.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.OwnerSidWhereDefined, new SqlAzManSID(row.ObjectSid.ToArray(), row.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.ObjectSidWhereDefined, (AuthorizationType)row.AuthorizationType, row.ValidFrom, row.ValidTo, this.ens);
                this.ens.AddPublisher(authorizations[index]);
                index++;
            }
            return authorizations;
        }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="authorizationId">The authorization id.</param>
        /// <returns></returns>
        public IAzManAuthorization GetAuthorization(int authorizationId)
        {
            AuthorizationsResult ar;
            if ((ar = (from t in this.db.Authorizations() where t.ItemId == this.itemId && t.AuthorizationId == authorizationId select t).FirstOrDefault())!=null)
            {
                if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && ar.ObjectSidWhereDefined == (byte)WhereDefined.Local)
                {
                    return null;
                }
                else
                {
                    IAzManAuthorization result = new SqlAzManAuthorization(this.db, this, ar.AuthorizationId.Value, new SqlAzManSID(ar.OwnerSid.ToArray(), ar.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)ar.OwnerSidWhereDefined, new SqlAzManSID(ar.ObjectSid.ToArray(), ar.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)(ar.ObjectSidWhereDefined), (AuthorizationType)ar.AuthorizationType, ar.ValidFrom, ar.ValidTo, this.ens);
                    this.ens.AddPublisher(result);
                    return result;
                }
            }
            else
            {
                return null;
            }
        }

        internal static byte[] getSqlBinarySid(SecurityIdentifier sid)
        { 
            byte[] binarySid = new byte[sid.BinaryLength];
            sid.GetBinaryForm(binarySid,0);
            byte[] result = new byte[85];
            int j = 0;
            for (int i = 85 - sid.BinaryLength; i < 85; i++)
            {
                result[i] = binarySid[j++];
            }
            return result;
        }

        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.CheckAccess(windowsIdentity, validFor, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, this.name, windowsIdentity, validFor, this.itemType == ItemType.Operation, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.CheckAccess(dbUser, validFor, out attributes, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(IAzManDBUser dbUser, DateTime validFor, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, this.name, dbUser, validFor, this.itemType == ItemType.Operation, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.BeginCheckAccess(windowsIdentity, validFor, callBack, stateObject, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.application.Store.Storage.BeginCheckAccess(this.application.Store.Name, this.application.Name, this.name, windowsIdentity, validFor, (this.itemType == ItemType.Operation ? true : false), callBack, stateObject, out attributes, contextParameters);
        }
                
        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(IAzManDBUser dbUser, DateTime validFor, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.BeginCheckAccess(dbUser, validFor, callBack, stateObject, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(IAzManDBUser dbUser, DateTime validFor, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.application.Store.Storage.BeginCheckAccess(this.application.Store.Name, this.application.Name, this.name, dbUser, validFor, (this.itemType == ItemType.Operation ? true : false), callBack, stateObject, out attributes, contextParameters);
        }

        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccess(IAsyncResult asyncResult)
        {
            return this.application.Store.Storage.EndCheckAccess(asyncResult);
        }

        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccess(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes)
        {
            return this.application.Store.Storage.EndCheckAccess(asyncResult, out attributes);
        }

        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult)
        {
            return this.application.Store.Storage.EndCheckAccessForDBUsers(asyncResult);
        }

        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes)
        {
            return this.application.Store.Storage.EndCheckAccessForDBUsers(asyncResult, out attributes);
        }
        /// <summary>
        /// Creates the delegation [Windows Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns>IAzManAuthorization</returns>
        public IAzManAuthorization CreateDelegateAuthorization(WindowsIdentity delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo)
        {
            //DateTime range check
            if (validFrom.HasValue && validTo.HasValue)
            {
                if (validFrom.Value > validTo.Value)
                    throw new InvalidOperationException("ValidFrom cannot be greater then ValidTo if supplied.");
            }
            string delegatedName;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(delegateUser.StringValue, out delegatedName, out isLocal);
            //Check if user has AllowWithDelegation permission on this Item.
            if (this.CheckAccess(delegatingUser, DateTime.Now) != AuthorizationType.AllowWithDelegation)
            {
                string msg = String.Format("Create Delegate permission deny for user '{0}' ({1}) to user '{2}' ({3}).", delegatingUser.Name, delegatingUser.User.Value, delegatedName, delegateUser.StringValue);
                throw new SqlAzManItemException(this, msg);
            }
            WhereDefined sidWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && sidWhereDefined == WhereDefined.Local)
            {
                throw new SqlAzManItemException(this, "Cannot create a Delegate defined on local in Administrator Mode");
            }
            IAzManSid owner = new SqlAzManSID(delegatingUser.User.Value);
            string ownerName;
            bool ownerIsLocal;
            DirectoryServicesUtils.GetMemberInfo(delegatingUser.User.Value, out ownerName, out ownerIsLocal);

            WhereDefined ownerSidWhereDefined = ownerIsLocal ? WhereDefined.Local : WhereDefined.LDAP;
            int? authorizationId = 0;
            this.db.CreateDelegate(this.itemId, owner.BinaryValue, (byte)ownerSidWhereDefined, delegateUser.BinaryValue, (byte)sidWhereDefined, (byte)authorizationType, (validFrom.HasValue ? validFrom.Value : new DateTime?() ), (validTo.HasValue ? validTo.Value : new DateTime?() ), ref authorizationId);
            IAzManAuthorization result = new SqlAzManAuthorization(this.db, this, authorizationId.Value, owner, ownerSidWhereDefined, delegateUser, sidWhereDefined, (AuthorizationType)authorizationType, validFrom, validTo, this.ens);
            this.raiseDelegateCreated(this, result);
            this.ens.AddPublisher(result);
            return result;
        }
        /// <summary>
        /// Creates the delegation [DB Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns>IAzManAuthorization</returns>
        public IAzManAuthorization CreateDelegateAuthorization(IAzManDBUser delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo)
        {
            //DateTime range check
            if (validFrom.HasValue && validTo.HasValue)
            {
                if (validFrom.Value > validTo.Value)
                    throw new InvalidOperationException("ValidFrom cannot be greater then ValidTo if supplied.");
            }
            string delegatedName;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(delegateUser.StringValue, out delegatedName, out isLocal);
            //Check if user has AllowWithDelegation permission on this Item.
            if (this.CheckAccess(delegatingUser, DateTime.Now) != AuthorizationType.AllowWithDelegation)
            {
                string msg = String.Format("Create Delegate permission deny for user '{0}' ({1}) to user '{2}' ({3}).", delegatingUser.UserName, delegatingUser.CustomSid.StringValue, delegatedName, delegateUser.StringValue);
                throw new SqlAzManItemException(this, msg);
            }
            WhereDefined sidWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && sidWhereDefined == WhereDefined.Local)
            {
                throw new SqlAzManItemException(this, "Cannot create a Delegate defined on local in Administrator Mode");
            }
            IAzManSid owner = delegatingUser.CustomSid;
            string ownerName = delegatingUser.UserName;

            WhereDefined ownerSidWhereDefined = WhereDefined.Database;
            int? authorizationId = 0;
            this.db.CreateDelegate(this.itemId, owner.BinaryValue, (byte)ownerSidWhereDefined, delegateUser.BinaryValue, (byte)sidWhereDefined, (byte)authorizationType, (validFrom.HasValue ? validFrom.Value : new DateTime?() ), (validTo.HasValue ? validTo.Value : new DateTime?() ), ref authorizationId);
            IAzManAuthorization result = new SqlAzManAuthorization(this.db, this, authorizationId.Value, owner, ownerSidWhereDefined, delegateUser, sidWhereDefined, (AuthorizationType)authorizationType, validFrom, validTo, this.ens);
            this.raiseDelegateCreated(this, result);
            this.ens.AddPublisher(result);
            return result;
        }
        /// <summary>
        /// Removes the delegate [Windows Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        public void DeleteDelegateAuthorization(WindowsIdentity delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType)
        {
            string delegatedName;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(delegateUser.StringValue, out delegatedName, out isLocal);

            //Check if user has AllowWithDelegation permission on this Item.
            if (this.CheckAccess(delegatingUser, DateTime.Now) != AuthorizationType.AllowWithDelegation)
            {
                string msg = String.Format("Remove Delegate permission deny for user '{0}' ({1}) to user '{2}' ({3}).", delegatingUser.Name, delegatingUser.User.Value, delegatedName, delegateUser.StringValue);
                throw new SqlAzManItemException(this, msg);
            }
            WhereDefined memberWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && memberWhereDefined == WhereDefined.Local)
            {
                throw new SqlAzManItemException(this, "Cannot remove Delegates defined on local in Administrator Mode");
            }
            IAzManSid owner = new SqlAzManSID(delegatingUser.User.Value);
            string ownerName;
            bool ownerIsLocal;
            DirectoryServicesUtils.GetMemberInfo(delegatingUser.User.Value, out ownerName, out ownerIsLocal);

            WhereDefined ownerSidWhereDefined = ownerIsLocal ? WhereDefined.Local : WhereDefined.LDAP;
            foreach (IAzManAuthorization auth in this.GetAuthorizations(owner, delegateUser))
            {
                if ((byte)auth.AuthorizationType == (byte)authorizationType)
                {
                    int affectedRecords = db.DeleteDelegate(auth.AuthorizationId, owner.BinaryValue);
                    if (affectedRecords != 0)
                        this.raiseDelegateDeleted(this, new SqlAzManSID(delegatingUser.User.Value), delegateUser, authorizationType);
                }
            }
        }
        /// <summary>
        /// Removes the delegate [DB Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        public void DeleteDelegateAuthorization(IAzManDBUser delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType)
        {
            string delegatedName;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(delegateUser.StringValue, out delegatedName, out isLocal);

            //Check if user has AllowWithDelegation permission on this Item.
            if (this.CheckAccess(delegatingUser, DateTime.Now) != AuthorizationType.AllowWithDelegation)
            {
                string msg = String.Format("Remove Delegate permission deny for user '{0}' ({1}) to user '{2}' ({3}).", delegatingUser.UserName, delegatingUser.CustomSid.StringValue, delegatedName, delegateUser.StringValue);
                throw new SqlAzManItemException(this, msg);
            }
            WhereDefined memberWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && memberWhereDefined == WhereDefined.Local)
            {
                throw new SqlAzManItemException(this, "Cannot remove Delegates defined on local in Administrator Mode");
            }
            IAzManSid owner = delegatingUser.CustomSid;
            string ownerName = delegatingUser.UserName;

            foreach (IAzManAuthorization auth in this.GetAuthorizations(owner, delegateUser))
            {
                if ((byte)auth.AuthorizationType == (byte)authorizationType)
                {
                    int affectedRecords = db.DeleteDelegate(auth.AuthorizationId, owner.BinaryValue);
                    if (affectedRecords != 0)
                        this.raiseDelegateDeleted(this, delegatingUser.CustomSid, delegateUser, authorizationType);
                }
            }
        }


        /// <summary>
        /// Gets the <see cref="T:IAzManAuthorization[]"/> with the specified owner.
        /// </summary>
        /// <value></value>
        public IAzManAuthorization[] this[IAzManSid owner, IAzManSid sid]
        {
            get { return this.GetAuthorizations(owner, sid); }
        }
        /// <summary>
        /// Gets the itemName attributes.
        /// </summary>
        /// <returns></returns>
        public IAzManAttribute<IAzManItem>[] GetAttributes()
        {

            IAzManAttribute<IAzManItem>[] attributes;
            var ia = from tf in this.db.ItemAttributes()
                     where tf.ItemId == this.itemId
                     select tf;
            attributes = new SqlAzManItemAttribute[ia.Count()];
            int index = 0;
            foreach (var row in ia)
            {
                attributes[index] = new SqlAzManItemAttribute(this.db, this, row.ItemAttributeId.Value, row.AttributeKey, row.AttributeValue, this.ens);
                this.ens.AddPublisher(attributes[index]);
                index++;
            }
            return attributes;
        }

        /// <summary>
        /// Gets the itemName attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManItem> GetAttribute(string key)
        {
            ItemAttributesResult iar;
            if ((iar = (from t in this.db.ItemAttributes() where t.ItemId == this.itemId && t.AttributeKey == key select t).FirstOrDefault())!=null)
            {
                IAzManAttribute<IAzManItem> result = new SqlAzManItemAttribute(this.db, this, iar.ItemAttributeId.Value, iar.AttributeKey, iar.AttributeValue, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates an itemName attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManItem> CreateAttribute(string key, string value)
        {
            try
            {
                int itemAttributeId = this.db.ItemAttributeInsert(this.itemId, key, value, this.application.ApplicationId);
                IAzManAttribute<IAzManItem> result = new SqlAzManItemAttribute(this.db, this, itemAttributeId, key, value, this.ens);
                this.raiseItemAttributeCreated(this, result);
                this.ens.AddPublisher(result);
                return result;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw new SqlAzManItemException(this, "An Item Attribute with the same Key name already exists.");
                else
                    throw sqlex;
            }
        }
        internal static AuthorizationType mergeAuthorizations(AuthorizationType auth1, AuthorizationType auth2)
        {
            if (auth1 == AuthorizationType.AllowWithDelegation)
            {
                if (auth2 == AuthorizationType.AllowWithDelegation)
                {
                    return AuthorizationType.AllowWithDelegation;
                }
                else if (auth2 == AuthorizationType.Allow)
                {
                    return AuthorizationType.AllowWithDelegation;
                }
                else if (auth2 == AuthorizationType.Deny)
                {
                    return AuthorizationType.Deny;
                }
                else if (auth2 == AuthorizationType.Neutral)
                {
                    return AuthorizationType.AllowWithDelegation;
                }
            }
            else if (auth1 == AuthorizationType.Allow)
            {
                if (auth2 == AuthorizationType.AllowWithDelegation)
                {
                    return AuthorizationType.AllowWithDelegation;
                }
                else if (auth2 == AuthorizationType.Allow)
                {
                    return AuthorizationType.Allow;
                }
                else if (auth2 == AuthorizationType.Deny)
                {
                    return AuthorizationType.Deny;
                }
                else if (auth2 == AuthorizationType.Neutral)
                {
                    return AuthorizationType.Allow;
                }
            }
            else if (auth1 == AuthorizationType.Deny)
            {
                return AuthorizationType.Deny;
            }
            //else if (auth1 == AuthorizationType.Neutral)
            //{
            return auth2;
            //}
        }
        #endregion
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
        #region IAzManImportExport Members

        /// <summary>
        /// Exports the specified XML writer.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeDBUsers">if set to <c>true</c> [include DB users].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <param name="ownerOfExport">The owner of export.</param>
        public void Export(System.Xml.XmlWriter xmlWriter, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, object ownerOfExport)
        {
            System.Windows.Forms.Application.DoEvents();
            xmlWriter.WriteStartElement("Item");
            xmlWriter.WriteAttributeString("Name", this.name);
            xmlWriter.WriteAttributeString("Description", this.description);
            xmlWriter.WriteAttributeString("ItemType", this.itemType.ToString());
            //Attributes
            xmlWriter.WriteStartElement("Attributes");
            foreach (IAzManAttribute<IAzManItem> attribute in this.GetAttributes())
            {
                ((IAzManExport)attribute).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
            xmlWriter.WriteEndElement();
            //Biz Rule
            if (!String.IsNullOrEmpty(this.bizRuleSource))
            {
                xmlWriter.WriteStartElement("BizRule");
                xmlWriter.WriteAttributeString("BizRuleSourceLanguage", this.BizRuleSourceLanguage.Value.ToString());
                xmlWriter.WriteCData(this.bizRuleSource);
                xmlWriter.WriteEndElement();
            }
            //Children
            xmlWriter.WriteStartElement("Members");
            foreach (IAzManItem member in this.GetMembers())
            {
                xmlWriter.WriteStartElement("Member");
                xmlWriter.WriteAttributeString("Name", member.Name);
                xmlWriter.WriteAttributeString("MemberType", member.ItemType.ToString());
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            //Authorizations
            if (includeAuthorizations)
            {
                xmlWriter.WriteStartElement("Authorizations");
                foreach (IAzManAuthorization authorization in this.GetAuthorizations())
                {
                    System.Windows.Forms.Application.DoEvents();
                    ((IAzManExport)authorization).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Imports the specified XML reader.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeDBUsers">if set to <c>true</c> [include DB users].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <param name="mergeOptions">The merge options.</param>
        public void ImportChildren(XmlNode xmlNode, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, SqlAzManMergeOptions mergeOptions)
        {
            List<IAzManAuthorization> importedAuthorizations = new List<IAzManAuthorization>();
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "Attributes")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Attribute")
                        {
                            if (!this.Attributes.ContainsKey(childNode.Attributes["Key"].Value))
                            {
                                IAzManAttribute<IAzManItem> newItemAttribute = this.CreateAttribute(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
                            }
                        }
                    }
                }
                else if (node.Name == "Attribute")
                {
                    if (!this.Attributes.ContainsKey(node.Attributes["Key"].Value))
                    {
                        IAzManAttribute<IAzManItem> newItemAttribute = this.CreateAttribute(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                    }
                }
                System.Windows.Forms.Application.DoEvents();
                if (node.Name == "BizRule")
                {
                    string sLang = node.Attributes["BizRuleSourceLanguage"].Value;
                    NetSqlAzMan.BizRuleSourceLanguage lang = NetSqlAzMan.BizRuleSourceLanguage.CSharp;
                    if (String.Compare(sLang, NetSqlAzMan.BizRuleSourceLanguage.VBNet.ToString(), true)==0)
                    {
                        lang = NetSqlAzMan.BizRuleSourceLanguage.VBNet;
                    }
                    string source = node.InnerText;
                    this.ReloadBizRule(source, lang);
                }
                if (node.Name == "Member")
                {
                    IAzManItem member = this.application.GetItem(node.Attributes["Name"].Value);
                    if (!this.Members.ContainsKey(member.Name))
                        this.AddMember(member);
                }
                else if (node.Name == "Members")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Member")
                        {
                            IAzManItem member = this.application.GetItem(childNode.Attributes["Name"].Value);
                            if (!this.Members.ContainsKey(member.Name))
                                this.AddMember(member);
                        }
                    }
                }
                else if (includeAuthorizations && node.Name == "Authorizations")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Authorization")
                        {
                            WhereDefined sidWhereDefined;
                            switch (childNode.Attributes["SidWhereDefined"].Value)
                            {
                                case "Application": sidWhereDefined = WhereDefined.Application; break;
                                case "LDAP": sidWhereDefined = WhereDefined.LDAP; break;
                                case "Local": sidWhereDefined = WhereDefined.Local; break;
                                case "Store": sidWhereDefined = WhereDefined.Store; break;
                                case "Database": sidWhereDefined = WhereDefined.Database; break;
                                default:
                                    throw new System.Xml.Schema.XmlSchemaValidationException("WhereDefined attribute not valid.");
                            }
                            WhereDefined ownerSidWhereDefined;
                            switch (childNode.Attributes["OwnerSidWhereDefined"].Value)
                            {
                                case "Application": ownerSidWhereDefined = WhereDefined.Application; break;
                                case "LDAP": ownerSidWhereDefined = WhereDefined.LDAP; break;
                                case "Local": ownerSidWhereDefined = WhereDefined.Local; break;
                                case "Store": ownerSidWhereDefined = WhereDefined.Store; break;
                                case "Database": ownerSidWhereDefined = WhereDefined.Database; break;
                                default:
                                    throw new System.Xml.Schema.XmlSchemaValidationException("OwnerSidWhereDefined attribute not valid.");
                            }
                            AuthorizationType authorizationType;
                            switch (childNode.Attributes["AuthorizationType"].Value)
                            {
                                case "Allow": authorizationType = AuthorizationType.Allow; break;
                                case "AllowWithDelegation": authorizationType = AuthorizationType.AllowWithDelegation; break;
                                case "Deny": authorizationType = AuthorizationType.Deny; break;
                                case "Neutral": authorizationType = AuthorizationType.Neutral; break;
                                default:
                                    throw new System.Xml.Schema.XmlSchemaValidationException("AuthorizationType attribute not valid.");
                            }
                            DateTime? validFrom = null;
                            DateTime? validTo = null;
                            DateTime app;
                            if (DateTime.TryParse(childNode.Attributes["ValidFrom"].Value, out app))
                                validFrom = app;
                            if (DateTime.TryParse(childNode.Attributes["ValidTo"].Value, out app))
                                validTo = app;
                            if (includeWindowsUsersAndGroups
                                ||
                                !includeWindowsUsersAndGroups && sidWhereDefined != WhereDefined.LDAP
                                &&
                                sidWhereDefined != WhereDefined.Local
                                &&
                                this.application.Store.Storage.Mode != NetSqlAzManMode.Developer
                                ||
                                includeDBUsers && sidWhereDefined==WhereDefined.Database
                                ||
                                sidWhereDefined == WhereDefined.Store
                                ||
                                sidWhereDefined == WhereDefined.Application)
                            {
                                IAzManSid authSid = new SqlAzManSID(childNode.Attributes["Sid"].Value, sidWhereDefined == WhereDefined.Database);
                                IAzManSid ownerSid = new SqlAzManSID(childNode.Attributes["Owner"].Value, sidWhereDefined == WhereDefined.Database);
                                IAzManAuthorization authorization = null;
                                if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.OverwritesExistingItemAuthorization))
                                {
                                    authorization = this.CreateAuthorization(ownerSid, ownerSidWhereDefined, authSid, sidWhereDefined, authorizationType, validFrom, validTo);
                                    importedAuthorizations.Add(authorization);
                                }
                                else if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.CreatesNewItemAuthorizations))
                                {
                                    bool alreadyExists = false;
                                    foreach (var auth in this.GetAuthorizations(ownerSid, authSid))
                                    {
                                        if (auth.ValidFrom == validFrom && auth.ValidTo == validTo && auth.AuthorizationType == authorizationType)
                                        {
                                            alreadyExists = true;
                                            break;
                                        }
                                    }
                                    if (!alreadyExists)
                                    {
                                        authorization = this.CreateAuthorization(ownerSid, ownerSidWhereDefined, authSid, sidWhereDefined, authorizationType, validFrom, validTo);
                                        importedAuthorizations.Add(authorization);
                                    }
                                }
                                if (authorization!=null)
                                    authorization.ImportChildren(childNode, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                            }
                        }
                    }
                }
            }
            //Delete missing item Authorizations
            if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.DeleteMissingItemAuthorizations))
            {
                foreach (var auth in this.GetAuthorizations())
                {
                    bool exists = false;
                    foreach (var iauth in importedAuthorizations)
                    {
                        if (this.areEquals(auth, iauth))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        auth.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Detect if auth1 is the same of auth2
        /// </summary>
        /// <param name="auth1">The auth1.</param>
        /// <param name="auth2">The auth2.</param>
        /// <returns></returns>
        internal bool areEquals(IAzManAuthorization auth1, IAzManAuthorization auth2)
        {
            return
                auth1.AuthorizationType == auth2.AuthorizationType
                &&
                auth1.Item.Name == auth2.Item.Name
                &&
                auth1.Owner.StringValue == auth2.Owner.StringValue
                &&
                auth1.OwnerSidWhereDefined == auth2.OwnerSidWhereDefined
                &&
                auth1.SID.StringValue == auth2.SID.StringValue
                &&
                auth1.SidWhereDefined == auth2.SidWhereDefined
                &&
                auth1.ValidFrom == auth2.ValidFrom
                &&
                auth1.ValidTo == auth2.ValidTo;
        }


        #endregion
        #region Object Members
        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Item ID: {0}\r\nName: {1}\r\nDescription: {2}",
                this.itemId, this.name, this.description);
        }
        #endregion Object Members
        #region Static Methods
        /// <summary>
        /// Clears the biz rule assembly cache.
        /// </summary>
        public static void ClearBizRuleAssemblyCache()
        {
            lock (SqlAzManItem.bizRuleAssemblyCache)
            {
                SqlAzManItem.bizRuleAssemblyCache.Clear();
            }
        }
        #endregion  Static Methods
    }
}
