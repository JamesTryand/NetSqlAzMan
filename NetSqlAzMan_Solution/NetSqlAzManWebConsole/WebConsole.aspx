<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebConsole.aspx.cs" Inherits="NetSqlAzManWebConsole.WebConsole" Title="NetSqlAzMan Web Console" EnableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>NetSqlAzMan Web Console</title>
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <meta http-equiv="Content-Script-Type" content="text/javascript" />
    <script type="text/javascript" src="javascript/common.js" language="javascript"></script>
    <script type="text/javascript" language="javascript">
    <!-- 
        function doPostBack()
        {
            <asp:Literal ID="litPostBack" runat="server" />
        }
    -->
    </script>
    <asp:Literal ID="litSnow" runat="server" />
</head>
<body>
    <form id="Form1" runat="server">
    <asp:Panel ID="containerPanel" runat="server" Width="100%">
    <!-- header -->
    <asp:Panel ID="headerPanel" runat="server" Width="100%" Height="9.2em" class="headerPanel">
        <asp:Panel ID="headerPanelTop" Width="100%" runat="server">
            <asp:Panel ID="headerPanelLeft" runat="server">
                <asp:Image ID="imgLogo" ToolTip=".NET Sql Authorization Manager Web Console" AlternateText=".NET Sql Authorization Manager Web Console" runat="server" ImageUrl="~/images/NetSqlAzMan.gif" BorderWidth="2px" BorderColor="White" BorderStyle="Groove" style="padding: 0px 0px 0px 0px; margin: 2px 0px 0px 2px;"/>
            </asp:Panel>
            <asp:Panel ID="headerRightPanel" runat="server">
                <asp:Panel ID="headerRightPanelText" runat="server" HorizontalAlign="Right" style="color: Yellow;">
                <asp:Image ID="imgLogo2" runat="server" ImageUrl="~/images/logo.gif" AlternateText=".NET Sql Authorization Manager Web Console" ToolTip=".NET Sql Authorization Manager Web Console" />
                </asp:Panel>
                <asp:Panel ID="headerRightPanelToolBar" runat="server" style="float: left">
                &nbsp;
                <asp:ImageButton ID="imgReload" runat="server" OnClick="lnkReload_Click" ToolTip="Reload Storage" ImageUrl="~/images/reload.png" onmouseover="this.src='images/reload_over.png'" onmouseout="this.src='images/reload.png'" />
                <asp:ImageButton ID="imgLogout" runat="server" ToolTip="Logout" ImageUrl="~/images/logout.png" onmouseover="this.src='images/logout_over.png'" onmouseout="this.src='images/logout.png'" OnClick="lnkLogoff_Click" />
                </asp:Panel>
                <asp:Panel ID="themPanel" runat="server" HorizontalAlign="Right" style="float: right; padding: 0px 6px 6px 0px">
                    <asp:Button ID="DefaultTheme" runat="server" BackColor="#ccccff" ForeColor="Black" Text=" 0 " OnClick="DefaultTheme_Click" CssClass="themeButton" ToolTip="Theme 0" />
                    <asp:Button ID="GreenTheme" runat="server" BackColor="#00cc99" ForeColor="Black" Text=" 1 " OnClick="GreenTheme_Click" CssClass="themeButton" ToolTip="Theme 1"/>
                    <asp:Button ID="YellowTheme" runat="server" BackColor="#ff9933" ForeColor="Black" Text=" 2 " OnClick="YellowTheme_Click" CssClass="themeButton" ToolTip="Theme 2"/>
                </asp:Panel>

            </asp:Panel>
                <asp:Panel ID="toolBarPanel" runat="server" Height="20px" CssClass="toolBarPanelLabel">
                    &nbsp;&nbsp;User:&nbsp;<asp:Label ID="lblUser" runat="server" CssClass="toolBarPanelText"  />&nbsp;-&nbsp;Domain:&nbsp;&nbsp;<asp:Label ID="lblDomain" runat="server" CssClass="toolBarPanelText" />&nbsp;-&nbsp;Storage:&nbsp;&nbsp;<asp:Label ID="lblStorage" runat="server" CssClass="toolBarPanelText" />&nbsp;-&nbsp;NetSqlAzMan Mode:&nbsp;&nbsp;<asp:Label ID="lblMode" runat="server" CssClass="toolBarPanelText" />
                </asp:Panel>
                <asp:Panel ID="breadcrumbsPanel" runat="server" Height="20px" CssClass="breadcrumbsPanel">
                    <asp:Literal ID="breadcrumbsLiteral" runat="server"></asp:Literal>
                </asp:Panel>
            &nbsp;
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="mainPanel" runat="server" Width="100%" Height="100%" ScrollBars="none" style="height: 455px">
        <!-- treePanel -->
            <asp:Panel ID="treePanel" runat="server" ScrollBars="Both" Width="30%" Height="100%" style="float: left">
                <asp:TreeView ID="tv" runat="server" ShowLines="True" OnTreeNodeExpanded="tv_TreeNodeExpanded" OnSelectedNodeChanged="tv_SelectedNodeChanged" SkipLinkText="">
                    <Nodes>
                        <asp:TreeNode PopulateOnDemand="True" Text=".NET SQL Authorization Manager" Value=".NET SQL Authorization Manager" Expanded="False">
                        </asp:TreeNode>
                    </Nodes>
                    <SelectedNodeStyle Font-Bold="True" CssClass="selectedNode" />
                    <NodeStyle HorizontalPadding="2px" />
                    <HoverNodeStyle CssClass="hoverNode" Font-Bold="True" />
                </asp:TreeView>
            </asp:Panel>
        <!-- centerPanel -->
        <asp:Panel ID="centerPanel" runat="server" ScrollBars="Both" Width="45%" Height="100%" style="float: left;">
            <asp:Table ID="detailsTable" runat="server" CssClass="detailsTable">
            </asp:Table>
        </asp:Panel>
        <!-- rightPanel -->
        <asp:Panel ID="rightPanel" runat="server" ScrollBars="Both" GroupingText="Action Pane" Width="24.5%" Height="100%">
                <asp:Menu ID="mnuAction"  runat="server" CssClass="actionPane" OnMenuItemClick="mnuAction_MenuItemClick" StaticSubMenuIndent="" SkipLinkText="">
                <Items>
                    <asp:MenuItem Text="Menu" Value="Menu">
                        <asp:MenuItem Text="Menu 1" Value="Menu 1">
                            <asp:MenuItem Text="Menu 1.1" Value="Menu 1.1"></asp:MenuItem>
                        </asp:MenuItem>
                        <asp:MenuItem Text="Menu 2" Value="Menu 2"></asp:MenuItem>
                    </asp:MenuItem>
                </Items>
                    <LevelMenuItemStyles>
                        <asp:MenuItemStyle CssClass="actionPane" Font-Underline="False" />
                    </LevelMenuItemStyles>
                    <DynamicMenuStyle CssClass="actionPane" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" />
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="actionPane" Font-Underline="False" />
                    </LevelSubMenuStyles>
                    <DynamicMenuItemStyle CssClass="actionPane" />
                    <DynamicHoverStyle Font-Bold="True" ForeColor="red" />
                    <StaticHoverStyle Font-Bold="True" ForeColor="red" />
                    <StaticMenuStyle CssClass="menuItem" />
                    <StaticMenuItemStyle CssClass="menuItem" />
                </asp:Menu>
        </asp:Panel>
        <%-- Button used to do postback, postback done behalf of it --%>
        <asp:Button ID="btnDummyToPostBack" runat="server" Visible="false" />
    </asp:Panel>
    <!-- footer -->
    <asp:Panel ID="footerPanel" runat="server" BackImageUrl="~/images/bar_bg.gif" Width="100%" Height="1.5em" HorizontalAlign="Center" CssClass="footerPanel">
            Web Console version:&nbsp;<asp:Label ID="lblWebConsoleVersion" runat="server" Text="Label"></asp:Label>&nbsp;-&nbsp;NetSqlAzMan version:&nbsp;<asp:Label ID="lblNetSqlAzManVersion" runat="server" Text="Label"></asp:Label>&nbsp;-&nbsp;Storage version:&nbsp;<asp:Label ID="lblNetSqlAzManStorageVersion" runat="server" Text="Label"></asp:Label>&nbsp;-&nbsp;<a href="mailto:aferende@hotmail.com?subject=About NetSqlAzMan Web Console ..." title="">Andrea Ferendeles</a>&nbsp;-&nbsp;<a href="http://netsqlazman.codeplex.com" target="_blank" title="">NetSqlAzMan Home Site</a>&nbsp;<asp:Label ID="lblUpdateWarning" runat="server" Text="Label" CssClass="updateWarning" Visible="false"> - New Version Available -</asp:Label>
        </asp:Panel>
    </asp:Panel>
    </form>
    <script language="javascript" type="text/javascript">
        resizeMainPanel();
        window.onresize = resizeMainPanel;
        if (typeof(TreeView_HoverNode)!="function")
        {
            TreeView_HoverNode = new Function('p1,p2', '');
        }
    </script>
</body>
</html>
