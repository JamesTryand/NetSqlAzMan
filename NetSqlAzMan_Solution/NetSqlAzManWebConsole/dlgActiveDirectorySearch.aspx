<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgActiveDirectorySearch.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgActiveDirectorySearch" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
            <asp:GridView ID="gvLDAP" runat="server"
                CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True" Width="100%">
                <FooterStyle BackColor="#E0E0E0" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                </EditItemTemplate>
                <ItemStyle Width="10px" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" Enabled="true" />
                </ItemTemplate>
            </asp:TemplateField>
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
                    <asp:BoundField DataField="ADSPath" HeaderText="ADSPath">
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
        </div>
</asp:Content>
