using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmItemAuthorizations : frmBase
    {
        internal IAzManItem item = null;
        private DataTable dtAuthorizations;
        private bool modified;
        private string currentOwnerName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        private IAzManSid currentOwnerSid = new SqlAzManSID(System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);
        private WhereDefined currentOwnerSidWhereDefined;
        public frmItemAuthorizations()
        {
            InitializeComponent();
            string memberName;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(this.currentOwnerSid.StringValue, out memberName, out isLocal);
            this.currentOwnerSidWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
            this.dtAuthorizations = new DataTable();
            DataColumn dcAuthorizationId = new DataColumn("AuthorizationID", typeof(int));
            dcAuthorizationId.AutoIncrement = true;
            dcAuthorizationId.AutoIncrementSeed = -1;
            dcAuthorizationId.AutoIncrementStep = -1;
            dcAuthorizationId.AllowDBNull = false;
            dcAuthorizationId.Unique = true;
            DataColumn dcMemberTypeEnum = new DataColumn("MemberTypeEnum", typeof(MemberType));
            DataColumn dcMemberType = new DataColumn("MemberType", typeof(Bitmap));
            DataColumn dcOwner = new DataColumn("Owner", typeof(string));
            DataColumn dcOwnerSid = new DataColumn("OwnerSID", typeof(string));
            DataColumn dcName = new DataColumn("Name", typeof(string));
            DataColumn dcObjectSid = new DataColumn("ObjectSID", typeof(string));
            DataColumn dcWhereDefined = new DataColumn("WhereDefined", typeof(string));
            DataColumn dcWhereDefinedEnum = new DataColumn("WhereDefinedEnum", typeof(WhereDefined));
            DataColumn dcAuthorizationType = new DataColumn("AuthorizationType", typeof(Bitmap));
            DataColumn dcAuthorizationTypeEnum = new DataColumn("AuthorizationTypeEnum", typeof(AuthorizationType));
            DataColumn dcValidFrom = new DataColumn("ValidFrom", typeof(DateTime));
            dcValidFrom.AllowDBNull = true;
            DataColumn dcValidTo = new DataColumn("ValidTo", typeof(DateTime));
            dcValidTo.AllowDBNull = true;


            dcMemberType.Caption = Globalization.MultilanguageResource.GetString("DGColumn_10");
            dcOwner.Caption = Globalization.MultilanguageResource.GetString("DGColumn_20");
            dcOwnerSid.Caption = Globalization.MultilanguageResource.GetString("DGColumn_30");
            dcName.Caption = Globalization.MultilanguageResource.GetString("DGColumn_40");
            dcObjectSid.Caption = Globalization.MultilanguageResource.GetString("DGColumn_50");
            dcWhereDefined.Caption = Globalization.MultilanguageResource.GetString("DGColumn_60");
            dcAuthorizationType.Caption = Globalization.MultilanguageResource.GetString("DGColumn_70");
            dcValidFrom.Caption = Globalization.MultilanguageResource.GetString("DGColumn_80");
            dcValidTo.Caption = Globalization.MultilanguageResource.GetString("DGColumn_90");


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

                });
            this.modified = false;
        }


        private void frmAuthorizations_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.btnAddStoreGroups.Enabled = this.btnAddStoreGroups.Enabled = this.item.Application.Store.HasStoreGroups();
            this.btnAddApplicationGroups.Enabled = this.item.Application.HasApplicationGroups();
            //Prepare DataGridView
            this.dgAuthorizations.DataSource = this.dtAuthorizations;
            this.dgAuthorizations.Columns["MemberTypeEnum"].Visible = false;
            this.dgAuthorizations.Columns["AuthorizationID"].Visible = false;
            this.dgAuthorizations.Columns["OwnerSID"].Visible = false;
            this.dgAuthorizations.Columns["ObjectSID"].Visible = false;
            this.dgAuthorizations.Columns["WhereDefinedEnum"].Visible = false;
            this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Visible = false;
            foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
            {
                dgvc.Resizable = DataGridViewTriState.True;
                dgvc.ReadOnly = true;
                dgvc.HeaderText = this.dtAuthorizations.Columns[dgvc.Name].Caption;
            }
            this.dgAuthorizations.Columns["WhereDefined"].SortMode = DataGridViewColumnSortMode.Programmatic;
            this.dgAuthorizations.Columns["MemberType"].SortMode = DataGridViewColumnSortMode.Programmatic;
            this.dgAuthorizations.Columns["AuthorizationType"].SortMode = DataGridViewColumnSortMode.Programmatic;
            this.dgAuthorizations.Columns["ValidFrom"].ReadOnly = false;
            this.dgAuthorizations.Columns["ValidTo"].ReadOnly = false;
            this.RenderItemAuthorizations();
            this.Text += " - " + this.item.Name;
            this.allowWithDelegationToolStripMenuItem.Text = Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation");
            this.allowToolStripMenuItem.Text = Globalization.MultilanguageResource.GetString("Domain_Allow");
            this.denyToolStripMenuItem.Text = Globalization.MultilanguageResource.GetString("Domain_Deny");
            this.neutralToolStripMenuItem.Text = Globalization.MultilanguageResource.GetString("Domain_Neutral");
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private void AddAuthorizationDataRow(IAzManAuthorization authorization)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
            dr["AuthorizationID"] = authorization.AuthorizationId;
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
                dr["ObjectSID"] = new SqlAzManSID(authorization.SID.BinaryValue, true);
            }
            else
            {
                dr["ObjectSID"] = authorization.SID;
            }

            switch (authorization.SidWhereDefined.ToString())
            {
                case "LDAP": dr["WhereDefined"] = Globalization.MultilanguageResource.GetString("WhereDefined_LDAP"); break;
                case "Local": dr["WhereDefined"] = Globalization.MultilanguageResource.GetString("WhereDefined_Local"); break;
                case "Database": dr["WhereDefined"] = Globalization.MultilanguageResource.GetString("WhereDefined_DB"); break;
                case "Store": dr["WhereDefined"] = Globalization.MultilanguageResource.GetString("WhereDefined_Store"); break;
                case "Application": dr["WhereDefined"] = Globalization.MultilanguageResource.GetString("WhereDefined_Application"); break;
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

        private DataRow AddApplicationDataRow(IAzManApplicationGroup member)
        {
            DataRow dr = this.dtAuthorizations.NewRow();
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
            //Adjust columns Width
            foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
            {
                dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            }
            return dr;
        }

        private void RenderItemAuthorizations()
        {
            this.HourGlass(true);
            this.dtAuthorizations.Rows.Clear();
            IAzManAuthorization[] authorizations = this.item.GetAuthorizations();
            foreach (IAzManAuthorization authorization in authorizations)
            {
                AddAuthorizationDataRow(authorization);
            }
            this.dtAuthorizations.AcceptChanges();
            this.btnAttributes.Enabled = this.dtAuthorizations.Rows.Count > 0;
            this.modified = false;
            this.btnApply.Enabled = false;
            //Adjust columns Width
            foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
            {
                dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            }
            if (!this.item.Application.IAmManager)
            {
                this.contextMenuStrip1.Enabled =
                    this.btnAddStoreGroups.Enabled = this.btnAddApplicationGroups.Enabled = this.btnAddWindowsUsersAndGroups.Enabled = this.btnAddDBUsers.Enabled =
                    this.btnOK.Enabled = this.btnApply.Enabled = this.btnRemove.Enabled = false;
                this.dgAuthorizations.ReadOnly = true;
            }
            this.HourGlass(false);
        }

        protected void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
        }

        protected void ShowError(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void frmAuthorizations_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnAddApplicationGroups_Click(object sender, EventArgs e)
        {
            try
            {
                frmApplicationGroupsList frm = new frmApplicationGroupsList();
                frm.application = this.item.Application;
                frm.applicationGroup = null;
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    List<DataRow> rowsAdded = new List<DataRow>();
                    foreach (IAzManApplicationGroup ag in frm.selectedApplicationGroups)
                    {
                        rowsAdded.Add(this.AddApplicationDataRow(ag));
                        this.modified = true;
                    }
                    this.SelectDataGridViewRows(rowsAdded);
                }
                this.btnApply.Enabled = this.modified;
                //Adjust columns Width
                foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
                {
                    dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }
                this.HourGlass(false);
            }
            finally
            {
                this.btnApply.Enabled = this.modified;
            }
        }

        private void btnAddStoreGroups_Click(object sender, EventArgs e)
        {
            frmStoreGroupsList frm = new frmStoreGroupsList();
            frm.store = this.item.Application.Store;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                List<DataRow> rowsAdded = new List<DataRow>();
                foreach (IAzManStoreGroup sg in frm.selectedStoreGroups)
                {
                    rowsAdded.Add(this.AddStoreDataRow(sg));
                    this.modified = true;
                }
                this.SelectDataGridViewRows(rowsAdded);
            }
            this.btnApply.Enabled = this.modified;
            //Adjust columns Width
            foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
            {
                dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            }
            this.HourGlass(false);
        }

        private void btnAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this.Handle, this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Developer);
                /*Application.DoEvents();*/
                if (res != null)
                {
                    List<DataRow> rowsAdded = new List<DataRow>();
                    foreach (ADObject o in res)
                    {
                        WhereDefined wd = WhereDefined.LDAP;
                        if (!o.ADSPath.StartsWith("LDAP"))
                            wd = WhereDefined.Local;
                        string displayName;
                        bool isAGroup;
                        bool isLocal;
                        DirectoryServicesUtils.GetMemberInfo(o.Sid, out displayName, out isAGroup, out isLocal);
                        GenericMember gm = new GenericMember(new SqlAzManSID(o.Sid), wd, AuthorizationType.Neutral, null, null);
                        gm.Name = displayName;
                        rowsAdded.Add(this.AddLDapDataRow(gm, isAGroup));
                        this.modified = true;
                    }
                    this.SelectDataGridViewRows(rowsAdded);
                }
                this.btnApply.Enabled = this.modified;
                //Adjust columns Width
                foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
                {
                    dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg10"));
            }
            finally
            {
                this.btnApply.Enabled = this.modified;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            foreach (DataGridViewRow dgViewRow in this.dgAuthorizations.SelectedRows)
            {
                ((DataRowView)dgViewRow.DataBoundItem).Row.Delete();
                this.modified = true;
            }
            if (this.dtAuthorizations.Rows.Count == 0)
                this.btnRemove.Enabled = false;
            this.btnApply.Enabled = this.modified;
            this.HourGlass(false);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.CommitChanges();
                this.btnApply.Enabled = false;
                this.DialogResult = DialogResult.OK;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.CommitChanges();
                this.btnApply.Enabled = false;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
        }

        private void dgAuthorizations_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgAuthorizations.SelectedRows.Count == 0)
            {
                this.btnRemove.Enabled = this.btnAttributes.Enabled = false;
                this.dgAuthorizations.ContextMenu = null;
            }
            else if (this.dgAuthorizations.SelectedRows.Count == 1)
            {
                this.btnRemove.Enabled = this.btnAttributes.Enabled = true && this.item.Application.IAmManager;
                this.dgAuthorizations.ContextMenuStrip = this.contextMenuStrip1;
            }
            else
            {
                this.btnRemove.Enabled = true && this.item.Application.IAmManager;
                this.btnAttributes.Enabled = false;
                this.dgAuthorizations.ContextMenuStrip = this.contextMenuStrip1;
            }
        }

        private void dgAuthorizations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Authorization Type
            if (e.ColumnIndex!=-1 && this.dgAuthorizations.Columns[e.ColumnIndex].Name == "AuthorizationType" && e.RowIndex >= 0 && this.item.Application.IAmManager)
            {
                DataRow dr = ((DataRowView)this.dgAuthorizations.Rows[e.RowIndex].DataBoundItem).Row;
                AuthorizationType newAuthorizationType = this.nextAuthorizationType(dr);
                this.modified = true;
                this.btnApply.Enabled = true;
            }
            else if (e.RowIndex == -1) //Sort request
            {
                //AuthorizationType column
                if (this.dgAuthorizations.Columns[e.ColumnIndex].Name == "AuthorizationType")
                {
                    ListSortDirection sortDirection = ListSortDirection.Descending;
                    if (this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Tag == null)
                    {
                        this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Tag = sortDirection;
                    }
                    else
                    {
                        sortDirection = (ListSortDirection)this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Tag;
                    }
                    if (sortDirection == ListSortDirection.Ascending)
                        sortDirection = ListSortDirection.Descending;
                    else
                        sortDirection = ListSortDirection.Ascending;
                    this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Tag = sortDirection;
                    this.dgAuthorizations.Sort(this.dgAuthorizations.Columns["AuthorizationTypeEnum"], sortDirection);
                }
                //WhereDefined column
                else if (this.dgAuthorizations.Columns[e.ColumnIndex].Name == "WhereDefined")
                {
                    ListSortDirection sortDirection = ListSortDirection.Descending;
                    if (this.dgAuthorizations.Columns["WhereDefinedEnum"].Tag == null)
                    {
                        this.dgAuthorizations.Columns["WhereDefinedEnum"].Tag = sortDirection;
                    }
                    else
                    {
                        sortDirection = (ListSortDirection)this.dgAuthorizations.Columns["WhereDefinedEnum"].Tag;
                    }
                    if (sortDirection == ListSortDirection.Ascending)
                        sortDirection = ListSortDirection.Descending;
                    else
                        sortDirection = ListSortDirection.Ascending;
                    this.dgAuthorizations.Columns["WhereDefinedEnum"].Tag = sortDirection;
                    this.dgAuthorizations.Sort(this.dgAuthorizations.Columns["WhereDefinedEnum"], sortDirection);
                }
                //MemberType column
                else if (this.dgAuthorizations.Columns[e.ColumnIndex].Name == "MemberType")
                {
                    ListSortDirection sortDirection = ListSortDirection.Descending;
                    if (this.dgAuthorizations.Columns["MemberTypeEnum"].Tag == null)
                    {
                        this.dgAuthorizations.Columns["MemberTypeEnum"].Tag = sortDirection;
                    }
                    else
                    {
                        sortDirection = (ListSortDirection)this.dgAuthorizations.Columns["MemberTypeEnum"].Tag;
                    }
                    if (sortDirection == ListSortDirection.Ascending)
                        sortDirection = ListSortDirection.Descending;
                    else
                        sortDirection = ListSortDirection.Ascending;
                    this.dgAuthorizations.Columns["MemberTypeEnum"].Tag = sortDirection;
                    this.dgAuthorizations.Sort(this.dgAuthorizations.Columns["MemberTypeEnum"], sortDirection);
                }
                else
                {
                    this.dgAuthorizations.Columns["AuthorizationTypeEnum"].Tag = null;
                    this.dgAuthorizations.Columns["WhereDefinedEnum"].Tag = null;
                    this.dgAuthorizations.Columns["MemberTypeEnum"].Tag = null;
                }
            }
        }

        private AuthorizationType nextAuthorizationType(DataRow dr)
        {
            AuthorizationType authorizationType = (AuthorizationType)dr["AuthorizationTypeEnum"];
            AuthorizationType newAuthorizationType = authorizationType;
            switch (authorizationType)
            {
                case AuthorizationType.AllowWithDelegation:
                    newAuthorizationType = AuthorizationType.Allow;
                    break;
                case AuthorizationType.Allow:
                    newAuthorizationType = AuthorizationType.Deny;
                    break;
                case AuthorizationType.Deny:
                    newAuthorizationType = AuthorizationType.Neutral;
                    break;
                case AuthorizationType.Neutral:
                    newAuthorizationType = AuthorizationType.AllowWithDelegation;
                    break;
            }
            dr.BeginEdit();
            dr["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
            dr["AuthorizationTypeEnum"] = newAuthorizationType;
            dr.EndEdit();
            return newAuthorizationType;
        }

        private Bitmap RenderAuthorizationType(AuthorizationType authorizationType)
        {
            switch (authorizationType)
            {
                case AuthorizationType.AllowWithDelegation:
                    return Properties.Resources.AllowForDelegation;
                case AuthorizationType.Allow:
                    return Properties.Resources.Allow;
                case AuthorizationType.Deny:
                    return Properties.Resources.Deny;
                default:
                case AuthorizationType.Neutral:
                    return Properties.Resources.Neutral;
            }
        }

        private Bitmap RenderMemberType(MemberType memberType, IAzManSid sid)
        {
            switch (memberType)
            {
                case MemberType.StoreGroup:
                    if (this.item.Application.Store.GetStoreGroup(sid).GroupType==GroupType.Basic)
                        return Properties.Resources.StoreApplicationGroup_16x16.ToBitmap();
                    else
                        return Properties.Resources.WindowsQueryLDAPGroup_16x16.ToBitmap();
                case MemberType.ApplicationGroup:
                    if (this.item.Application.GetApplicationGroup(sid).GroupType == GroupType.Basic)
                        return Properties.Resources.StoreApplicationGroup_16x16.ToBitmap();
                    else
                        return Properties.Resources.WindowsQueryLDAPGroup_16x16.ToBitmap();
                case MemberType.WindowsNTUser:
                    return Properties.Resources.WindowsUser_16x16.ToBitmap();
                case MemberType.WindowsNTGroup:
                    return Properties.Resources.WindowsBasicGroup_16x16.ToBitmap();
                case MemberType.DatabaseUser:
                    return Properties.Resources.DBUser_16x16.ToBitmap();
                default:
                case MemberType.AnonymousSID:
                    return Properties.Resources.SIDNotFound_16x16.ToBitmap();
            }
        }

        private void dgAuthorizations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.item.Application.IAmManager)
                return;
            DataRow currentRow = ((DataRowView)this.dgAuthorizations.Rows[e.RowIndex].DataBoundItem).Row;
            if (currentRow["ValidFrom"] != DBNull.Value && currentRow["ValidTo"] != DBNull.Value)
            {
                DateTime validFrom = (DateTime)currentRow["ValidFrom"];
                DateTime validTo = (DateTime)currentRow["ValidTo"];
                if (validTo < validFrom)
                {
                    string error = Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg20");
                    currentRow.SetColumnError(this.dgAuthorizations.Columns[e.ColumnIndex].Name, error);
                }
                else
                {
                    currentRow.ClearErrors();
                }
            }
            else
            {
                currentRow.ClearErrors();
            }
            this.dgAuthorizations.Columns[e.ColumnIndex].Width = this.dgAuthorizations.Columns[e.ColumnIndex].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            this.modified = true;
            this.btnApply.Enabled = true;
        }

        private void CommitChanges()
        {
            if (this.dtAuthorizations.HasErrors)
                throw new Exception(Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg30"));
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
                            new SqlAzManSID((string)dr["OwnerSID"], this.currentOwnerSidWhereDefined==WhereDefined.Database),
                            this.currentOwnerSidWhereDefined,
                            (((WhereDefined)dr["WhereDefinedEnum"])==WhereDefined.Database ? 
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
                
                this.modified = false;
                this.dtAuthorizations.AcceptChanges();
                this.item.Application.Store.Storage.CommitTransaction();
            }
            catch
            {
                this.item.Application.Store.Storage.RollBackTransaction();
                throw;
            }
            finally
            {
                this.btnApply.Enabled = this.modified;
            }
        }

        private void allowWithDelegationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drv in this.dgAuthorizations.SelectedRows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                AuthorizationType newAuthorizationType = AuthorizationType.AllowWithDelegation;
                dr.BeginEdit();
                dr["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
                dr["AuthorizationTypeEnum"] = newAuthorizationType;
                dr.EndEdit();
                this.modified = true;
                this.btnApply.Enabled = true;
            }
        }

        private void allowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drv in this.dgAuthorizations.SelectedRows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                AuthorizationType newAuthorizationType = AuthorizationType.Allow;
                dr.BeginEdit();
                dr["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
                dr["AuthorizationTypeEnum"] = newAuthorizationType;
                dr.EndEdit();
                this.modified = true;
                this.btnApply.Enabled = true;
            }
        }

        private void denyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drv in this.dgAuthorizations.SelectedRows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                AuthorizationType newAuthorizationType = AuthorizationType.Deny;
                dr.BeginEdit();
                dr["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
                dr["AuthorizationTypeEnum"] = newAuthorizationType;
                dr.EndEdit();
                this.modified = true;
                this.btnApply.Enabled = true;
            }
        }

        private void neutralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drv in this.dgAuthorizations.SelectedRows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                AuthorizationType newAuthorizationType = AuthorizationType.Neutral;
                dr.BeginEdit();
                dr["AuthorizationType"] = this.RenderAuthorizationType(newAuthorizationType);
                dr["AuthorizationTypeEnum"] = newAuthorizationType;
                dr.EndEdit();
                this.modified = true;
                this.btnApply.Enabled = true;
            }
        }

        private void SelectDataGridViewRows(List<DataRow> rows)
        {
            this.dgAuthorizations.SuspendLayout();
            foreach (DataGridViewRow dgvr in this.dgAuthorizations.Rows)
            {
                DataRow gridRow = ((DataRowView)dgvr.DataBoundItem).Row;
                dgvr.Selected = false;
                foreach (DataRow dr in rows)
                {
                    if (gridRow == dr)
                    {
                        dgvr.Selected = true;
                        rows.Remove(dr);
                        break;
                    }
                }
            }
            this.dgAuthorizations.ResumeLayout();
        }

        private void btnAttributes_Click(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;
            if (this.modified)
            {
                dr = MessageBox.Show(Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg40"), Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Tit40"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            if (dr == DialogResult.Yes)
            {
                if (this.modified)
                {
                    try
                    {
                        this.HourGlass(true);
                        this.CommitChanges();
                        this.btnApply.Enabled = false;
                        this.HourGlass(false);
                    }
                    catch (Exception ex)
                    {
                        this.HourGlass(false);
                        this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
                        return;
                    }
                }
                try
                {
                    DataRow dataRow = ((DataRowView)this.dgAuthorizations.SelectedRows[0].DataBoundItem).Row;
                    frmAuthorizationAttributes frm = new frmAuthorizationAttributes();
                    frm.Text += " - " + this.item.Name;
                    frm.authorization = this.item.GetAuthorization((int)dataRow["AuthorizationID"]);
                    frm.ShowDialog(this);
                    /*Application.DoEvents();*/
                    frm.Dispose();
                    /*Application.DoEvents();*/
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg50"));
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dgAuthorizations_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //DateTime syntax check
            string columnName = this.dgAuthorizations.Columns[e.ColumnIndex].Name;
            this.ShowError(String.Format(Globalization.MultilanguageResource.GetString("frmItemAuthorizations_Msg60")+" {0}",columnName),columnName);
            e.ThrowException = false;
        }

        private void btnAddDBUsers_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                frmDBUsersList frm = new frmDBUsersList();
                frm.application = this.item.Application;
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    List<DataRow> rowsAdded = new List<DataRow>();
                    foreach (IAzManDBUser dbUser in frm.selectedDBUsers)
                    {
                        rowsAdded.Add(this.AddDBUserDataRow(dbUser));
                        this.modified = true;
                    }
                    this.SelectDataGridViewRows(rowsAdded);
                }
                this.btnApply.Enabled = this.modified;
                //Adjust columns Width
                foreach (DataGridViewColumn dgvc in this.dgAuthorizations.Columns)
                {
                    dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }

            }
            finally
            {
                this.HourGlass(false);
            }
        }
    }
}
