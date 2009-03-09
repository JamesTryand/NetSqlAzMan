<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgActiveDirectoryObjectsList.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgActiveDirectoryObjectsList" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <asp:Panel ID="LDAPQueryResultPanel" runat="server" Width="99%" style="visibility: visible">
            <asp:GridView ID="gvLDAPQueryResults" runat="server"
                CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True" Width="100%">
                <FooterStyle BackColor="#E0E0E0" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="sAMAccountName" HeaderText="sAMAccountName">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Name">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="objectClass" HeaderText="objectClass">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="objectSid" HeaderText="objectSid">
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
        </div>
        <asp:Panel ID="pnlWait" runat="server" Width="99%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; vertical-align: middle">
            <br />
            Please wait ...<br />
            <br />
            <asp:Image ID="hourglass" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
        <br />
    </asp:Panel>
</asp:Content>
