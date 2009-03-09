<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgNewStoreGroup.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgNewStoreGroup" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblName" runat="server" CssClass="modalDialogLabel" Text="Name:"></asp:Label><br />
    <asp:TextBox ID="txtName" runat="server" CssClass="modalDialogSingleLine" MaxLength="255" TabIndex="1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
        ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
    <asp:Label ID="lblDescription" runat="server" CssClass="modalDialogLabel" Text="Description:"></asp:Label><br />
    <asp:TextBox ID="txtDescription" runat="server" MaxLength="1024" TextMode="MultiLine" TabIndex="2" Height="210px" Width="90%"></asp:TextBox>
    <br />
    <br />
    
    <div>
        Group Type:
        <asp:RadioButton ID="rbtBasic" runat="server" Checked="True" GroupName="GroupType"
            Text="Basic" ToolTip="Basic Group" TabIndex="3" />
        <asp:RadioButton ID="rbtLDAPQuery" runat="server" GroupName="GroupType" Text="LDAP Query"
            ToolTip="LDAP Query Group" TabIndex="4" /><br />
    </div>
</asp:Content>
