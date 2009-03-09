<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgGenerateCheckAccessHelper.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgGenerateCheckAccessHelper" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h4>
        <table width="100%">
            <tr>
                <td style="width: 139px">
                    Class name:</td>
                <td style="width: 275px">
                    <asp:TextBox ID="txtClassName" runat="server" Width="230px" TabIndex="1">CheckAccessHelper</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">' ControlToValidate="txtClassName"></asp:RequiredFieldValidator></td>
                <td>
                    Choose one or more 
                    <br />
                    generation options:</td>
            </tr>
            <tr>
                <td style="width: 139px">
                    Namespace:</td>
                <td style="width: 275px">
                    <asp:TextBox ID="txtNamespace" runat="server" Width="230px" TabIndex="2">[ApplicationName].Security</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">' ControlToValidate="txtNamespace"></asp:RequiredFieldValidator></td>
                <td>
                    <asp:CheckBox ID="chkAllowRoles" runat="server" Text="Allow Role Check Access" TabIndex="5" /></td>
            </tr>
            <tr>
                <td style="width: 139px">
                </td>
                <td style="width: 275px">
                </td>
                <td>
                    <asp:CheckBox ID="chkAllowTasks" runat="server" Text="Allow Task Check Access" TabIndex="6" /></td>
            </tr>
            <tr>
                <td style="width: 139px">
                    Source Code Language:</td>
                <td style="width: 275px">
                    <asp:RadioButton ID="rbCSharp" runat="server" Checked="True" GroupName="Language"
                        Text="C#" TabIndex="3" />
                    <asp:RadioButton ID="rbVBNet" runat="server" GroupName="Language" Text="VB.NET" TabIndex="4" /></td>
                <td>
                    <asp:CheckBox ID="chkAllowOperations" runat="server" Checked="True" Enabled="False"
                        Text="Allow Operation Check Access" TabIndex="7" /></td>
            </tr>
        </table>
        </h4>
        <asp:Panel ID="pnlSourceCode" runat="server" GroupingText="Source Code"
            Width="100%">
            <asp:TextBox ID="txtSourceCode" runat="server" BackColor="Blue" ForeColor="Yellow" style="background-image: none;"
                Height="200px" ReadOnly="True" TextMode="MultiLine" Width="99%" Wrap="False" TabIndex="8">(source code)</asp:TextBox></asp:Panel>
    <input id="btnCopy" runat="server" type="button" value="Copy All" tabindex="9" />
        <asp:Button ID="btnGenerate" runat="server" Font-Bold="True" OnClick="btnGenerate_Click"
            Text="Generate" TabIndex="10" />
</asp:Content>
