<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true"
    CodeBehind="dlgDBUsersList.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgDBUsersList"
    Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    DB Users list:<br />
    <br />
    <table>
        <colgroup>
            <col width="10%" />
            <col width="30%" />
            <col width="20%" />
            <col width="30%" />
            <col width="10%" />
        </colgroup>
        <tr>
        <td>
            Filter:
        </td>
            <td>
                <asp:DropDownList ID="cmbFieldName" runat="server" Width="100%">
                </asp:DropDownList>
            </td>
            <td>
                        <asp:DropDownList ID="cmbOperator" runat="server" Width="100%">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtFieldValue" runat="server" Width="100%"></asp:TextBox>
                
            </td>
            <td>
            <asp:Button ID="btnApplyFilter" runat="server" Text="Apply filter" 
                    onclick="Filters_Changed" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvDBUsers" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="None" Width="100%">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:TemplateField>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("Select") %>' />
                </EditItemTemplate>
                <ItemStyle Width="10px" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("Select") %>' Enabled="true" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name">
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="SID" HeaderText="Custom SID">
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Type" HeaderText="Type">
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
</asp:Content>
