using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Printing
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public class ptItemAuthorizations : PrintDocumentBase
    {
        protected IAzManApplication[] applications;
        private List<object> alreadyPrinted;
        private Pen linePen;

        public ptItemAuthorizations()
        {
            this.applications = new IAzManApplication[0];
            this.Title = Globalization.MultilanguageResource.GetString("Folder_Msg20");
            this.TopIcon = Properties.Resources.ItemAuthorization_32x32;
            this.alreadyPrinted = new List<object>();

            this.linePen = new Pen(Brushes.Black, 1F);
            this.linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }
        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        /// <value>The applications.</value>
        public IAzManApplication[] Applications
        {
            get
            {
                return this.applications;
            }
            set
            {
                this.applications = value;
            }
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
            this.alreadyPrinted.Clear();
        }

        /// <summary>
        /// Prints the body.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        protected override bool PrintPageBody(PrintPageEventArgs e)
        {
            float parentStoreX;
            float parentStoreY;
            if (this.applications == null || this.applications.Length == 0)
            {
                return false;
            }
            base.SkipLines(e, 1);
            if (!this.alreadyPrinted.Contains(this.applications[0].Store))
            {
                this.alreadyPrinted.Add(this.applications[0].Store);
                base.WriteLineString("\t", Properties.Resources.Store_16x16, String.Format("{0}{1}", this.applications[0].Store.Name, (String.IsNullOrEmpty(this.applications[0].Store.Description) ? String.Empty : String.Format(" - {0}", this.applications[0].Store.Description))), e);
            }
            parentStoreX = base.lastX + Properties.Resources.Store_16x16.Size.Width / 2;
            parentStoreY = base.lastY + Properties.Resources.Store_16x16.Size.Height + 3;
            if (base.EOP) return true;
            float parentApplicationX = 0.0F;
            float parentApplicationY = 0.0F;
            foreach (IAzManApplication app in this.applications)
            {
                if (!this.alreadyPrinted.Contains(app))
                {
                    base.WriteLineString("\t\t", Properties.Resources.Application_16x16, String.Format("{0}{1}", app.Name, (String.IsNullOrEmpty(app.Description) ? String.Empty : String.Format(" - {0}", app.Description))), e);
                    parentApplicationX = base.lastX + Properties.Resources.Application_16x16.Width / 2;
                    parentApplicationY = base.lastY + Properties.Resources.Application_16x16.Height + 3;
                    this.currentY += 3;
                    e.Graphics.DrawLine(base.pen, e.Graphics.MeasureString("\t\t", this.font).Width + Properties.Resources.Application_16x16.Size.Width + 3, this.currentY, this.right, this.currentY);
                    this.currentY += 3;
                    this.alreadyPrinted.Add(app);
                    if (base.EOP) return true;
                }
                //Roles

                foreach (IAzManItem role in app.Items.Values)
                {
                    if (role.ItemType == ItemType.Role)
                    {
                        if (this.PrintItem(e, role, 3, parentApplicationX, parentApplicationY))
                            return true;
                    }
                }
                //Tasks
                foreach (IAzManItem task in app.Items.Values)
                {
                    if (task.ItemType == ItemType.Task)
                    {
                        if (this.PrintItem(e, task, 3, parentApplicationX, parentApplicationY))
                            return true;
                    }
                }
                //Operations
                foreach (IAzManItem operation in app.Items.Values)
                {
                    if (operation.ItemType == ItemType.Operation)
                    {
                        if (this.PrintItem(e, operation, 3, parentApplicationX, parentApplicationY))
                            return true;
                    }
                }
            }
            return false;
        }

        private bool PrintItem(PrintPageEventArgs e, IAzManItem item, int indentLevel, float parentItemX, float parentItemY)
        {
            Icon itemIcon = null;
            switch (item.ItemType)
            {
                case ItemType.Role:
                    itemIcon = Properties.Resources.Role_16x16;
                    break;
                case ItemType.Task:
                    itemIcon = Properties.Resources.Task_16x16;
                    break;
                case ItemType.Operation:
                    itemIcon = Properties.Resources.Operation_16x16;
                    break;
            }
            float parentParentItemX = 0.0F;
            float parentParentItemY = 0.0F;
            if (!this.alreadyPrinted.Contains(item.ItemId))
            {
                base.WriteLineString(new String('\t', indentLevel), itemIcon, String.Format("{0}{1}", item.Name, (String.IsNullOrEmpty(item.Description) ? String.Empty : String.Format(" - {0}", item.Description))), e);
                if (parentItemX == 0 || parentItemY == 0)
                {
                    parentItemX = e.Graphics.MeasureString(new String('\t', indentLevel - 1), base.font).Width + itemIcon.Size.Width / 2;
                    parentItemY = base.lastY - 1.5F;
                }
                parentParentItemX = base.lastX + itemIcon.Width / 2;
                parentParentItemY = base.lastY + itemIcon.Height + 3;
                this.alreadyPrinted.Add(item.ItemId);
                if (base.EOP) return true;
            }
            AuthorizationType authType = AuthorizationType.AllowWithDelegation;
            while (true)
            {
                IAzManAuthorization[] authz = new IAzManAuthorization[item.Authorizations.Count];
                string sAuthz = String.Empty;
                Image imageType = null;
                item.Authorizations.CopyTo(authz, 0); ;
                if (authz.Length > 0)
                {
                    if (!this.alreadyPrinted.Contains(item.ItemId.ToString() + authType.ToString()))
                    {
                        string sAuthType = String.Empty;
                        switch (authType)
                        {
                            case AuthorizationType.AllowWithDelegation: sAuthType = Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation"); imageType = Properties.Resources.AllowForDelegation; break;
                            case AuthorizationType.Allow: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Allow"); imageType = Properties.Resources.Allow; break;
                            case AuthorizationType.Deny: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Deny"); imageType = Properties.Resources.Deny; break;
                            case AuthorizationType.Neutral: sAuthType = Globalization.MultilanguageResource.GetString("Domain_Neutral"); imageType = Properties.Resources.Neutral; break;
                        }
                        foreach (IAzManAuthorization auth in authz)
                        {
                            if (auth.AuthorizationType == authType)
                            {
                                string displayName = String.Empty;
                                MemberType mt = auth.GetMemberInfo(out displayName);
                                sAuthz += displayName + ", ";
                            }
                        }
                        if (sAuthz.EndsWith(", ")) sAuthz = sAuthz.Remove(sAuthz.Length - 2);
                        if (!String.IsNullOrEmpty(sAuthz))
                        {
                            base.currentX = e.Graphics.MeasureString(new string('\t', indentLevel+1), base.font).Width;
                            RectangleF rect = new RectangleF(this.currentX, this.currentY, e.PageBounds.Width - this.currentX - e.PageBounds.Left, e.PageBounds.Height - e.PageBounds.Top);
                            StringFormat sf = new StringFormat();
                            sf.FormatFlags = StringFormatFlags.NoClip;
                            sf.Trimming = StringTrimming.Word;
                            if (this.currentY + this.meauseMultiLines(sAuthz, this.font, rect, sf, e) + this.spaceBetweenLines > e.PageBounds.Bottom - 70)
                            {
                                //all authz in the next page
                                return true;
                            }
                            base.WriteLineString(new string('\t', indentLevel + 1), imageType, sAuthType.ToUpper(), e);
                            base.currentX = e.Graphics.MeasureString(new string('\t', indentLevel + 1), base.font).Width;
                            base.WriteLineString(sAuthz, e);
                            this.alreadyPrinted.Add(item.ItemId.ToString() + authType.ToString());
                            if (base.EOP) return true;
                            base.WriteLineString(" ", e);
                            if (base.EOP) return true;
                        }
                    }
                }
                bool stop = false;
                switch (authType)
                {
                    case AuthorizationType.AllowWithDelegation: authType = AuthorizationType.Allow; break;
                    case AuthorizationType.Allow: authType = AuthorizationType.Deny; break;
                    case AuthorizationType.Deny: authType = AuthorizationType.Neutral; break;
                    case AuthorizationType.Neutral: stop = true; break;
                }
                if (stop) break;
            }
            return false;
        }
    }
}
