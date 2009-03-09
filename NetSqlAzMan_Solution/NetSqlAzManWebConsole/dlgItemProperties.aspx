<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgItemProperties.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgItemProperties" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Menu ID="mnuTab" runat="server" BackColor="#FFFFC0" BorderColor="black" BorderStyle="Solid"
        BorderWidth="1px" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" OnMenuItemClick="mnuTab_MenuItemClick" ForeColor="black" TabIndex="1">
        <Items>
            <asp:MenuItem Text=" " Value=" "></asp:MenuItem>
            <asp:MenuItem Text="Item Definition" ToolTip="Item Definition" Value="Item Definition">
            </asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="Roles" Value="Roles" ToolTip="Roles"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="Tasks" ToolTip="Tasks" Value="Tasks"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="Operations" Value="Operations" ToolTip="Operations"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" " Value=" "></asp:MenuItem>
        </Items>
        <StaticSelectedStyle Font-Bold="True" />
        <StaticHoverStyle Font-Bold="True" Font-Underline="False" />
        <StaticMenuStyle BackColor="#E0E0E0" />
    </asp:Menu>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="tabItemDefinition" runat="server">
            <br />
            <asp:Label ID="lblName" runat="server" CssClass="modalDialogLabel" Text="Name:"></asp:Label><br />
    <asp:TextBox ID="txtName" runat="server" CssClass="modalDialogSingleLine" MaxLength="255" TabIndex="2" OnTextChanged="txtName_TextChanged"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
        ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
    <asp:Label ID="lblDescription" runat="server" CssClass="modalDialogLabel" Text="Description:"></asp:Label><br />
    <asp:TextBox ID="txtDescription" runat="server" MaxLength="1024" Height="240px" Width="90%"
        TextMode="MultiLine" TabIndex="3" OnTextChanged="txtDescription_TextChanged"></asp:TextBox>
            <br />
            <asp:Button ID="btnAttributes" runat="server" Text="Attributes" OnClientClick="javascript:openDialog('dlgItemAttributes.aspx', 1);" TabIndex="4" />
            <asp:Button ID="btnBizRule" runat="server" Text="Biz Rule" OnClientClick="javascript:openDialog('dlgBizRule.aspx', 1);" TabIndex="5" /><br />
            </asp:View><asp:View ID="tabRoles" runat="server">
                <asp:Panel ID="pnlRoles" runat="server" Height="300px" ScrollBars="Both" Width="100%">
                Roles:<br />
                <asp:GridView ID="dgRoles" runat="server" TabIndex="6" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" Enabled="true" />
                            </EditItemTemplate>
                            <ItemStyle Width="10px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemId" HeaderText="ItemId" >
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
                <br />
                <asp:Button ID="btnAddRole" runat="server" TabIndex="6" Text="Add" OnClientClick="javascript:openDialog('dlgItemsList.aspx', 2);" />
                <asp:Button ID="btnRemoveRole" runat="server" TabIndex="7" Text="Remove" OnClick="btnRemoveRole_Click" /></asp:View>
        <asp:View ID="tabTasks" runat="server">
            <asp:Panel ID="pnlTasks" runat="server" Height="300px" ScrollBars="Both" Width="100%">
            Tasks:<br />
            <asp:GridView ID="dgTasks" runat="server" TabIndex="6" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Enabled="true" />
                        </EditItemTemplate>
                        <ItemStyle Width="10px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
                <br />
                &nbsp;</asp:Panel>
            <br />
            <asp:Button ID="btnAddTask" runat="server" TabIndex="8" Text="Add" OnClientClick="javascript:openDialog('dlgItemsList.aspx', 2);" />
            <asp:Button ID="btnRemoveTask" runat="server" TabIndex="9" Text="Remove" OnClick="btnRemoveTask_Click" /><br />
            </asp:View>
        <asp:View ID="tabOperations" runat="server">
            <asp:Panel ID="pnlOperations" runat="server" Height="300px" ScrollBars="Both" Width="100%">
            Operations:<br />
            <asp:GridView ID="dgOperations" runat="server" TabIndex="6" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Enabled="true" />
                        </EditItemTemplate>
                        <ItemStyle Width="10px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
            &nbsp;<br />
            </asp:Panel>
            <br />
            <asp:Button ID="btnAddOperation" runat="server" TabIndex="10" Text="Add" OnClientClick="javascript:openDialog('dlgItemsList.aspx', 2);" />
            <asp:Button ID="btnRemoveOperation" runat="server" TabIndex="11" Text="Remove" OnClick="btnRemoveOperation_Click" /></asp:View>
    </asp:MultiView>
</asp:Content>
