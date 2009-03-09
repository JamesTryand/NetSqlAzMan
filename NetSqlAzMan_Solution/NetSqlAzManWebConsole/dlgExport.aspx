<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgExport.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgExport" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
    <asp:Label ID="Label2" runat="server" Text=".NET Sql Authorization Manager Export"></asp:Label>&nbsp;</h2>
    <p>
        <asp:Panel ID="pnlExport" runat="server" Width="90%" style="visibility: visible">
        <asp:Panel ID="pnlExportOptions" runat="server" GroupingText="Export Options" Height="50px"
            Width="90%">
            <asp:CheckBox ID="chkAuthorizations" runat="server" Checked="True" Text="Include Item Authorizations" TabIndex="1" />&nbsp;<br />
            <asp:CheckBox
                ID="chkUsersAndGroups" runat="server" Text="Include Windows Users / Groups" TabIndex="2" />&nbsp;<br />
            <asp:CheckBox
                    ID="chkDBUsers" runat="server" Text="Include Database Users" TabIndex="3" /></asp:Panel>
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
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
