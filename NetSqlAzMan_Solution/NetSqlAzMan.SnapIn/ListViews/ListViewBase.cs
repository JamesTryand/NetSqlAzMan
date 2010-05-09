using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public abstract class ListViewBase : MMC.MmcListView
    {
        protected bool isALive;
        protected ListViewBase()
        {
            this.isALive = false;
        }

        protected override void OnShow()
        {
            base.OnShow();
            this.isALive = true;
        }

        protected override void OnShutdown(MMC.SyncStatus status)
        {
            this.isALive = false;
            base.OnShutdown(status);
        }

        protected override void OnInitialize(MMC.AsyncStatus status)
        {
            base.OnInitialize(status);
        }

        protected abstract void Refresh();

        private void internalShowDialog(string text, string caption, MessageBoxIcon icon)
        {
            try
            {
                MessageBoxParameters mbp = new MessageBoxParameters();
                mbp.Buttons = MessageBoxButtons.OK;
                mbp.Caption = caption;
                mbp.Text = text;
                mbp.Icon = icon;
                this.SnapIn.Console.ShowDialog(mbp);
            }
            catch
            { }
        }

        protected void ShowError(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Warning);
        }
    }
}
