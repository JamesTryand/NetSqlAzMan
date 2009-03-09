<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="rptItemsAuthorizations.aspx.cs" Inherits="NetSqlAzManWebConsole.rptItemsAuthorizations" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="itemsHierachyPanel" runat="server" Width="99%" style="visibility: visible">
    <h2>
        Items Authorizations</h2>
    <hr />
        <asp:TreeView ID="itemsHierarchyTreeView" runat="server" Width="100%" Height="100%" ShowLines="True" TabIndex="3" >
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
