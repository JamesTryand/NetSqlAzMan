<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgItemAttributes.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgItemAttributes" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:GridView ID="gvAttributes" runat="server"
            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDeleting="gvAttributes_RowDeleting" AutoGenerateColumns="False" ShowFooter="True" OnRowCancelingEdit="gvAttributes_RowCancelingEdit" OnRowEditing="gvAttributes_RowEditing" OnRowUpdating="gvAttributes_RowUpdating" Width="100%">
            <FooterStyle BackColor="#E0E0E0" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/undo.gif" DeleteImageUrl="~/images/delete.gif"
                    EditImageUrl="~/images/edit.gif" ShowEditButton="True" UpdateImageUrl="~/images/Ok.gif" >
                    <FooterStyle Width="6%" />
                </asp:CommandField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.gif" OnClientClick="return confirm('Delete Attribute?');" ToolTip="Delete" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="imgNew" runat="server" ImageUrl="~/images/Add.gif" OnClick="imgNew_Click" ToolTip="New Attribute" />
                        <asp:ImageButton ID="imgOk" runat="server" ImageUrl="~/images/Ok.gif" OnClick="imgOk_Click"
                            ToolTip="Insert" Visible="False" />
                        <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~/images/undo.gif" OnClick="imgCancel_Click"
                            ToolTip="Cancel" Visible="False" />&nbsp;
                    </FooterTemplate>
                    <FooterStyle Width="6%" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="Key" ReadOnly="True" HeaderText="Original Key" Visible="False" >
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Key">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtKey" runat="server" Text='<%# Bind("Key") %>'></asp:TextBox>&nbsp;
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblKey" runat="server" Text='<%# Bind("Key") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;
                        <asp:TextBox ID="txtNewKey" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtValue" runat="server" Text='<%# Bind("Value") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;
                        <asp:TextBox ID="txtNewValue" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        &nbsp; &nbsp;&nbsp; &nbsp;</div>
</asp:Content>
