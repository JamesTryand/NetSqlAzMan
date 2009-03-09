<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgOptions.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgOptions" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="99%">
        <tr>
            <td colspan="4">
                <asp:Label ID="Label1" runat="server" Text=".NET Sql Authorization Manager mode:" Width="300px" Font-Bold="True"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px">
                &nbsp;</td>
            <td style="width: 6px">
                &nbsp; &nbsp;&nbsp;</td>
            <td style="width: 28px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td colspan="3">
                <asp:RadioButton ID="rbAdministrator" runat="server" Width="293px" Font-Bold="True" Text="Administrator mode:" Checked="True" GroupName="Mode" TabIndex="1" /></td>
        </tr>
        <tr>
            <td style="width: 8px; height: 18px;">
            </td>
            <td style="width: 6px; height: 18px;">
            </td>
            <td style="width: 28px; height: 18px;">
                <asp:Label ID="Label2" runat="server" Text="MMC:"></asp:Label></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="In Administrator mode, local Windows Users/Groups and Operations are not visible." Width="400px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 8px; height: 19px;">
            </td>
            <td style="width: 6px; height: 19px;">
            </td>
            <td style="width: 28px; height: 19px;">
                <asp:Label ID="Label4" runat="server" Text="Run-Time:" Width="88px"></asp:Label></td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="In Administrator mode, local Windows Users/Groups authorizations are skipped by CheckAccess methods." Width="400px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 8px; height: 22px;">
            </td>
            <td colspan="3">
                <asp:RadioButton ID="rbDeveloper" runat="server" Font-Bold="True" Text="Developer mode:" GroupName="Mode" TabIndex="2" /></td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
                <asp:Label ID="Label6" runat="server" Text="MMC:"></asp:Label></td>
            <td>
                <asp:Label ID="Label8" runat="server" Text="In Developer mode, users can see Domain/Local Windows Users/Groups and full control on Operations." Width="400px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px; height: 18px;">
            </td>
            <td style="width: 6px; height: 18px;">
            </td>
            <td style="width: 28px; height: 18px;">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
                <asp:Label ID="Label7" runat="server" Text="Run-Time:" Width="89px"></asp:Label></td>
            <td>
                <asp:Label ID="Label9" runat="server" Text="In Developer mode, local Windows Users/Groups and Domain User/Groups authorizations are NOT skipped by CheckAccess methods." Width="400px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="Event Log:"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 8px; height: 22px;">
            </td>
            <td style="width: 6px; height: 22px;">
            </td>
            <td style="width: 28px; height: 22px;">
            </td>
            <td>
                <asp:CheckBox ID="chkLogOnEventLog" runat="server" OnCheckedChanged="chkLogType_CheckedChanged" Text="Log into Event Log" AutoPostBack="True" Width="131px" TabIndex="3" />
                <asp:CheckBox ID="chkLogOnDb" runat="server" OnCheckedChanged="chkLogType_CheckedChanged" Text="Log into Database" AutoPostBack="True" Width="131px" TabIndex="4" /></td>
        </tr>
        <tr>
            <td style="width: 8px">
            </td>
            <td style="width: 6px">
            </td>
            <td style="width: 28px">
            </td>
            <td>
                <asp:CheckBox ID="chkErrors" runat="server" Enabled="False" Text="Errors" TabIndex="5" />
                <asp:CheckBox ID="chkWarnings" runat="server" Enabled="False" Text="Warnings" TabIndex="6" />
                <asp:CheckBox ID="chkInformations" runat="server" Enabled="False" Text="Informations" TabIndex="7" /></td>
        </tr>
    </table>
    
</asp:Content>
