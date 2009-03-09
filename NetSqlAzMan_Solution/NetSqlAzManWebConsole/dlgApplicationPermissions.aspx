<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgApplicationPermissions.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgApplicationPermissions" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table style="width: 100%; height: 90px">
            <tr>
                <td colspan="3" style="text-align: center">
                    <asp:Label ID="Label1" runat="server" Text='Sql Logins with special permissions.<br />(Logins must be added before to Sql Roles: "NetSqlAzMan_Managers", "NetSqlAzMan_Users", "NetSqlAzMan_Readers").<br />Only checked Logins will be granted for this Application.'
                        Width="100%" Font-Size="X-Small"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/NetSqlAzMan_Managers.bmp" ToolTip="Application Managers" /></td>
                <td style="text-align: center">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/NetSqlAzMan_Users.bmp" ToolTip="Application Users" /></td>
                <td style="text-align: center">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/NetSqlAzMan_Readers.bmp" ToolTip="Application Readers" /></td>
            </tr>
            <tr>
                <td style="border-right: black 1px solid; border-top: black 1px solid; min-height: 200px; vertical-align: top; border-left: black 1px solid; border-bottom: black 1px solid;">
                    &nbsp;<asp:CheckBoxList ID="chkManagers" runat="server" style="float: left" Width="100%" TabIndex="1">
                    </asp:CheckBoxList></td>
                <td style="border-right: black 1px solid; border-top: black 1px solid; min-height: 200px; vertical-align: top; border-left: black 1px solid; border-bottom: black 1px solid">
                    &nbsp;<asp:CheckBoxList ID="chkUsers" runat="server" style="float: left" Width="100%" TabIndex="2">
                    </asp:CheckBoxList></td>
                <td style="border-right: black 1px solid; border-top: black 1px solid; min-height: 200px; vertical-align: top; border-left: black 1px solid; border-bottom: black 1px solid">
                    &nbsp;<asp:CheckBoxList ID="chkReaders" runat="server" style="float: left" Width="100%" TabIndex="3">
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td style="text-align: center; background-color: #cccccc;">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Blue" Text="Application Managers"></asp:Label></td>
                <td style="text-align: center; background-color: #cccccc;">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Green" Text="Application Users"></asp:Label></td>
                <td style="text-align: center; background-color: #cccccc;">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="red" Text="Application Readers"></asp:Label></td>
            </tr>
        </table>
        </div>
</asp:Content>
