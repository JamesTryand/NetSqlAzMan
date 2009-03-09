using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan;
using NetSqlAzMan.SnapIn.ScopeNodes;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class AuthorizationsListView : ListViewBase
    {
        public AuthorizationsListView()
        { 
        
        }

        protected override void OnInitialize(MMC.AsyncStatus status)
        {
            base.OnInitialize(status);
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            /*Application.DoEvents();*/
            base.OnInitialize(status);
            this.Columns.Clear();
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_Name");
            this.Columns[0].SetWidth(250);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_AuthorizationType"),150),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_WhereDefined"),100),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Owner"),250),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ValidFrom"), 180),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ValidTo"), 180),
                    new MMC.MmcListViewColumn("Sid",300)});
            this.Mode = MMC.MmcListViewMode.Report;
            this.Refresh();
            ItemAuthorizationScopeNode itSN = this.ScopeNode as ItemAuthorizationScopeNode;
            if (itSN != null)
                itSN.ScopeNodeChanged += new ScopeNodeChangedHandler(AuthorizationsListView_ScopeNodeChanged);
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }

        void AuthorizationsListView_ScopeNodeChanged()
        {
            if (this.isALive)
            {
                this.Refresh();
                this.SelectScopeNode(this.ScopeNode);
            }
        }

        protected override void OnSelectionChanged(MMC.SyncStatus status)
        {
            base.OnSelectionChanged(status);
            //Multiple delete
            if (this.SelectedNodes.Count >= 1)
            {
                //Prepare actions
                this.SelectionData.ActionsPaneItems.Clear();
                this.SelectionData.Update(this.SelectedNodes, this.SelectedNodes.Count > 1, null, null);
                
                //Authorizations - MMC.SyncAction
                MMC.ActionGroup authorizationsActionGroup = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("Menu_Msg30"), Globalization.MultilanguageResource.GetString("Menu_Tit30"));
                this.SelectionData.ActionsPaneItems.Add(authorizationsActionGroup);
                //Allow with delegation
                MMC.SyncAction allowWithDelegationAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation"), Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegationDescription"));
                if (this.ScopeNode as ItemAuthorizationScopeNode!=null && !((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.IAmManager ||
                    this.ScopeNode as ItemDefinitionScopeNode!=null && !((ItemDefinitionScopeNode)this.ScopeNode).Item.Application.IAmManager)
                    allowWithDelegationAction.Enabled = false;
                allowWithDelegationAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(allowWithDelegationAction_Triggered);
                authorizationsActionGroup.Items.Add(allowWithDelegationAction);
                //Allow
                MMC.SyncAction allowAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Domain_Allow"), Globalization.MultilanguageResource.GetString("Domain_AllowDescription"));
                if (this.ScopeNode as ItemAuthorizationScopeNode != null && !((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.IAmManager ||
                    this.ScopeNode as ItemDefinitionScopeNode != null && !((ItemDefinitionScopeNode)this.ScopeNode).Item.Application.IAmManager)
                    allowAction.Enabled = false;
                allowAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(allowAction_Triggered);
                authorizationsActionGroup.Items.Add(allowAction);
                //Deny
                MMC.SyncAction denyAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Domain_Deny"), Globalization.MultilanguageResource.GetString("Domain_DenyDescription"));
                if (this.ScopeNode as ItemAuthorizationScopeNode != null && !((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.IAmManager ||
                    this.ScopeNode as ItemDefinitionScopeNode != null && !((ItemDefinitionScopeNode)this.ScopeNode).Item.Application.IAmManager)
                    denyAction.Enabled = false;
                denyAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(denyAction_Triggered);
                authorizationsActionGroup.Items.Add(denyAction);
                //Neutral
                MMC.SyncAction neutralAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Domain_Neutral"), Globalization.MultilanguageResource.GetString("Domain_NeutralDescription"));
                if (this.ScopeNode as ItemAuthorizationScopeNode != null && !((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.IAmManager ||
                    this.ScopeNode as ItemDefinitionScopeNode != null && !((ItemDefinitionScopeNode)this.ScopeNode).Item.Application.IAmManager)
                    neutralAction.Enabled = false;
                neutralAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(neutralAction_Triggered);
                authorizationsActionGroup.Items.Add(neutralAction);
                //Line separator
                MMC.ActionSeparator lineAction = new MMC.ActionSeparator();
                this.SelectionData.ActionsPaneItems.Add(lineAction);
                //MMC.SyncAction - Delete Authorizations
                MMC.SyncAction deleteAuthorizationsAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg40"), Globalization.MultilanguageResource.GetString("Menu_Tit40"));
                if (this.ScopeNode as ItemAuthorizationScopeNode != null && !((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.IAmManager ||
                    this.ScopeNode as ItemDefinitionScopeNode != null && !((ItemDefinitionScopeNode)this.ScopeNode).Item.Application.IAmManager)
                    deleteAuthorizationsAction.Enabled = false;
                deleteAuthorizationsAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(deleteAuthorizationsAction_Triggered);
                this.SelectionData.ActionsPaneItems.Add(deleteAuthorizationsAction);
            }
        }

        void permissionAction_Triggered(object sender, MMC.SyncActionEventArgs e, AuthorizationType authorizationType)
        {
            try
            {
                foreach (MMC.ResultNode resultNode in this.SelectedNodes)
                {
                    IAzManAuthorization auth = (IAzManAuthorization)resultNode.Tag;
                    this.changePermission(auth, authorizationType);
                    string sAuthType;
                    switch (authorizationType)
                    {
                        default:
                        case AuthorizationType.Neutral: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Neutral"); break;
                        case AuthorizationType.Allow: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Allow"); break;
                        case AuthorizationType.AllowWithDelegation: sAuthType = Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation"); break;
                        case AuthorizationType.Deny: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Deny"); break;
                    }
                    resultNode.SubItemDisplayNames[0] = sAuthType;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("AuthorizationsListView_Tit10"));
            }
            finally
            {
                /*Application.DoEvents();*/
            }

        }

        private void changePermission(IAzManAuthorization authorization, AuthorizationType authorizationType)
        {
            authorization.Update(authorization.Owner, authorization.SID, authorization.SidWhereDefined, authorizationType, authorization.ValidFrom, authorization.ValidTo);
        }

        void neutralAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            this.permissionAction_Triggered(sender, e, AuthorizationType.Neutral);
        }

        void denyAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            this.permissionAction_Triggered(sender, e, AuthorizationType.Deny);
        }

        void allowAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            this.permissionAction_Triggered(sender, e, AuthorizationType.Allow);
        }

        void allowWithDelegationAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            this.permissionAction_Triggered(sender, e, AuthorizationType.AllowWithDelegation);
        }

        void deleteAuthorizationsAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = e.Action.DisplayName;
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg50"));
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    foreach (MMC.ResultNode resultNode in this.SelectedNodes)
                    {
                        IAzManAuthorization auth = (IAzManAuthorization)resultNode.Tag;
                        auth.Delete();
                        this.ResultNodes.Remove(resultNode);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("AuthorizationsListView_Tit20"));
                }
            }
        }

        protected override void Refresh()
        {
            this.ResultNodes.Clear();
            IAzManAuthorization[] authorizations = ((ItemAuthorizationScopeNode)this.ScopeNode).Item.GetAuthorizations();
            List<MMC.ResultNode> list = new List<MMC.ResultNode>();
            foreach (IAzManAuthorization authorization in authorizations)
            {
                MMC.ResultNode resultNode = new MMC.ResultNode();
                resultNode.Tag = authorization;
                string sAuthType;
                switch (authorization.AuthorizationType)
                {
                    default:
                    case AuthorizationType.Neutral: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Neutral"); break;
                    case AuthorizationType.Allow: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Allow"); break;
                    case AuthorizationType.AllowWithDelegation: sAuthType = Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation"); break;
                    case AuthorizationType.Deny: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Deny"); break;
                }
                string displayName;
                MemberType memberType = authorization.GetMemberInfo(out displayName);
                string ownerName;
                MemberType ownerType = authorization.GetOwnerInfo(out ownerName);
                resultNode.DisplayName = displayName;
                switch (memberType)
                {
                    case MemberType.AnonymousSID: resultNode.ImageIndex = ImageIndexes.SidNotFoundImgIdx; break;
                    case MemberType.ApplicationGroup:
                        if (((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.GetApplicationGroup(authorization.SID).GroupType == GroupType.Basic)
                        {
                            resultNode.ImageIndex = ImageIndexes.ApplicationGroupBasicImgIdx;
                        }
                        else
                        {
                            resultNode.ImageIndex = ImageIndexes.ApplicationGroupLDAPImgIdx;
                        }
                        break;
                    case MemberType.StoreGroup:
                        if (((ItemAuthorizationScopeNode)this.ScopeNode).Item.Application.Store.GetStoreGroup(authorization.SID).GroupType == GroupType.Basic)
                        {
                            resultNode.ImageIndex = ImageIndexes.StoreGroupBasicImgIdx;
                        }
                        else
                        {
                            resultNode.ImageIndex = ImageIndexes.StoreGroupLDAPImgIdx;
                        }
                        break;
                    case MemberType.WindowsNTGroup: resultNode.ImageIndex = ImageIndexes.WindowsGroupImgIdx; break;
                    case MemberType.WindowsNTUser: resultNode.ImageIndex = ImageIndexes.WindowsUserImgIdx; break;
                    case MemberType.DatabaseUser: resultNode.ImageIndex = ImageIndexes.DatabaseUserImgIdx; break;
                }
                string sidWDS = String.Empty;
                switch (authorization.SidWhereDefined.ToString())
                {
                    case "LDAP": sidWDS = Globalization.MultilanguageResource.GetString("WhereDefined_LDAP"); break;
                    case "Local": sidWDS = Globalization.MultilanguageResource.GetString("WhereDefined_Local"); break;
                    case "Database": sidWDS = Globalization.MultilanguageResource.GetString("WhereDefined_DB"); break;
                    case "Store": sidWDS = Globalization.MultilanguageResource.GetString("WhereDefined_Store"); break;
                    case "Application": sidWDS = Globalization.MultilanguageResource.GetString("WhereDefined_Application"); break;
                }
                resultNode.SubItemDisplayNames.AddRange(
                    new string[] {
                        sAuthType,
                        sidWDS,
                        ownerName,
                        (authorization.ValidFrom.HasValue ? authorization.ValidFrom.Value.ToString() : String.Empty),
                        (authorization.ValidTo.HasValue ? authorization.ValidTo.Value.ToString() : String.Empty),
                        authorization.SID.StringValue});
                list.Add(resultNode);
            }
            this.ResultNodes.AddRange(list.ToArray());
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            base.OnRefresh(status);
            //Children
            this.Refresh();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }
    }
}
