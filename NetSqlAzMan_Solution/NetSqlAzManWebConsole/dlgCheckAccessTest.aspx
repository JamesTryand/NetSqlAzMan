<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgCheckAccessTest.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgCheckAccessTest" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Panel ID="pnlCheckAccessTest" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/WindowsUser_32x32.gif" /><asp:RadioButton ID="rbWindowsUser" runat="server" Text="Windows User:" AutoPostBack="True" GroupName="CheckAccessTest" OnCheckedChanged="rbWindowsUser_CheckedChanged" Checked="True" TabIndex="1" /></td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtWindowsUser" runat="server" ToolTip="Users rather then current can be selected only from a Windows Server 2003 machine in a Windows 2003 Native Domain (Kerberos Protocol Transition)." Width="200px" TabIndex="2"></asp:TextBox><br />
                    (otheruser@domain.ext)
                </td>
                <td style="text-align: center;">
                    &nbsp;<asp:Button ID="btnCheckAccessTest" runat="server" Text="Start Check Access Test" Width="180px" OnClick="btnCheckAccessTest_Click" TabIndex="8" /></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    Valid for:</td>
                <td style="text-align: left">
                    <asp:TextBox ID="dtValidFor" runat="server" Width="200px" TabIndex="3"></asp:TextBox>
                </td>
                <td style="text-align: center">
                    <asp:CheckBox ID="chkCache" runat="server" AutoPostBack="True" 
                        oncheckedchanged="chkCache_CheckedChanged" TabIndex="7" 
                        Text="UserPermissionCache" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/DBUser_32x32.gif" /><asp:RadioButton ID="rbDBUser" runat="server" Text="Database User:" AutoPostBack="True" GroupName="CheckAccessTest" OnCheckedChanged="rbDBUser_CheckedChanged" TabIndex="4" /></td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtDBUser" runat="server" Width="200px" TabIndex="5"></asp:TextBox>
                    <asp:Button ID="btnBrowseDBUser" runat="server" Text="..." 
                        OnClientClick="javascript:openDialog('dlgDBUsersList.aspx', 2);" 
                        CausesValidation="False" TabIndex="6" /></td>
                <td style="text-align: center;">
                    <asp:CheckBox ID="chkStorageCache" runat="server" AutoPostBack="True" 
                        oncheckedchanged="chkStorageCache_CheckedChanged" TabIndex="7" 
                        Text="StorageCache" />
                </td>
            </tr>
        </table>
        <br />
    <br />
    <asp:Panel ID="Panel1" runat="server" Height="150px" Width="99%" GroupingText="Details">
        <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Width="99%" Wrap="False" Height="140px" ReadOnly="True" TabIndex="9"></asp:TextBox></asp:Panel>
    <br />
        <br />
    <asp:Panel ID="Panel2" runat="server" Width="99%" GroupingText="Check Access results">
        <asp:TreeView ID="itemsHierarchyTreeView" runat="server" Height="99%" ShowLines="True" TabIndex="10" Width="100%" SkipLinkText="">
        </asp:TreeView>
        </asp:Panel>
        <asp:Label ID="lblMessage" runat="server" Text="..." Width="100%"></asp:Label></asp:Panel>
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="imgHourGlass" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
    </asp:Panel>
</asp:Content>
