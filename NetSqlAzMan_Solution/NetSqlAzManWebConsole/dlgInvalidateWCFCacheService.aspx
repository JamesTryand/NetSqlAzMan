<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgInvalidateWCFCacheService.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgInvalidateWCFCacheService" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        <asp:Label ID="Label2" runat="server" Text="Invalidate WCF Cache Service"></asp:Label>&nbsp;</h2>
    <p>
        &nbsp;<asp:Panel ID="pnlExport" runat="server" Width="90%" Style="visibility: visible">
            <asp:Label ID="Label3" runat="server" Text="WCF Cache Service EndPoint:"></asp:Label>
            &nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="modalDialogSingleLine" Width="450px">net.tcp://localhost:8000/NetSqlAzMan.Cache.Service/CacheService/</asp:TextBox>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </asp:Panel>
        &nbsp;</p>
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
