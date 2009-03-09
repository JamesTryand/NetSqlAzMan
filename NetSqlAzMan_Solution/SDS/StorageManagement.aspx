<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StorageManagement.aspx.cs" Inherits="StorageManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCreateStore" runat="server" OnClick="btnCreateStore_Click" Text="Create Store Programmatically" />
        <br />
        <asp:Button ID="btnPickUpItemsCount" runat="server" OnClick="btnPickUpItemsCount_Click"
            Text="Pick up App 1 items count" Width="267px" />
        <asp:TextBox ID="txtItemsCount" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnDeleteStore" runat="server" OnClick="btnDeleteStore_Click" Text="Delete Store Programmatically" /></div>
    </form>
</body>
</html>
