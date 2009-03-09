<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgStoreProperties.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgStoreProperties" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblName" runat="server" CssClass="modalDialogLabel" Text="Name:"></asp:Label><br />
    <asp:TextBox ID="txtName" runat="server" CssClass="modalDialogSingleLine" MaxLength="255" TabIndex="1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
        ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
    <asp:Label ID="lblDescription" runat="server" CssClass="modalDialogLabel" Text="Description:"></asp:Label><br />
    <asp:TextBox ID="txtDescription" runat="server" CssClass="modalDialogMultiLine" MaxLength="1024" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
    
    <div>
        <asp:Button ID="btnAttributes" runat="server" Text="Attributes" TabIndex="3" OnClientClick="javascript:openDialog('dlgStoreAttributes.aspx', 1);"/>
        <asp:Button ID="btnPermissions" runat="server" Text="Permissions" TabIndex="4" OnClientClick="javascript:openDialog('dlgStorePermissions.aspx', 1);" />
    </div>
</asp:Content>
