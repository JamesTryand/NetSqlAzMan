using System;
using System.Collections.Generic;

namespace NetSqlAzManWebConsole
{
    public struct MenuInfo
    {
        public string Text;
        public string ToolTip;
        public bool Enabled;
        public string NavigateUrl;
        public List<MenuInfo> SubMenus;

        public MenuInfo(string text)
        {
            this.Text = text;
            this.ToolTip = String.Empty;
            this.Enabled = true;
            this.NavigateUrl = String.Empty;
            this.SubMenus = new List<MenuInfo>();
        }
        public MenuInfo(string text, string toolTip, params MenuInfo[] subMenus) : this(text)
        {
            this.ToolTip = toolTip;
            this.SubMenus.AddRange(subMenus);
        }

        public MenuInfo(string text, string toolTip, bool enabled, params MenuInfo[] subMenus) : this(text, toolTip, subMenus)
        {
            this.Enabled = enabled;
        }

        public MenuInfo(string text, string toolTip, string navigateUrl, params MenuInfo[] subMenus)
            : this(text, toolTip, true, subMenus)
        {
            this.NavigateUrl = navigateUrl;
        }

        public MenuInfo(string text, string toolTip, bool enabled, string navigateUrl, params MenuInfo[] subMenus)
            : this(text, toolTip, enabled, subMenus)
        {
            this.NavigateUrl = navigateUrl;
        }

    }
}
