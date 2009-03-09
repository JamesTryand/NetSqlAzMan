<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgBizRule.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgBizRule" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server" GroupingText="Source Code:" Width="100%">
        <asp:TextBox ID="txtSourceCode" runat="server" BackColor="Blue" ForeColor="Yellow" style="background-image: none;"
            Height="260px" TextMode="MultiLine" Width="99%" Wrap="False" TabIndex="1"></asp:TextBox></asp:Panel>
    <br />
    <asp:Panel ID="Panel2" runat="server" GroupingText="Language:" Width="100%">
        <asp:RadioButton ID="rbCSharp" runat="server" GroupName="Language" Text="C#" TabIndex="2" />
        <asp:RadioButton ID="rbVBNet" runat="server" GroupName="Language" Text="VB.NET" TabIndex="3" /></asp:Panel>
    <br />
    <asp:Button ID="btnReloadBizRule" runat="server" Text="Reload rule into Store" OnClick="btnReloadBizRule_Click" TabIndex="4" />
    <asp:Button ID="btnClearBizRule" runat="server" OnClientClick="return confirm('Clear rule from Store ?');"
        Text="Clear rule from Store" OnClick="btnClearBizRule_Click" TabIndex="5" />
    <asp:Button ID="btnNewFromTemplate" runat="server" Text="New Biz Rule" OnClick="btnNewFromTemplate_Click" TabIndex="6" />
</asp:Content>
