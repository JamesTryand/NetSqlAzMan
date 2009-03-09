<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StorageConnection.aspx.cs" Inherits="NetSqlAzManWebConsole.StorageConnection" StylesheetTheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <%--<meta http-equiv="Page-Enter" content="revealTrans(Duration=3.0,Transition=0)" />--%>
    <meta http-equiv="Page-Exit" content="revealTrans(Duration=1.0,Transition=0)" />
    <title>NetSqlAzMan - .NET Sql Authorization Manager - Storage connection</title>
    <script type="text/javascript" src="javascript/common.js" language="javascript"></script>
    <script language="javascript" type="text/javascript">
    <!--
        function centerDiv()
        {
            document.getElementById('centerDiv').style.marginLeft=new String(document.body.clientWidth/2 - document.getElementById('Panel1').clientWidth/2) + 'px';
        }
        window.onresize = centerDiv;
    -->
    </script>

</head>
<body onload="centerDiv()">
    <form id="form1" runat="server">
    <div id="centerDiv">
        <asp:Panel ID="Panel1" runat="server" Height="50px" Width="572px" GroupingText="Storage Connection" DefaultButton="btnOk">
            <table>
                <tr>
                    <td colspan="2" style="width: 389px; text-align: left; background-color: #e0e0e0;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/NetSqlAzMan.gif" style="float: left;" />&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #e0e0e0; text-align: right">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Label ID="Label1" runat="server" Text="Data Source:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px">
                        <asp:DropDownList ID="cmbDataSources" runat="server" AutoPostBack="True" Width="310px" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredDataSourceValidator" runat="server" ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">' ControlToValidate="cmbDataSources"></asp:RequiredFieldValidator>
                        <asp:Button ID="btnRefreshDataSource" runat="server" Text="Refresh" TabIndex="2" CausesValidation="False" OnClick="btnRefreshDataSource_Click" OnClientClick="javascript:doHourglass();" /><br />
                        <span style="font-size: 8pt">add entry (machine\instance, port):</span><br />
                        <asp:TextBox ID="txtDataSource" runat="server" ToolTip="3" Width="310px"></asp:TextBox>
                        <asp:Button ID="btnAddDataSource" runat="server" CausesValidation="False" OnClick="btnAddDataSource_Click"
                            Text="Add" ToolTip="4" OnClientClick="javascript:doHourglass();" /></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Label ID="Label2" runat="server" Text="Authentication:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px">
                        <asp:RadioButton ID="rbIntegrated" runat="server" Checked="True" Text="Integrated" TabIndex="5" AutoPostBack="True" GroupName="Authentication" OnCheckedChanged="rbAuthentication_CheckedChanged" />
                        <asp:RadioButton ID="rbSql" runat="server" Text="Sql" TabIndex="6" AutoPostBack="True" GroupName="Authentication" OnCheckedChanged="rbAuthentication_CheckedChanged" /></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Label ID="Label3" runat="server" Text="User Id:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px">
                        <asp:TextBox ID="txtUserId" runat="server" Width="350px" TabIndex="7" AutoCompleteType="DisplayName"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredUserIdValidator" runat="server" ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">' ControlToValidate="txtUserId" Enabled="False"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 389px; height: 26px;">
                        <asp:Label ID="lblPassword" runat="server" Text="Password:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px; height: 26px;">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="350px" TabIndex="8" AutoCompleteType="Disabled" OnPreRender="Password_PreRender"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Label ID="Label4" runat="server" Text="NetSqlAzMan DB:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px">
                        <asp:DropDownList ID="cmbDatabases" runat="server" Width="310px" TabIndex="9">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredDBValidator" runat="server" ControlToValidate="cmbDatabases"
                            ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator>
                        <asp:Button ID="btnRefreshDatabases" runat="server" Text="Refresh" TabIndex="10" CausesValidation="False" OnClick="btnRefreshDatabases_Click" OnClientClick="javascript:doHourglass();" /></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Label ID="Label5" runat="server" Text="Other Settings:" Font-Bold="True"></asp:Label></td>
                    <td style="width: 1070px">
                        <asp:TextBox ID="txtOtherSettings" runat="server" TabIndex="11" Width="350px" AutoCompleteType="Notes"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                    </td>
                    <td style="width: 1070px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 389px; text-align: left;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/NetSqlAzMan_32x32.gif" /></td>
                    <td style="width: 1070px; text-align: right;">
                        <asp:Label ID="Label6" runat="server" Text="Manage Authorization Manager Storage connection info."></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right">
                        <hr />
                        <asp:CheckBox ID="chkRemember" runat="server" Text="Remember connection info" TabIndex="12" /></td>
                </tr>
                <tr>
                    <td style="width: 389px">
                        <asp:Button ID="btnTestConnection" runat="server" TabIndex="13" Text="Test" Width="80px" OnClick="btnTestConnection_Click" OnClientClick="javascript:doHourglass();" /></td>
                    <td style="width: 1070px; text-align: right;">
                        &nbsp;
                        <asp:Button ID="btnOk" runat="server" TabIndex="14" Text="OK" Width="80px" Font-Bold="True" OnClick="btnOk_Click" OnClientClick="javascript:doHourglass();" />
                        </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
