<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="ADObjectPickerShowDialog.aspx.cs" Inherits="NetSqlAzManWebConsole.ADObjectPickerShowDialog" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        <br />
        <asp:Label ID="lblTopDescription" runat="server" Text="Choose Users and/or Groups:"></asp:Label>
        </h2>
        <asp:TextBox ID="txtInput" runat="server" CssClass="modalDialogMultiLine" TextMode="MultiLine" TabIndex="1"></asp:TextBox><br />
    <em><span style="font-size: 0.8em; color: blue">e.g.: "user@domain.ext; user2; john
        doe"</span></em><br />
        <br />
        <asp:Button ID="btnCheckNames" runat="server" OnClick="btnCheckNames_Click" Text="Check Names" TabIndex="2" OnClientClick="javascript:doHourglass();" />
    <asp:Button ID="btnBrowse" runat="server" Text="Browse" OnClientClick="javascript:doHourglass();openDialog('dlgActiveDirectorySearch.aspx', 3);" />
</asp:Content>
