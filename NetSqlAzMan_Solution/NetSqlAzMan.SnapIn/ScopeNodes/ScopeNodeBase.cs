using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public abstract class ScopeNodeBase : MMC.ScopeNode
    {
        protected internal event ScopeNodeChangedHandler ScopeNodeChanged;
        protected ScopeNodeBase()
        {
            
        }

        protected ScopeNodeBase(bool hideExpandIcon)
            : base(hideExpandIcon)
        {
            
        }

        protected abstract void Render();

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            try
            {
                status.ReportProgress(50, 100, "");
                this.NotifyChanged();
                /*Application.DoEvents();*/
            }
            catch
            {
                //ignore ... because viewDescription List can be shutting down.
            }
        }

        protected virtual void NotifyChanged()
        {
            try
            {
                this.SnapIn.RegisterCurrentThreadForUI();
                if (this.ScopeNodeChanged != null)
                {
                    this.ScopeNodeChanged();
                }
            }
            catch
            { }
            finally
            {
                this.SnapIn.UnregisterCurrentThreadForUI();
            }

        }

        protected void internalShowDialog(string text, string caption, MessageBoxIcon icon)
        {
            try
            {
                this.SnapIn.RegisterCurrentThreadForUI();
                MessageBoxParameters mbp = new MessageBoxParameters();
                mbp.Buttons = MessageBoxButtons.OK;
                mbp.Caption = caption;
                mbp.Text = text;
                mbp.Icon = icon;
                this.SnapIn.Console.ShowDialog(mbp);
            }
            catch
            {
                //
            }
            finally
            {
                this.SnapIn.UnregisterCurrentThreadForUI();
            }
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
