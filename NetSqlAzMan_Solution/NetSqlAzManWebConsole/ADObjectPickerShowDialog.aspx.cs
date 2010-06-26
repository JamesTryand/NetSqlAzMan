using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using NetSqlAzMan;

namespace NetSqlAzManWebConsole
{
    public partial class ADObjectPickerShowDialog : dlgPage
    {
        protected internal ADObjectType adObjectType;
        private List<ADObject> adObjects = null;

        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Active Directory Object Picker")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("ActiveDirectory.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.adObjectType = (ADObjectType)this.Session["ADObjectType"];
                switch (this.adObjectType)
                {
                    case ADObjectType.UsersAndGroups: this.lblTopDescription.Text = "Choose Users and/or Groups:"; break;
                    case ADObjectType.UsersOnly: this.lblTopDescription.Text = "Choose Users:"; break;
                    case ADObjectType.OneUserOnly: this.lblTopDescription.Text = "Choose 1 User:"; break;
                }
                this.Text = "Active Directory Search";
                this.Description = this.Text;
                this.Title = this.Text;
                this.txtInput.Focus();
            }
            else
            {
                if (this.Session["selectedADObjects"] != null)
                {
                    this.txtInput.Text = String.Empty;
                    foreach (ADObject ado in (List<ADObject>)this.Session["selectedADObjects"])
                    {
                        this.txtInput.Text += ado.Name + "; ";
                    }
                    this.checkNames();
                }
                if (this.Session["selectedADObjectsFromList"] != null)
                {
                    foreach (ADObject ado in (List<ADObject>)this.Session["selectedADObjectsFromList"])
                    {
                        this.txtInput.Text += ado.Name + "; ";
                    }
                    this.checkNames();
                }
            }
            this.btnBrowse.Enabled = this.Application["Active Directory List"] != null && ((DataView)this.Application["Active Directory List"]).Table.Rows.Count > 0;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkNames())
                {
                    this.Session["selectedADObjects"] = this.adObjects;
                    this.closeWindow(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private bool checkNames()
        {
            this.Session["selectedADObjects"] = null;
            this.Session["selectedADObjectsFromList"] = null;
            bool allResolved = true;
            if (this.txtInput.Text.Trim() != String.Empty && !this.txtInput.Text.EndsWith(";"))
            {
                this.txtInput.Text += ";";
            }
            string[] sadObjects = HttpUtility.HtmlDecode(this.txtInput.Text).Split(';');
            if (this.adObjectType == ADObjectType.OneUserOnly && sadObjects.Length != 1)
            {
                throw new InvalidOperationException("Please select 1 User.");
            }
            this.adObjects = new List<ADObject>();
            foreach (string adObject in sadObjects)
            {
                if (adObject.Trim() != String.Empty)
                {
                    try
                    {
                        ADObject ado = this.resolveName(adObject);
                        if (ado.state != ADObjectState.Resolved)
                            allResolved = false;
                        adObjects.Add(ado);
                    }
                    catch (Exception ex)
                    {
                        this.ShowError(String.Format("Unable to resolve: {0}.\r\n\r\nError:\r\n{1}", adObject, ex.Message));
                    }
                }
            }
            if (allResolved)
            {
                this.Session["selectedADObjects"] = null;
                this.Session["proposedADObjects"] = null;
                this.txtInput.Text = String.Empty;
                if (adObjects != null)
                {
                    foreach (ADObject ado in adObjects)
                    {
                        this.txtInput.Text += HttpUtility.HtmlEncode(ado.Name) + "; ";
                    }
                }
                return true;
            }
            else
            {
                this.Session["selectedADObjects"] = adObjects;
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "allResolvedVariableDeclaration", "var allResolved;", true);
                this.RegisterEndClientScript("do { openDialog('dlgActiveDirectoryObjectPickUp.aspx', 3); } while (!allResolved)");
                return false;
            }
        }

        private ADObject resolveName(string name)
        {
            name = name.Trim();
            DirectoryEntry root = Utility.NewDirectoryEntry("LDAP://" + SqlAzManStorage.RootDSEPath);
            DirectorySearcher deSearch = new DirectorySearcher(root);
            //Try find exactly
            if (this.adObjectType == ADObjectType.UsersOnly || this.adObjectType == ADObjectType.OneUserOnly)
            {
                deSearch.Filter = String.Format("(&(|(displayName={0})(samaccountname={0})(userprincipalname={0})(objectSid={0}))(&(objectClass=user)(objectCategory=person)))", name);
            }
            else if (this.adObjectType == ADObjectType.UsersAndGroups)
            {
                deSearch.Filter = String.Format("(&(|(displayName={0})(samaccountname={0})(userprincipalname={0})(objectSid={0}))(|(&(objectClass=user)(objectCategory=person))(objectClass=group)))", name);
            }
            
            SearchResultCollection results = deSearch.FindAll();
            ADObject ado = new ADObject();
            try
            {
                //Try find exactly
                if (results.Count == 1)
                {
                    DirectoryEntry de = results[0].GetDirectoryEntry();
                    ado.Name = (string)de.InvokeGet("samaccountname");
                    ado.ADSPath = de.Path;
                    ado.UPN = (string)de.InvokeGet("userPrincipalName");
                    ado.internalSid = new SecurityIdentifier((byte[])de.Properties["objectSid"][0], 0);
                    ado.state = ADObjectState.Resolved;
                    return ado;
                }
                //Then try find with jolly (*)
                if (this.adObjectType == ADObjectType.UsersOnly || this.adObjectType == ADObjectType.OneUserOnly)
                {
                    deSearch.Filter = String.Format("(&(|(displayName=*{0}*)(samaccountname=*{0}*)(userprincipalname=*{0}*))(&(objectClass=user)(objectCategory=person)))", name);
                }
                else if (this.adObjectType == ADObjectType.UsersAndGroups)
                {
                    deSearch.Filter = String.Format("(&(|(displayName=*{0}*)(samaccountname=*{0}*)(userprincipalname=*{0}*))(|(&(objectClass=user)(objectCategory=person))(objectClass=group)))", name);
                }
                results = deSearch.FindAll();
                if (results.Count == 0)
                {
                    //Check for Well Know Sid
                    try
                    {
                        NTAccount nta = new NTAccount(name);
                        SecurityIdentifier sid = (SecurityIdentifier)nta.Translate(typeof(SecurityIdentifier));
                        nta = (NTAccount)sid.Translate(typeof(NTAccount));
                        ado.Name = nta.Value;
                        ado.ADSPath = String.Format("LDAP://<SID={0}>", sid.Value);
                        ado.UPN = nta.Value;
                        ado.internalSid = sid;
                        ado.state = ADObjectState.Resolved;
                        return ado;
                    }
                    catch { }
                    ado.Name = name;
                    ado.state = ADObjectState.NotFound;
                    return ado;
                }
                else
                {
                    List<ADObject> proposedADObjects = new List<ADObject>();
                    foreach (SearchResult sr in results)
                    {
                        DirectoryEntry de = sr.GetDirectoryEntry();
                        ADObject proposal = new ADObject();
                        proposal.Name = (string)de.InvokeGet("samaccountname");
                        proposal.ADSPath = de.Path;
                        proposal.ClassName = de.SchemaClassName;
                        proposal.UPN = (string)de.InvokeGet("userPrincipalName");
                        proposal.internalSid = new SecurityIdentifier((byte[])de.Properties["objectSid"][0], 0);
                        proposedADObjects.Add(proposal);
                        this.Session["proposedADObjects"] = proposedADObjects;
                    }
                    ado.Name = name;
                    ado.state = ADObjectState.Multiple;
                    return ado;
                }
            }
            catch
            {
                return ado;
            }
        }

        protected void btnCheckNames_Click(object sender, EventArgs e)
        {
            try
            {
                this.checkNames();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
