<%@ Page Language="C#" MasterPageFile="~/ModalDialog.Master" AutoEventWireup="true" CodeBehind="dlgItemAuthorizations.aspx.cs" Inherits="NetSqlAzManWebConsole.dlgItemAuthorizations" Title="NetSqlAzMan Web Console" StylesheetTheme="Default" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 
    <asp:Panel ID="pnlWait" runat="server" Width="83%" Height="250px" BorderColor="black" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" style="visibility: hidden; position: absolute; top: 150px; left: 100px; vertical-align: middle">
        <br />
        Please wait ...<br />
        <br />
        <asp:Image ID="imgHourGlass" runat="server" ImageUrl="~/images/hourglass.gif" /><br />
    </asp:Panel>
    -->
        <asp:Panel ID="Panel4" runat="server" Height="290px" Width="720px" ScrollBars="Both" Wrap="False">
            <asp:GridView ID="dgAuthorizations" runat="server" CellPadding="4" ForeColor="#333333" TabIndex="6" Width="95%" AutoGenerateColumns="False" OnRowCancelingEdit="dgAuthorizations_RowCancelingEdit" OnRowEditing="dgAuthorizations_RowEditing" OnRowUpdating="dgAuthorizations_RowUpdating">
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
                    <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/undo.gif" EditImageUrl="~/images/edit.gif"
                        ShowEditButton="True" UpdateImageUrl="~/images/Ok.gif" >
                        <ItemStyle Wrap="False" />
                    </asp:CommandField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgAttributes" runat="server" CausesValidation="False" CommandName="Attributes" 
                                ImageUrl="~/images/AuthorizationAttribute_16x16.gif" OnClientClick='<%# Eval("AttributesLink") %>' ToolTip="Attributes" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Member Type">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemTemplate>
                            <asp:Image ID="imgMemberType" runat="server" ImageUrl='<%# Eval("MemberType") %>' ToolTip='<%# getMemberTypeToolTip((string)Eval("MemberType")) %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ObjectSID" HeaderText="Object SID" ReadOnly="True">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Authorization Type">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlAuthorizationType" runat="server" SelectedIndex='<%# getSelectedIndex((int)Eval("AuthorizationID")) %>'>
                                <asp:ListItem>Allow With Delegation</asp:ListItem>
                                <asp:ListItem>Allow</asp:ListItem>
                                <asp:ListItem>Deny</asp:ListItem>
                                <asp:ListItem>Neutral</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemTemplate>
                            <asp:Image ID="imgAuthorizationType" runat="server" ImageUrl='<%# Eval("AuthorizationType") %>' ToolTip='<%# getAuthorizationTypeToolTip((string)Eval("AuthorizationType")) %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WhereDefined" HeaderText="Where Defined" ReadOnly="True" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Valid From" >
                        <EditItemTemplate>
                            <asp:TextBox ID="txtValidFrom" runat="server" Text='<%# Bind("ValidFrom") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtValidFrom"
                                ErrorMessage="Enter a valid Date/Time (eg.: dd/MM/yyyy hh.mm.ss)"
                                ValidationExpression="^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2})) (20|21|22|23|[0-1]?\d).[0-5]?\d.[0-5]?\d$" Display="Dynamic" ForeColor="Lime"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ValidFrom") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valid To">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtValidTo" runat="server" Text='<%# Bind("ValidTo") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtValidTo"
                                ErrorMessage="Enter a valid Date/Time (eg.: dd/MM/yyyy hh.mm.ss)"
                                ValidationExpression="^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2})) (20|21|22|23|[0-1]?\d).[0-5]?\d.[0-5]?\d$" Display="Dynamic" ForeColor="Lime"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ValidTo") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Owner" DataField="Owner" ReadOnly="True" >
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OwnerSID" HeaderText="Owner SID" ReadOnly="True" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="AuthorizationID" HeaderText="Authorization ID" ReadOnly="True" >
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1bis" runat="server" HorizontalAlign="Right" Width="720px">
            &nbsp;<asp:Button ID="btnRemove" runat="server" Text="Remove" Width="100px" TabIndex="1" OnClick="btnRemove_Click" />

        </asp:Panel>
        
    <asp:Panel ID="Panel2" runat="server" Width="720px">
        <asp:Button ID="btnAddStoreGroups" runat="server" Text="Add Store Groups" Width="200px" OnClientClick="javascript:openDialog('dlgStoreGroupsList.aspx', 2);" TabIndex="2" />
        <asp:Button ID="btnAddWindowsUsersAndGroups" runat="server" Text="Add Windows Users and Groups" Width="250px" OnClientClick="javascript:openDialog('ADObjectPickerShowDialog.aspx', 2);" TabIndex="3" /></asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Width="720px" TabIndex="4">
        <asp:Button ID="btnAddApplicationGroups" runat="server" Text="Add Application Groups" Width="200px" OnClientClick="javascript:openDialog('dlgApplicationGroupsList.aspx', 2);" />
        <asp:Button ID="btnAddDBUsers" runat="server" Text="Add DB Users" Width="250px" OnClientClick="javascript:openDialog('dlgDBUsersList.aspx', 2);" TabIndex="5" /></asp:Panel>
</asp:Content>
