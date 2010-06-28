using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgItemAuthorizations : dlgPage
    {
        internal IAzManItem item = null;
        private DataTable dtAuthorizations;
        private bool modified;
        private string currentOwnerName;
        private IAzManSid currentOwnerSid;
        private WhereDefined currentOwnerSidWhereDefined;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Item Authorizations")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("ItemAuthorization_32x32.gif");
            this.showOkCancelApply();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.setApplyHandler(new EventHandler(this.btnApply_Click));
        }

        private void saveSessionVariables()
        {
            this.Session["modified"] = this.modified;
            this.Session["item"] = this.item;
            this.Session["dtAuthorizations"] = this.dtAuthorizations;
            this.Session["store"] = this.item.Application.Store;
            this.Session["application"] = this.item.Application;
            this.Session["ADObjectType"] = ADObjectType.UsersAndGroups;
            this.Session["currentOwnerName"] = this.currentOwnerName;
            this.Session["currentOwnerSid"] = this.currentOwnerSid;
            this.Session["currentOwnerSidWhereDefined"] = this.currentOwnerSidWhereDefined;
            this.Session["storeGroup"] = null;
            this.Session["applicationGroup"] = null;
        }

        private void loadSessionVariables()
        {
            if (this.Session["modified"] != null)
                this.modified = (bool)this.Session["modified"];
            else
                this.modified = false;
            this.item = this.Session["selectedObject"] as IAzManItem;
            this.dtAuthorizations = this.Session["dtAuthorizations"] as DataTable;
            this.currentOwnerName = (string)this.Session["currentOwnerName"];
            this.currentOwnerSid = this.Session["currentOwnerSid"] as IAzManSid;
            this.currentOwnerSidWhereDefined = (WhereDefined)this.Session["currentOwnerSidWhereDefined"];
        }

        private void bindGridView()
        {
            this.dgAuthorizations.DataSource = this.dtAuthorizations;
            this.dgAuthorizations.DataBind();
            this.EmptyGridFix(this.dgAuthorizations);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.item = this.Session["selectedObject"] as IAzManItem;
            this.menuItem = Request["MenuItem"];
            this.Text = "Item Authorizations";
            this.Description = this.Text;
            this.Title = this.Text;
            this.currentOwnerName = this.Request.LogonUserIdentity.Name;
            this.currentOwnerSid = new SqlAzManSID(this.Request.LogonUserIdentity.User.Value);
            //this.showWaitPanelOnSubmit(this.pnlWait, this.pnlXXX);
            if (!this.Page.IsPostBack)
            {
                string memberName;
                bool isLocal;
                DirectoryServicesWebUtils.GetMemberInfo(this.currentOwnerSid.StringValue, out memberName, out isLocal);
                this.currentOwnerSidWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
                this.saveSessionVariables();
                this.loadSessionVariables();
                this.dtAuthorizations = new DataTable();
                DataColumn dcAuthorizationId = new DataColumn("AuthorizationID", typeof(int));
                dcAuthorizationId.AutoIncrement = true;
                dcAuthorizationId.AutoIncrementSeed = -1;
                dcAuthorizationId.AutoIncrementStep = -1;
                dcAuthorizationId.AllowDBNull = false;
                dcAuthorizationId.Unique = true;
                DataColumn dcAttributesLink = new DataColumn("AttributesLink", typeof(string));
                DataColumn dcMemberTypeEnum = new DataColumn("MemberTypeEnum", typeof(MemberType));
                DataColumn dcMemberType = new DataColumn("MemberType", typeof(string));
                DataColumn dcOwner = new DataColumn("Owner", typeof(string));
                DataColumn dcOwnerSid = new DataColumn("OwnerSID", typeof(string));
                DataColumn dcName = new DataColumn("Name", typeof(string));
                DataColumn dcObjectSid = new DataColumn("ObjectSID", typeof(string));
                DataColumn dcWhereDefined = new DataColumn("WhereDefined", typeof(string));
                DataColumn dcWhereDefinedEnum = new DataColumn("WhereDefinedEnum", typeof(WhereDefined));
                DataColumn dcAuthorizationType = new DataColumn("AuthorizationType", typeof(string));
                DataColumn dcAuthorizationTypeEnum = new DataColumn("AuthorizationTypeEnum", typeof(AuthorizationType));
                DataColumn dcValidFrom = new DataColumn("ValidFrom", typeof(DateTime));
                dcValidFrom.AllowDBNull = true;
                DataColumn dcValidTo = new DataColumn("ValidTo", typeof(DateTime));
                dcValidTo.AllowDBNull = true;

                dcMemberType.Caption = "Member Type";
                dcOwner.Caption = "Owner";
                dcOwnerSid.Caption = "Owner SID";
                dcName.Caption = "Name";
                dcObjectSid.Caption = "Object SID";
                dcWhereDefined.Caption = "Where Defined";
                dcAuthorizationType.Caption = "Authorization Type";
                dcValidFrom.Caption = "Valid From";
                dcValidTo.Caption = "Valid To";

                this.dtAuthorizations.Columns.AddRange(
                    new DataColumn[] 
                {
                    dcAuthorizationId,
                    dcMemberType,
                    dcName, 
                    dcAuthorizationType,
                    dcWhereDefined,
                    dcOwner,
                    dcOwnerSid,
                    dcValidFrom,
                    dcValidTo,
                    dcObjectSid, 
                    dcAuthorizationTypeEnum,
                    dcWhereDefinedEnum,
                    dcMemberTypeEnum,
                    dcAttributesLink
                });
                foreach (DataColumn dc in this.dtAuthorizations.Columns)
                {
                    dc.AllowDBNull = true;
                }
                dcMemberType.AllowDBNull = false;
                dcAuthorizationType.AllowDBNull = false;
                this.modified = false;

                this.btnAddStoreGroups.Enabled = this.item.Application.Store.HasStoreGroups();
                this.btnAddApplicationGroups.Enabled = this.item.Application.HasApplicationGroups();
                //Prepare DataGridView
                this.dgAuthorizations.DataSource = this.dtAuthorizations;
                this.dgAuthorizations.DataBind();
                this.RenderItemAuthorizations();
                this.Text += " - " + this.item.Name;
                this.saveSessionVariables();
                this.bindGridView();
            }
            else
            {
                this.loadSessionVariables();
                if (this.Session["selectedStoreGroups"] != null)
                {
                    this.btnAddStoreGroups_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedApplicationGroups"] != null)
                {
                    this.btnAddApplicationGroups_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedDBUsers"] != null)
                {
                    this.btnAddDBUsers_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedADObjects"] != null)
                {
                    this.btnAddWindowsUsersAndGroups_Click(this, EventArgs.Empty);
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.CommitChanges();
                Response.Redirect("dlgItemAuthorizations.aspx");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.CommitChanges();
                this.Session["FindNodeText"] = this.item.Name;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void AddAuthorizationDataRow(IAzManAuthorization authorization)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AuthorizationID"] = authorization.AuthorizationId;
            dr["AttributesLink"] = this.getAttributesLink((int)dr["AuthorizationID"]);
            string displayName;
            MemberType memberType = authorization.GetMemberInfo(out displayName);
            string ownerName;
            MemberType ownerType = authorization.GetOwnerInfo(out ownerName);
            dr["MemberType"] = this.RenderMemberType(memberType, authorization.SID);
            dr["MemberTypeEnum"] = memberType;
            dr["Owner"] = ownerName;
            dr["Name"] = displayName;
            dr["OwnerSID"] = authorization.Owner;
            if (authorization.SidWhereDefined == WhereDefined.Database)
            {
                dr["ObjectSID"] = authorization.SID.StringValue;
            }
            else
            {
                dr["ObjectSID"] = authorization.SID.StringValue;
            }

            switch (authorization.SidWhereDefined.ToString())
            {
                case "LDAP": dr["WhereDefined"] = "Active Directory"; break;
                case "Local": dr["WhereDefined"] = "Local"; break;
                case "Database": dr["WhereDefined"] = "DB User"; break;
                case "Store": dr["WhereDefined"] = "Store"; break;
                case "Application": dr["WhereDefined"] = "Application"; break;
            }

            dr["WhereDefinedEnum"] = authorization.SidWhereDefined;
            dr["AuthorizationType"] = this.RenderAuthorizationType(authorization.AuthorizationType);
            dr["AuthorizationTypeEnum"] = authorization.AuthorizationType;
            dr["ValidFrom"] = authorization.ValidFrom.HasValue ? (object)authorization.ValidFrom.Value : DBNull.Value;
            dr["ValidTo"] = authorization.ValidTo.HasValue ? (object)authorization.ValidTo.Value : DBNull.Value;
            this.dtAuthorizations.Rows.Add(dr);
        }

        private DataRow AddDBUserDataRow(IAzManDBUser member)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AttributesLink"] = this.getApplyBeforeAlert();
            dr["Owner"] = this.currentOwnerName;
            dr["OwnerSID"] = this.currentOwnerSid;
            dr["Name"] = member.UserName;
            dr["MemberType"] = this.RenderMemberType(MemberType.DatabaseUser, member.CustomSid);
            dr["MemberTypeEnum"] = MemberType.DatabaseUser;
            dr["ObjectSID"] = member.CustomSid.StringValue;
            dr["WhereDefined"] = WhereDefined.Database.ToString();
            dr["WhereDefinedEnum"] = WhereDefined.Database;
            dr["AuthorizationType"] = this.RenderAuthorizationType(AuthorizationType.Neutral);
            dr["AuthorizationTypeEnum"] = AuthorizationType.Neutral;
            dr["ValidFrom"] = DBNull.Value;
            dr["ValidTo"] = DBNull.Value;
            this.dtAuthorizations.Rows.Add(dr);
            return dr;
        }

        private DataRow AddStoreDataRow(IAzManStoreGroup member)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AttributesLink"] = this.getApplyBeforeAlert();
            dr["Owner"] = this.currentOwnerName;
            dr["OwnerSID"] = this.currentOwnerSid;
            dr["Name"] = member.Name;
            dr["MemberType"] = this.RenderMemberType(MemberType.StoreGroup, member.SID);
            dr["MemberTypeEnum"] = MemberType.StoreGroup;
            dr["ObjectSID"] = member.SID;
            dr["WhereDefined"] = WhereDefined.Store.ToString();
            dr["WhereDefinedEnum"] = WhereDefined.Store;
            dr["AuthorizationType"] = this.RenderAuthorizationType(AuthorizationType.Neutral);
            dr["AuthorizationTypeEnum"] = AuthorizationType.Neutral;
            dr["ValidFrom"] = DBNull.Value;
            dr["ValidTo"] = DBNull.Value;
            this.dtAuthorizations.Rows.Add(dr);
            return dr;
        }

        private string getAttributesLink(int authorizationId)
        {
            return String.Format("javascript:openDialog('dlgAuthorizationAttributes.aspx?AuthorizationID={0}', 1)", authorizationId);
        }

        private string getApplyBeforeAlert()
        {
            return "javascript:window.alert('Changes must be applied before manage Authorization Attributes.')";
        }

        private DataRow AddApplicationDataRow(IAzManApplicationGroup member)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AttributesLink"] = this.getApplyBeforeAlert();
            dr["Owner"] = this.currentOwnerName;
            dr["OwnerSID"] = this.currentOwnerSid;
            dr["Name"] = member.Name;
            dr["MemberType"] = this.RenderMemberType(MemberType.ApplicationGroup, member.SID);
            dr["MemberTypeEnum"] = MemberType.ApplicationGroup;
            dr["ObjectSID"] = member.SID;
            dr["WhereDefined"] = WhereDefined.Application.ToString();
            dr["WhereDefinedEnum"] = WhereDefined.Application;
            dr["AuthorizationType"] = this.RenderAuthorizationType(AuthorizationType.Neutral);
            dr["AuthorizationTypeEnum"] = AuthorizationType.Neutral;
            dr["ValidFrom"] = DBNull.Value;
            dr["ValidTo"] = DBNull.Value;
            this.dtAuthorizations.Rows.Add(dr);
            return dr;
        }

        private DataRow AddLDapDataRow(GenericMember member, bool isAGroup)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AttributesLink"] = this.getApplyBeforeAlert();
            dr["Owner"] = this.currentOwnerName;
            dr["OwnerSID"] = this.currentOwnerSid;
            dr["Name"] = member.Name;
            MemberType memberType = isAGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
            dr["MemberType"] = this.RenderMemberType(memberType, member.sid);
            dr["MemberTypeEnum"] = memberType;
            dr["ObjectSID"] = member.sid;
            dr["WhereDefined"] = member.WhereDefined.ToString();
            dr["WhereDefinedEnum"] = member.WhereDefined;
            dr["AuthorizationType"] = this.RenderAuthorizationType(member.authorizationType);
            dr["AuthorizationTypeEnum"] = member.authorizationType;
            dr["ValidFrom"] = member.validFrom.HasValue ? (object)member.validFrom.Value : DBNull.Value;
            dr["ValidTo"] = member.validTo.HasValue ? (object)member.validTo.Value : DBNull.Value;
            this.dtAuthorizations.Rows.Add(dr);
            return dr;
        }

        private void RenderItemAuthorizations()
        {
            this.dtAuthorizations.Rows.Clear();
            IAzManAuthorization[] authorizations = this.item.GetAuthorizations();
            foreach (IAzManAuthorization authorization in authorizations)
            {
                AddAuthorizationDataRow(authorization);
            }
            this.dtAuthorizations.AcceptChanges();
            this.modified = false;
            if (!this.item.Application.IAmManager)
            {
                    this.btnAddStoreGroups.Enabled = this.btnAddApplicationGroups.Enabled = this.btnAddWindowsUsersAndGroups.Enabled = this.btnAddDBUsers.Enabled =
                    this.btnOk.Enabled = this.btnApply.Enabled = this.btnRemove.Enabled = false;
            }
            this.bindGridView();
        }

        private void btnAddApplicationGroups_Click(object sender, EventArgs e)
        {
            IAzManApplicationGroup[] selectedApplicationGroups = (IAzManApplicationGroup[])this.Session["selectedApplicationGroups"];
            this.Session["selectedApplicationGroups"] = null;
            List<DataRow> rowsAdded = new List<DataRow>();
            foreach (IAzManApplicationGroup ag in selectedApplicationGroups)
            {
                rowsAdded.Add(this.AddApplicationDataRow(ag));
                this.modified = true;
            }
            this.saveSessionVariables();
            this.bindGridView();
        }

        private void btnAddStoreGroups_Click(object sender, EventArgs e)
        {
            IAzManStoreGroup[] selectedStoreGroups = (IAzManStoreGroup[])this.Session["selectedStoreGroups"];
            this.Session["selectedStoreGroups"] = null;
            List<DataRow> rowsAdded = new List<DataRow>();
            foreach (IAzManStoreGroup sg in selectedStoreGroups)
            {
                rowsAdded.Add(this.AddStoreDataRow(sg));
                this.modified = true;
            }
            this.saveSessionVariables();
            this.bindGridView();
        }

        private void btnAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            try
            {
                ADObject[] res = ((List<ADObject>)this.Session["selectedADObjects"]).ToArray();
                this.Session["selectedADObjects"] = null;
                if (res != null)
                {
                    List<DataRow> rowsAdded = new List<DataRow>();
                    foreach (ADObject o in res)
                    {
                        WhereDefined wd = WhereDefined.LDAP;
                        if (!o.ADSPath.StartsWith("LDAP"))
                            wd = WhereDefined.Local;
                        string displayName = String.Empty;
                        bool isAGroup = false;
                        bool isLocal = false;
                        GenericMember gm = null;
                        DirectoryServicesWebUtils.GetMemberInfo(o.Sid, out displayName, out isAGroup, out isLocal);
                        gm = new GenericMember(new SqlAzManSID(o.Sid), wd, AuthorizationType.Neutral, null, null);
                        gm.Name = displayName;
                        rowsAdded.Add(this.AddLDapDataRow(gm, isAGroup));
                        this.modified = true;
                    }
                }
                this.saveSessionVariables();
                this.bindGridView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            for (int i=0;i<this.dgAuthorizations.Rows.Count;i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgAuthorizations.Rows[i].FindControl("chkSelect")).Checked)
                {
                    int authId = int.Parse(this.dgAuthorizations.Rows[i].Cells[12].Text);
                    this.dtAuthorizations.Select("AuthorizationID=" + authId.ToString())[0].Delete();
                    this.modified = true;
                }
            }
            this.bindGridView();
            this.saveSessionVariables();
        }

        private string RenderAuthorizationType(AuthorizationType authorizationType)
        {
            switch (authorizationType)
            {
                case AuthorizationType.AllowWithDelegation:
                    return this.getImageUrl("AllowForDelegation.bmp");
                case AuthorizationType.Allow:
                    return this.getImageUrl("Allow.bmp");
                case AuthorizationType.Deny:
                    return this.getImageUrl("Deny.bmp");
                default:
                case AuthorizationType.Neutral:
                    return this.getImageUrl("Neutral.bmp");
            }
        }

        private string RenderMemberType(MemberType memberType, IAzManSid sid)
        {
            switch (memberType)
            {
                case MemberType.StoreGroup:
                    if (this.item.Application.Store.GetStoreGroup(sid).GroupType == GroupType.Basic)
                        return this.getImageUrl("StoreApplicationGroup_16x16.gif");
                    else
                        return this.getImageUrl("WindowsQueryLDAPGroup_16x16.gif");
                case MemberType.ApplicationGroup:
                    if (this.item.Application.GetApplicationGroup(sid).GroupType == GroupType.Basic)
                        return this.getImageUrl("StoreApplicationGroup_16x16.gif");
                    else
                        return this.getImageUrl("WindowsQueryLDAPGroup_16x16.gif");
                case MemberType.WindowsNTUser:
                    return this.getImageUrl("WindowsUser_16x16.gif");
                case MemberType.WindowsNTGroup:
                    return this.getImageUrl("WindowsBasicGroup_16x16.gif");
                case MemberType.DatabaseUser:
                    return this.getImageUrl("DBUser_16x16.gif");
                default:
                case MemberType.AnonymousSID:
                    return this.getImageUrl("SIDNotFound_16x16.gif");
            }
        }

        private void CommitChanges()
        {
            try
            {
                if (!this.modified)
                    return;
                //Application Group Properties
                this.item.Application.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                // To Delete
                DataTable toDelete = this.dtAuthorizations.GetChanges(DataRowState.Deleted);
                if (toDelete != null)
                {
                    toDelete.RejectChanges();
                    foreach (DataRow dr in toDelete.Rows)
                    {
                        this.item.GetAuthorization((int)dr["AuthorizationID"]).Delete();
                    }
                }
                // To Add
                DataTable toAdd = this.dtAuthorizations.GetChanges(DataRowState.Added);
                if (toAdd != null)
                {
                    foreach (DataRow dr in toAdd.Rows)
                    {
                        IAzManAuthorization authorization = this.item.CreateAuthorization(
                            new SqlAzManSID((string)dr["OwnerSID"], this.currentOwnerSidWhereDefined == WhereDefined.Database),
                            this.currentOwnerSidWhereDefined,
                            (((WhereDefined)dr["WhereDefinedEnum"]) == WhereDefined.Database ?
                            new SqlAzManSID((string)dr["ObjectSID"], true) : new SqlAzManSID((string)dr["ObjectSID"], false)),
                            (WhereDefined)dr["WhereDefinedEnum"],
                            (AuthorizationType)dr["AuthorizationTypeEnum"],
                            (dr["ValidFrom"] != DBNull.Value ? (DateTime?)dr["ValidFrom"] : null),
                            (dr["ValidTo"] != DBNull.Value ? (DateTime?)dr["ValidTo"] : null));
                        DataRow originalRow = this.dtAuthorizations.Select("AuthorizationID=" + dr["AuthorizationID"].ToString())[0];
                        originalRow["AuthorizationID"] = authorization.AuthorizationId;
                    }
                }
                // To Update
                DataTable toUpdate = this.dtAuthorizations.GetChanges(DataRowState.Modified);
                if (toUpdate != null)
                {
                    foreach (DataRow dr in toUpdate.Rows)
                    {
                        this.item.GetAuthorization((int)dr["AuthorizationID"]).Update(
                            new SqlAzManSID((string)dr["OwnerSID"], this.currentOwnerSidWhereDefined == WhereDefined.Database),
                            new SqlAzManSID((string)dr["ObjectSID"], ((WhereDefined)dr["WhereDefinedEnum"]) == WhereDefined.Database),
                            (WhereDefined)dr["WhereDefinedEnum"],
                            (AuthorizationType)dr["AuthorizationTypeEnum"],
                            (dr["ValidFrom"] != DBNull.Value ? (DateTime?)dr["ValidFrom"] : null),
                            (dr["ValidTo"] != DBNull.Value ? (DateTime?)dr["ValidTo"] : null));
                    }
                }
                this.item.Application.Store.Storage.CommitTransaction();
                this.modified = false;
                this.dtAuthorizations.AcceptChanges();
            }
            catch
            {
                this.item.Application.Store.Storage.RollBackTransaction();
                throw;
            }
        }

        private void btnAddDBUsers_Click(object sender, EventArgs e)
        {
            IAzManDBUser[] selectedDBUsers = (IAzManDBUser[])this.Session["selectedDBUsers"];
            this.Session["selectedDBUsers"] = null;
            List<DataRow> rowsAdded = new List<DataRow>();
            foreach (IAzManDBUser dbu in selectedDBUsers)
            {
                rowsAdded.Add(this.AddDBUserDataRow(dbu));
                this.modified = true;
            }
            this.saveSessionVariables();
            this.bindGridView();
        }

        protected void dgAuthorizations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.dgAuthorizations.EditIndex = e.NewEditIndex;
            this.bindGridView();
        }

        protected void dgAuthorizations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.dgAuthorizations.EditIndex = -1;
            this.bindGridView();
        }

        protected void dgAuthorizations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int authorizationId = int.Parse(this.dgAuthorizations.Rows[e.RowIndex].Cells[12].Text);
                DataRow drAuthorization = this.dtAuthorizations.Select("AuthorizationID=" + authorizationId.ToString())[0];
                int authorizationTypeSelectedIndex = ((DropDownList)this.dgAuthorizations.Rows[e.RowIndex].Cells[6].FindControl("ddlAuthorizationType")).SelectedIndex;
                AuthorizationType newAuthorizationType;
                switch (authorizationTypeSelectedIndex)
                {
                    case 0: newAuthorizationType = AuthorizationType.AllowWithDelegation; break;
                    case 1: newAuthorizationType = AuthorizationType.Allow; break;
                    case 2: newAuthorizationType = AuthorizationType.Deny; break;
                    default:
                    case 3: newAuthorizationType = AuthorizationType.Neutral; break;
                }
                TextBox txtValidFrom = (TextBox)this.dgAuthorizations.Rows[e.RowIndex].Cells[8].FindControl("txtValidFrom");
                DateTime? newValidFrom = String.IsNullOrEmpty(txtValidFrom.Text.Trim()) ? (DateTime?)null : DateTime.Parse(txtValidFrom.Text);
                TextBox txtValidTo = (TextBox)this.dgAuthorizations.Rows[e.RowIndex].Cells[9].FindControl("txtValidTo");
                DateTime? newValidTo = String.IsNullOrEmpty(txtValidTo.Text.Trim()) ? (DateTime?)null : DateTime.Parse(txtValidTo.Text);
                drAuthorization.BeginEdit();
                drAuthorization["AuthorizationTypeEnum"] = newAuthorizationType;
                drAuthorization["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
                drAuthorization["ValidFrom"] = newValidFrom.HasValue ? (object)newValidFrom.Value : DBNull.Value;
                drAuthorization["ValidTo"] = newValidTo.HasValue ? (object)newValidTo.Value : DBNull.Value;
                drAuthorization.EndEdit();
                this.modified = true;
                this.dgAuthorizations.EditIndex = -1;
                this.saveSessionVariables();
                this.bindGridView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected int getSelectedIndex(int authorizationId)
        {
            //int authorizationId = int.Parse(this.dgAuthorizations.Rows[this.dgAuthorizations.EditIndex].Cells[12].Text);
            AuthorizationType at = (AuthorizationType)this.dtAuthorizations.Select("AuthorizationId=" + authorizationId.ToString())[0]["AuthorizationTypeEnum"];
            switch (at)
            {
                case AuthorizationType.AllowWithDelegation: return 0;
                case AuthorizationType.Allow: return 1;
                case AuthorizationType.Deny: return 2;
                case AuthorizationType.Neutral: return 3;
                default:
                    return -1;
            }
        }

        protected string getAuthorizationTypeToolTip(string authorizationTypeImageUrl)
        {
            if (authorizationTypeImageUrl.EndsWith("AllowForDelegation.bmp")) return "Allow With Delegation";
            else if (authorizationTypeImageUrl.EndsWith("Allow.bmp")) return "Allow";
            else if (authorizationTypeImageUrl.EndsWith("Deny.bmp")) return "Deny";
            else if (authorizationTypeImageUrl.EndsWith("Neutral.bmp")) return "Neutral";
            else return String.Empty;

        }

        protected string getMemberTypeToolTip(string memberTypeImageUrl)
        {
            if (memberTypeImageUrl.EndsWith("StoreApplicationGroup_16x16.gif")) return "Basic Group";
            else if (memberTypeImageUrl.EndsWith("WindowsQueryLDAPGroup_16x16.gif")) return "LDAP Group";
            else if (memberTypeImageUrl.EndsWith("WindowsUser_16x16.gif")) return "Windows User";
            else if (memberTypeImageUrl.EndsWith("WindowsBasicGroup_16x16.gif")) return "Windows Group";
            else if (memberTypeImageUrl.EndsWith("DBUser_16x16.gif")) return "Database User";
            else return "SID not found";

        }

        protected string convertDateTime(object dateTime)
        {
            return ((DateTime)dateTime).ToString("G");
        }

        protected string getAttributesLink(string attributeId)
        {
            return String.Format("javascript:openDialog('dlgAuthorizationAttributes.aspx?AuthorizationID={0}', 1)", attributeId);
        }
    }
}
