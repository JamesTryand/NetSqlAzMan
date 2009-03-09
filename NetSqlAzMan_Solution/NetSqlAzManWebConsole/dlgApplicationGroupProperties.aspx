<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgApplicationGroupProperties.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgApplicationGroupProperties" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Menu ID="mnuTab" runat="server" BackColor="#FFFFC0" BorderColor="black" BorderStyle="Solid"
        BorderWidth="1px" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" OnMenuItemClick="mnuTab_MenuItemClick" ForeColor="black" TabIndex="1">
        <Items>
            <asp:MenuItem Text=" " Value=" "></asp:MenuItem>
            <asp:MenuItem Text="General" ToolTip="General" Value="General">
            </asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="LDAP Query" Value="LDAP Query"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="Members" ToolTip="Members" Value="Members"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" | " Value=" | "></asp:MenuItem>
            <asp:MenuItem Text="Non Members" Value="Non Members"></asp:MenuItem>
            <asp:MenuItem Selectable="False" Text=" " Value=" "></asp:MenuItem>
        </Items>
        <StaticSelectedStyle Font-Bold="True" />
        <StaticHoverStyle Font-Bold="True" Font-Underline="False" />
        <StaticMenuStyle BackColor="#E0E0E0" />
    </asp:Menu>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="tabGeneral" runat="server">
            <br />
            <asp:Label ID="lblName" runat="server" CssClass="modalDialogLabel" Text="Name:"></asp:Label><br />
    <asp:TextBox ID="txtName" runat="server" CssClass="modalDialogSingleLine" MaxLength="255" TabIndex="1" OnTextChanged="txtName_TextChanged"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
        ErrorMessage='<img src="images/ExclamationMark.gif" title="Required" class="requiredImage">'></asp:RequiredFieldValidator><br />
    <asp:Label ID="lblDescription" runat="server" CssClass="modalDialogLabel" Text="Description:"></asp:Label><br />
    <asp:TextBox ID="txtDescription" runat="server" MaxLength="1024" Height="215px" Width="90%"
        TextMode="MultiLine" TabIndex="2" OnTextChanged="txtDescription_TextChanged"></asp:TextBox>
            <br />
            <asp:Label ID="Label1" runat="server" CssClass="modalDialogLabel" Text="Group Type:"></asp:Label><br />
            <asp:TextBox ID="txtGroupType" runat="server" CssClass="modalDialogSingleLine" ReadOnly="true" TabIndex="3" ></asp:TextBox></asp:View>
        <asp:View ID="tabLdapQuery" runat="server">
            <br />
            The LDAP Query that defines members of this group.<br />
            <br />
            <asp:TextBox ID="txtLDapQuery" runat="server" Width="90%" Height="240px" MaxLength="1024"
                TabIndex="4" TextMode="MultiLine" OnTextChanged="txtLDapQuery_TextChanged"></asp:TextBox><br />
            <br />
            <asp:Button ID="btnTestLDapQuery" runat="server" TabIndex="5" Text="Execute LDAP Query" OnClientClick="javascript:openDialogWithArguments('dlgActiveDirectoryObjectsList.aspx', 'LDAPQuery', 'ctl00_ContentPlaceHolder1_txtLDapQuery', 2);" /></asp:View>
        <asp:View ID="tabMembers" runat="server">
        <asp:Panel ID="pnlMembers" runat="server" Width="100%" Height="270px" ScrollBars="Both">
            <br />
            Users and Groups that are members of this group.<br />
            <br />
            <asp:GridView ID="dgMembers" runat="server" TabIndex="6" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Where Defined">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("WhereDefined") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("WhereDefined") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="SID" HeaderText="SID" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
            <br />
            <asp:Button ID="btnMembersAddStoreGroup" runat="server" TabIndex="6" Text="Add Store Groups" OnClientClick="javascript:openDialog('dlgStoreGroupsList.aspx', 2);" />
            <asp:Button ID="btnMembersAddApplicationGroup" runat="server" TabIndex="7" Text="Add Application Groups" OnClientClick="javascript:openDialog('dlgApplicationGroupsList.aspx', 2);" />
            <asp:Button ID="btnMembersAddWindowsUsersAndGroups" runat="server" TabIndex="8" Text="Add Windows Users and Groups" OnClientClick="javascript:openDialog('ADObjectPickerShowDialog.aspx', 2);" />
            <asp:Button ID="btnMembersAddDBUsers" runat="server" TabIndex="9" Text="Add DB Users" OnClientClick="javascript:openDialog('dlgDBUsersList.aspx', 2);" />
            <asp:Button ID="btnMembersRemove" runat="server" TabIndex="10" Text="Remove" OnClick="btnMembersRemove_Click" /></asp:View>
        <asp:View ID="tabNonMembers" runat="server">
        <asp:Panel ID="pnlNonMembers" runat="server" Width="100%" Height="270px" ScrollBars="Both">
            <br />
            Users and Groups that are excluded from this group.<br />
            <br />
            <asp:GridView ID="dgNonMembers" runat="server" TabIndex="11" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                    <asp:BoundField DataField="Name" HeaderText="Name" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="WhereDefined" HeaderText="Where Defined" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SID" HeaderText="SID" />
                </Columns>
            </asp:GridView>
            </asp:Panel>
            <br />
            <asp:Button ID="btnNonMembersAddStoreGroup" runat="server" TabIndex="11" Text="Add Store Groups" OnClientClick="javascript:openDialog('dlgStoreGroupsList.aspx', 2);" />
            <asp:Button ID="btnNonMembersAddApplicationGroup" runat="server" TabIndex="12" Text="Add Application Groups" OnClientClick="javascript:openDialog('dlgApplicationGroupsList.aspx', 2);" />&nbsp;<asp:Button
                ID="btnNonMembersAddWindowsUsersAndGroup" runat="server" TabIndex="13" Text="Add Windows Users and Groups" OnClientClick="javascript:openDialog('ADObjectPickerShowDialog.aspx', 2);" />&nbsp;<asp:Button
                    ID="btnNonMembersAddDBUsers" runat="server" TabIndex="14" Text="Add DB Users" OnClientClick="javascript:openDialog('dlgDBUsersList.aspx', 2);" />&nbsp;<asp:Button ID="btnNonMembersRemove" runat="server" TabIndex="15" Text="Remove" OnClick="btnNonMembersRemove_Click" /></asp:View>
    </asp:MultiView>
</asp:Content>
