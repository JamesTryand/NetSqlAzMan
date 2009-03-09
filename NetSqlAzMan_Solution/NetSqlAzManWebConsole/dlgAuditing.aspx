<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgAuditing.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgAuditing" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
    <asp:Label ID="Label2" runat="server" Text="Generate T-SQL Script to enable Auditing on NetSqlAzMan Storage."></asp:Label>
    </h2>
    <asp:Panel ID="pnlAudit" runat="server" Width="90%" style="visibility: visible">
    <asp:TextBox ID="txtDDLScript" runat="server" CssClass="modalDialogMultiLine" MaxLength="1024"
        TextMode="MultiLine" TabIndex="1" Width="95%" ReadOnly="True" style="font-family: Courier New; font-size:0.9em" Height="226px"></asp:TextBox>
    <br />
    <asp:RadioButton ID="tsbCreateTablesTriggerAndViews" runat="server" Checked="True"
        GroupName="GenerationType" Text="Create Audit Tables, Trigger and Views" TabIndex="2" />
    <asp:RadioButton ID="tspDropTablesTriggersAndViews" runat="server" GroupName="GenerationType"
        Text="Drop Audit Tables, Triggers and Views" TabIndex="3" />
    <asp:RadioButton ID="tsbCreateAuditTriggersOnly" runat="server" GroupName="GenerationType"
        Text="Create Audit Triggers only" TabIndex="4" />
    <asp:RadioButton ID="tsbDropAuditTriggersOnly" runat="server" GroupName="GenerationType"
        Text="Drop Audit Triggers only" TabIndex="5" />
    <br />
    <div style="text-align: center">
        <asp:ImageButton ID="imgGenerate" runat="server" ImageUrl="~/images/BuildDDLScript.bmp"
            OnClick="imgGenerate_Click" ToolTip="Generate DDL Script" Width="30px"  BorderWidth="2px" BorderColor="White" BorderStyle="Groove" style="padding: 1px 1px;" TabIndex="6" />
<h4 style="margin: 0 0 0 0;">
        <asp:Label ID="Label1" runat="server" Text="SQLAudit is open source project (LGPL) by Andrea Ferendeles." TabIndex="7"></asp:Label>
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://sqlaudit.sourceforge.net"
            Target="_blank" TabIndex="8">http://sqlaudit.sourceforge.net</asp:HyperLink>
</h4>            
            </div>
            </asp:Panel>
            <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
