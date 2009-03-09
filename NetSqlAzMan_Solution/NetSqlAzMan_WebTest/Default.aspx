<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label2" runat="server" Text="I'm"></asp:Label>
        <asp:Label ID="lblIM" runat="server" Text="(login)"></asp:Label><br />
        <br />
        <asp:TextBox ID="txtAzManTestCheckAccessCount" runat="server" Width="91px">1</asp:TextBox>
        <asp:Button ID="btnAzManTestCheckAccess" runat="server" OnClick="btnAzManTestCheckAccess_Click"
            Text="AzMan Test Check Access" />
        <asp:TextBox ID="txtOperation" runat="server" Width="130px">Operation Test</asp:TextBox><br />
        <asp:TextBox ID="txtAzManCheckAccessResults" runat="server" Width="130px">0</asp:TextBox>
        <asp:Label ID="lblAzManCheckAccess" runat="server" Text="(result)"></asp:Label><br />
        <br />
        <asp:TextBox ID="txtNetSqlAzManTestCheckAccessCount" runat="server" Width="91px">1</asp:TextBox>&nbsp;<asp:Button
            ID="btnNetSqlAzManTestCheckAccess" runat="server" Text="NetSqlAzMan Test Check Access"
            Width="238px" OnClick="btnNetSqlAzManTestCheckAccess_Click" />
        <asp:TextBox ID="txtItem" runat="server" Width="130px">Item Test</asp:TextBox><br />
        <asp:TextBox ID="txtNetSqlAzManCheckAccessResults" runat="server" Width="130px">0</asp:TextBox>
        <asp:Label ID="lblNetSqlAzManCheckAccess" runat="server" Text="(result)"></asp:Label><br />
        <br />
        <asp:TextBox ID="txtNetSqlAzManTestDirectCheckAccessCount" runat="server" Style="position: relative"
            Width="91px">1</asp:TextBox>
        <asp:Button
            ID="btnNetSqlAzManTestDirectCheckAccess" runat="server" Text="NetSqlAzMan Test Direct Check Access"
            Width="260px" OnClick="btnNetSqlAzManTestDirectCheckAccess_Click" style="position: relative" />
        <asp:TextBox ID="txtDirectItem" runat="server" Style="position: relative" Width="130px">Item Test</asp:TextBox><br />
        <asp:TextBox ID="txtNetSqlAzManDirectCheckAccessResults" runat="server" Style="position: relative"
            Width="130px">0</asp:TextBox>
        <asp:Label ID="lblNetSqlAzManDirectCheckAccess" runat="server" Style="position: relative"
            Text="(result)"></asp:Label>
        <asp:CheckBox ID="chkNetSqlAzManDirectMultiThread" runat="server" Style="z-index: 101; left: 95px; position: relative;
            top: 0px" Text="Multi Thread" /><br />
        <asp:CheckBox ID="chkNetSqlAzManMultiThread" runat="server" Style="z-index: 101; left: 290px; position: absolute;
            top: 148px" Text="Multi Thread" />
        <br />
        <asp:CheckBox ID="chkAzManMultiThread" runat="server" Style="z-index: 102; left: 290px;
            position: absolute; top: 77px" Text="Multi Thread" />
        <asp:Button ID="btnCheckAccess" runat="server" OnClick="btnCheckAccess_Click" Text="Check Access" />
        <asp:TextBox ID="txtCheckAccessResults" runat="server"></asp:TextBox></div>
    </form>
</body>
</html>
