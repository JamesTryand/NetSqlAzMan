using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;

namespace NetSqlAzMan.SnapIn.Printing
{
    /// <summary>
    /// Items Hierarchy Report
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public class ptItemsHierarchy : PrintDocumentBase
    {
        protected IAzManApplication[] applications;
        private List<object> alreadyPrinted;
        private frmItemsHierarchyView frm;
        private Pen linePen;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ptItemsHierarchy"/> class.
        /// </summary>
        public ptItemsHierarchy()
        {
            this.applications = new IAzManApplication[0];
            this.Title = Globalization.MultilanguageResource.GetString("frmItemsHierarchyView_lblInfo.Text");
            this.TopIcon = Properties.Resources.Hierarchy_32x32;
            this.alreadyPrinted = new List<object>();
            this.frm = new frmItemsHierarchyView();
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
                this.frm.applications = value;
            }
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            this.frm.buildApplicationsTreeView();
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
            base.WriteLineString("\t", Properties.Resources.Store_16x16, String.Format("{0}{1}", this.applications[0].Store.Name, (String.IsNullOrEmpty(this.applications[0].Store.Description) ? String.Empty : String.Format(" - {0}", this.applications[0].Store.Description))), e);
            parentStoreX = base.lastX + Properties.Resources.Store_16x16.Size.Width / 2;
            parentStoreY = base.lastY + Properties.Resources.Store_16x16.Size.Height + 3;
            if (base.EOP) return true;
            float parentApplicationX = 0.0F;
            float parentApplicationY = 0.0F;
            foreach (TreeNode tn in frm.itemsHierarchyTreeView.Nodes[0].Nodes[0].Nodes)
            {
                IAzManApplication app = (IAzManApplication)tn.Tag;
                if (!this.alreadyPrinted.Contains(tn)
                    //||
                    //this.alreadyPrinted.Contains(app) && tn.NextNode != null && !this.alreadyPrinted.Contains((IAzManApplication)tn.NextNode.Tag)
                    //||
                    //this.alreadyPrinted.Contains(app) && tn.NextNode == null && tn == frm.itemsHierarchyTreeView.Nodes[0].Nodes[0].LastNode
                    )
                {

                    base.WriteLineString("\t\t", Properties.Resources.Application_16x16, String.Format("{0}{1}", app.Name, (String.IsNullOrEmpty(app.Description) ? String.Empty : String.Format(" - {0}", app.Description))), e);
                    base.LineFrom(parentStoreX, parentStoreY);
                    base.LineTo(parentStoreX, base.lastY + Properties.Resources.Application_16x16.Height / 2, e, this.linePen);
                    base.LineFrom(parentStoreX, base.lastY + Properties.Resources.Application_16x16.Height / 2);
                    base.LineTo(base.lastX - 3, base.lastY + Properties.Resources.Application_16x16.Height / 2, e, this.linePen);
                    parentApplicationX = base.lastX + Properties.Resources.Application_16x16.Width / 2;
                    parentApplicationY = base.lastY + Properties.Resources.Application_16x16.Height + 3;
                    this.currentY += 3;
                    e.Graphics.DrawLine(base.pen, e.Graphics.MeasureString("\t\t", this.font).Width + Properties.Resources.Application_16x16.Size.Width + 3, this.currentY, this.right, this.currentY);
                    this.currentY += 3;
                    this.alreadyPrinted.Add(tn);
                    if (base.EOP) return true;
                }
                foreach (TreeNode tnChild in tn.Nodes)
                {
                    if (this.PrintItem(e, tnChild, 3, parentApplicationX, parentApplicationY))
                        return true;
                }
            }
            TreeNode lastNode = this.frm.itemsHierarchyTreeView.Nodes[0];
            while (lastNode.Nodes.Count > 0)
            {
                lastNode = lastNode.LastNode;
            }
            return !(this.alreadyPrinted.Contains(lastNode));
        }

        private bool PrintItem(PrintPageEventArgs e, TreeNode tn, int indentLevel, float parentItemX, float parentItemY)
        {
            IAzManItem item = (IAzManItem)tn.Tag;
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
            if (!this.alreadyPrinted.Contains(tn))
            {
                base.WriteLineString(new String('\t', indentLevel), itemIcon, String.Format("{0}{1}", item.Name, (String.IsNullOrEmpty(item.Description) ? String.Empty : String.Format(" - {0}", item.Description))), e);
                if (parentItemX == 0 || parentItemY == 0)
                {
                    parentItemX = e.Graphics.MeasureString(new String('\t', indentLevel - 1), base.font).Width + itemIcon.Size.Width / 2;
                    parentItemY = base.lastY - 1.5F;
                }
                base.LineFrom(parentItemX, parentItemY);
                base.LineTo(parentItemX, base.lastY + itemIcon.Height / 2, e, this.linePen);
                base.LineFrom(parentItemX, base.lastY + itemIcon.Height / 2);
                base.LineTo(base.lastX - 3, base.lastY + itemIcon.Height / 2, e, this.linePen);
                parentParentItemX = base.lastX + itemIcon.Width / 2;
                parentParentItemY = base.lastY + itemIcon.Height + 3;
                this.alreadyPrinted.Add(tn);
                if (base.EOP) return true;
            }
            foreach (TreeNode tnChild in tn.Nodes)
            {
                if (this.PrintItem(e, tnChild, indentLevel + 1, parentParentItemX, parentParentItemY))
                    return true;
            }
            return false;
        }
    }
}
