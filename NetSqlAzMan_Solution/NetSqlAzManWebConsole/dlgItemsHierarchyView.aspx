<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgItemsHierarchyView.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgItemsHierarchyView" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="itemsHierachyPanel" runat="server" Width="99%" style="visibility: visible">
        <asp:LinkButton ID="lnkExpandAll" runat="server" OnClick="lnkExpandAll_Click" TabIndex="1">Expand All</asp:LinkButton>
        <asp:LinkButton ID="lnkCollapseAll" runat="server" OnClick="lnkCollapseAll_Click"
            TabIndex="2">Collapse All</asp:LinkButton><br />
        <br />
        <asp:TreeView ID="itemsHierarchyTreeView" runat="server" Width="100%" Height="100%" BorderStyle="Solid" ShowLines="True" BorderColor="black" BorderWidth="1px" TabIndex="3" SkipLinkText="" >
        </asp:TreeView>
    </asp:Panel>    
    <asp:Panel ID="pnlWait" runat="server" Width="99%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="hourglass" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
