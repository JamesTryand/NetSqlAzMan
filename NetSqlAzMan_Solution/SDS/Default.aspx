<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SDS (Software Development System) Application Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnBudgetCheck" runat="server" Style="z-index: 100; left: 562px;
            position: absolute; top: 38px" Text="Controllo del Budget" />
        <asp:Label ID="lblIAM" runat="server" Style="z-index: 102; left: 34px; position: absolute;
            top: 88px" Text="Label" Font-Bold="True" ForeColor="Red"></asp:Label>
        <img src="Images/Organigramma.png" />
        <asp:Label ID="lblDateTime" runat="server" Font-Bold="True" ForeColor="Blue" Style="z-index: 102;
            left: 34px; position: absolute; top: 109px"></asp:Label>
        <asp:Button ID="btnUndelegate" runat="server" Style="z-index: 100; left: 26px;
            position: absolute; top: 200px" Text="Annulla Delega" Width="207px" OnClick="btnUndelegate_Click" />
        <asp:Button ID="btnDelegateForBudgetCheck" runat="server" Style="z-index: 100; left: 25px;
            position: absolute; top: 169px" Text='Delega per "Controllo del Budget"' Width="208px" OnClick="btnDelegateForBudgetCheck_Click" />
        &nbsp;
        <asp:Button ID="btnDevelopment" runat="server" Style="z-index: 100; left: 379px;
            position: absolute; top: 638px" Text="Sviluppo" Width="230px" />
        <asp:Button ID="btnTimesheetCompile" runat="server" Style="z-index: 100; left: 374px;
            position: absolute; top: 468px" Text="Compilazione del Timesheet" />
        <asp:Button ID="btnTimesheetCheck" runat="server" Style="z-index: 100; left: 393px;
            position: absolute; top: 333px" Text="Approvazione del TimeSheet" Width="206px" />
        <asp:Button ID="btnConstraintCheck" runat="server" Style="z-index: 100; left: 411px;
            position: absolute; top: 210px" Text="Controllo dei Vincoli" />
        <asp:Button ID="btnCustomerRelationshipManagement" runat="server" Style="z-index: 100; left: 404px;
            position: absolute; top: 181px" Text="Relazioni con i Clienti" /></div>
    </form>
</body>
</html>
