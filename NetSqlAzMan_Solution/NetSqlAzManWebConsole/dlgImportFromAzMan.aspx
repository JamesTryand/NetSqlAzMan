<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgImportFromAzMan.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgImportFromAzMan" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
    <asp:Label ID="Label2" runat="server" Text=" Import Store from Microsoft Authorization Manager (AzMan)"></asp:Label>&nbsp;</h2>
    <p>
        <asp:Panel ID="pnlImportFromAzMan" runat="server" Height="270px" Width="90%" style="visibility: visible">
        <asp:Panel ID="pnlSource" runat="server" Width="90%" GroupingText="MS AzMan source">
            Source type:
            <asp:RadioButton ID="rbtStoreFile" runat="server" AutoPostBack="True" GroupName="SourceType" Checked="True" OnCheckedChanged="rbtStoreFile_CheckedChanged" Text="XML File" ToolTip="XML File" TabIndex="1" />
            <asp:RadioButton ID="rbtStorePath" runat="server" AutoPostBack="True" GroupName="SourceType" OnCheckedChanged="rbtStoreFile_CheckedChanged" Text="Network Path" ToolTip="Network Path" TabIndex="2" /><br />
        <asp:Label ID="lblStoreFile" runat="server" Text="MS AzMan XML Store file:" Font-Bold="True"></asp:Label>
        <asp:FileUpload ID="FileUpload1" runat="server" Width="90%" TabIndex="3" />
        <asp:RequiredFieldValidator ID="rfvFile" runat="server" ControlToValidate="FileUpload1"
            ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
            <asp:Label ID="lblStorePath" runat="server" Text="MS AzMan Network path:" Font-Bold="True"></asp:Label><br />
            <asp:TextBox ID="txtAzManStorePath" runat="server" Width="90%" TabIndex="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPath" runat="server" ControlToValidate="txtAzManStorePath"
                ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
            <asp:Label ID="lblDescription1" runat="server" Font-Size="XX-Small" Text="(ex.: MSLDAP://myserver:389/DC=MyCompany, DC=org, DC=MyPartition)"></asp:Label><br />
            <asp:Label ID="lblDescription2" runat="server" Font-Size="XX-Small" Text="(ex.: MSSQL://[connection string]/[database name]/[policy store name])"></asp:Label></asp:Panel>
            <br />
        <asp:Panel ID="pnlDestination" runat="server" Width="90%" GroupingText="NetSqlAzMan - Destination">
            <asp:Label ID="Label1" runat="server" Text="Store name:" Font-Bold="True"></asp:Label><br />
            <asp:TextBox ID="txtNetSqlAzManStoreName" runat="server" Width="90%" TabIndex="5"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDestination" runat="server" ControlToValidate="txtNetSqlAzManStoreName"
                ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
            <asp:Label ID="Label3" runat="server" Font-Size="XX-Small" Text="* Any of the Authorization Scripts will be imported."></asp:Label><br />
        </asp:Panel>
        </asp:Panel>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; position: absolute; top: 150px; left: 101px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
