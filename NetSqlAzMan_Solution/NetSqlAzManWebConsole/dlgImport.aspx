<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgImport.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgImport" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        <asp:Label ID="Label2" runat="server" Text=".NET Sql Authorization Manager Import"></asp:Label>&nbsp;</h2>
    <p>
        &nbsp;<asp:Panel ID="pnlImport" runat="server" Width="90%" Style="visibility: visible">
            <asp:Label ID="Label1" runat="server" Text="Upload XML file to Import:"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" Width="90%" TabIndex="1" />
            <asp:RequiredFieldValidator ID="rfvFile" runat="server" ControlToValidate="FileUpload1" ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
            <br />
            <asp:Panel ID="pnlImportOptions" runat="server" GroupingText="Import Options" Width="90%">
                <asp:CheckBox ID="chkAuthorizations" runat="server" Checked="True" Text="Include Item Authorizations" TabIndex="2" />&nbsp;<br />
                <asp:CheckBox ID="chkUsersAndGroups" runat="server" Text="Include Windows Users / Groups" TabIndex="3" />&nbsp;<br />
                <asp:CheckBox ID="chkDBUsers" runat="server" Text="Include Database Users" TabIndex="4" />
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="pnlMergeOptions" runat="server" GroupingText="Merge Options" Width="90%">
                <asp:CheckBox ID="chkCreatesNewItems" runat="server" Checked="True" Text="Creates new Items" TabIndex="5" />&nbsp;<br />
                <asp:CheckBox ID="chkOverwritesExistingItems" runat="server" Checked="False" Text="Overwrites existing Items" TabIndex="6" />&nbsp;<br />
                <asp:CheckBox ID="chkDeleteMissingItems" runat="server" Checked="False" Text="Delete missing Items" TabIndex="7" />&nbsp;<br />
                <asp:CheckBox ID="chkCreatesNewItemAuthorizations" runat="server" Checked="True" Text="Creates new Item authorizations" TabIndex="8" />&nbsp;<br />
                <asp:CheckBox ID="chkOverwritesItemAuthorizations" runat="server" Checked="False" Text="Overwrites Item authorizations" TabIndex="9" />&nbsp;<br />
                <asp:CheckBox ID="chkDeleteMissingItemAuthorizations" runat="server" Checked="False" Text="Delete missing Item authorizations" TabIndex="10" />&nbsp;<br />
                <br />
            </asp:Panel>
        </asp:Panel>
        &nbsp;</p>
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
