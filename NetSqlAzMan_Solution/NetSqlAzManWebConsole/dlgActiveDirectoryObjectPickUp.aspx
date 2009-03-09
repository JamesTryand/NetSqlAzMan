<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgActiveDirectoryObjectPickUp.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgActiveDirectoryObjectPickUp" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <asp:Panel ID="LDAPQueryResultPanel" runat="server" Width="99%" style="visibility: visible">
        <asp:Panel ID="pnlToResolve" runat="server" Height="50px" Width="100%">
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label><br />
            <br />
            <asp:TextBox ID="txtUnknow" runat="server" TabIndex="1" Width="327px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUnknow"
                ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
            <br />
            <asp:Button ID="btnChange" runat="server" OnClick="btnChange_Click" Text="Change" TabIndex="2" />
        &nbsp;<asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove from list" CausesValidation="False" TabIndex="3" /><br />
            <br />
            <asp:GridView ID="gvLDAPQueryResults" runat="server"
                CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True" Width="100%" OnSelectedIndexChanging="gvLDAPQueryResults_SelectedIndexChanging">
                <FooterStyle BackColor="#E0E0E0" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Name" HeaderText="Name">
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UPN" HeaderText="UPN">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="objectClass" HeaderText="objectClass">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ADSPath" HeaderText="ADSPath">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sid" HeaderText="Sid">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        </asp:Panel>
        </div>
        <asp:Panel ID="pnlWait" runat="server" Width="99%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; vertical-align: middle">
            <br />
            Please wait ...<br />
            <br />
            <asp:Image ID="hourglass" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
