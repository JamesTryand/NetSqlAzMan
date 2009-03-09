using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.SnapIn.ListViews;
using NetSqlAzMan.SnapIn.Forms;

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
