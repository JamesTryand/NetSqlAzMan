using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.CodeDom;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;
using NetSqlAzMan;
using NetSqlAzMan.SnapIn.Forms;
using System.Security.Principal;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;
using CheckAccessNamespace;
using NetSqlAzMan.Cache;
using System.Linq;
using System.Diagnostics;

namespace NetSqlAzMan_WinTest
{
    public partial class Form1 : Form
    {
        private IAzManSid currentOwnerSid = new SqlAzManSID(System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStoreManipulate_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
            IAzManStore store1 = storage.CreateStore("Store di prova 3", "descrizione");

            IAzManStoreGroup storeGroup = store1.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), "Store Group 2", "sg1 des", null, GroupType.Basic);
            storeGroup.CreateStoreGroupMember(SqlAzManSID.NewSqlAzManSid(), WhereDefined.LDAP, true);
            IAzManStoreGroupMember[] storeGroupMembers = storeGroup.GetStoreGroupMembers();
            store1.CreateApplication("Application 3", "description of store 3");
            IAzManApplication app = store1.GetApplication("Application 3");
            IAzManApplicationGroup appGroup = app.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), "Application Group 3", "ag3 des", null, GroupType.Basic);
            appGroup.CreateApplicationGroupMember(SqlAzManSID.NewSqlAzManSid(), WhereDefined.LDAP, false);
            IAzManApplicationGroupMember[] appGroupMembers = appGroup.GetApplicationGroupMembers();
            IAzManItem item1 = app.CreateItem("Responsabile UO", "descrizione", ItemType.Role);
            IAzManItem item11 = app.CreateItem("Modifica", "mod des", ItemType.Task);
            IAzManItem item111 = app.CreateItem("Salva", "salva descr", ItemType.Operation);
            item1.AddMember(item11);
            item11.AddMember(item111);
            IAzManAuthorization auth111 = item111.CreateAuthorization(this.currentOwnerSid, WhereDefined.LDAP, SqlAzManSID.NewSqlAzManSid(), WhereDefined.Store, AuthorizationType.AllowWithDelegation, DateTime.Now, null);
            IAzManAttribute<IAzManAuthorization> attr111 = auth111.CreateAttribute("UO", "SS20S");
            storage.CommitTransaction();
        }

        private void btnItemManipulate_Click(object sender, EventArgs e)
        {
            try
            {
                IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                IAzManStore store = storage.GetStore("Store Stress Test");
                //IAzManStoreGroup storage = store.GetStoreGroup("Store Group 1");
                IAzManItem item = store.GetApplication("Application0").GetItem("Operation0");
                string bizRule =
                                        @"using System;
using System.Security.Principal;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace Prova.BizRules
{
    public sealed class BizRule : IAzManBizRule
    {
        public BizRule()
        { }

        public bool Execute(Hashtable contextParameters, IAzManSid identity, IAzManItem ownerItem, ref AuthorizationType authorizationType)
        {
            //my comments
            //Assign authorizationType to some AuthorizationType value to force CheckAccess result for this item.
            return true;
        }
    }
}
";
//@"Imports System
//Imports System.Security.Principal
//Imports System.IO
//Imports System.Data
//Imports System.Data.SqlClient
//Imports System.Collections
//Imports System.Collections.Specialized
//Imports System.Collections.Generic
//Imports System.Text
//Imports NetSqlAzMan
//Imports NetSqlAzMan.Interfaces
//
//Namespace MyApplication.BizRules
//    Public NotInheritable Class BizRule : Implements IAzManBizRule
//        Public Sub New()
//        End Sub
//
//        Public Overloads Function Execute(ByVal contextParameters As Hashtable, ByVal identity As WindowsIdentity, ByVal ownerItem As IAzManItem) As Boolean _
//            Implements IAzManBizRule.Execute
//            Return True
//        End Function
//    End Class
//End Namespace
//
//";
                item.ReloadBizRule(bizRule, NetSqlAzMan.BizRuleSourceLanguage.CSharp);
                Assembly ass = item.LoadBizRuleAssembly();

                //AuthorizationType authorizationType = storage.CheckAccess(System.Security.Principal.WindowsIdentity.GetCurrent(), DateTime.Now);
                //MessageBox.Show(authorizationType.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ADObject[] res = NetSqlAzMan.SnapIn.DirectoryServices.DirectoryServicesUtils.ADObjectPickerShowDialog(this.Handle, true, true, false);
                if (res != null)
                {
                    foreach (ADObject o in res)
                    {
                        NTAccount acc = new NTAccount(String.IsNullOrEmpty(o.UPN.Trim()) ? o.Name : o.UPN);
                        SecurityIdentifier sid = (SecurityIdentifier)acc.Translate(typeof(SecurityIdentifier));
                        MessageBox.Show(acc.Value + " " + sid.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);
            }
        }

        private void btnACL_Click(object sender, EventArgs e)
        {
            try
            {
                string sUmeshGroups =
                    @"1) SACCAP\Domain Users - (S-1-5-21-1590038678-1968142254-1237804090-513)
2) Everyone - (S-1-1-0)
3) UMESHS-PC1\mqbrkrs - (S-1-5-21-3267003821-1900849679-2171792829-1046)
4) UMESHS-PC1\mqm - (S-1-5-21-3267003821-1900849679-2171792829-1044)
5) BUILTIN\Administrators - (S-1-5-32-544)
6) BUILTIN\Users - (S-1-5-32-545)
7) NT AUTHORITY\INTERACTIVE - (S-1-5-4)
8) NT AUTHORITY\Authenticated Users - (S-1-5-11)
9) LOCAL - (S-1-2-0)
10) SACCAP\UATWEB_etc_RO - (S-1-5-21-1590038678-1968142254-1237804090-52520)
11) SACCAP\apps_bloombergdata_readers_qa - (S-1-5-21-1590038678-1968142254-1237804090-29448)
12) SACCAP\IssueStatus - (S-1-5-21-1590038678-1968142254-1237804090-19477)
13) SACCAP\apps_condensedsi_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-30665)
14) SACCAP\apps_reutersdata_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-32651)
15) SACCAP\Admin_QTOE07 - (S-1-5-21-1590038678-1968142254-1237804090-31948)
16) SACCAP\apps_condensedsi_users_qa - (S-1-5-21-1590038678-1968142254-1237804090-30679)
17) SACCAP\FS1_Trading_StockLoan_MLPMaster_RO - (S-1-5-21-1590038678-1968142254-1237804090-48504)
18) SACCAP\Tech_InternationalEquities - (S-1-5-21-1590038678-1968142254-1237804090-33547)
19) SACCAP\Apps_HR_PhoneDirectory_UAT - (S-1-5-21-1590038678-1968142254-1237804090-52827)
20) SACCAP\Apps_HR_PhoneDirectory - (S-1-5-21-1590038678-1968142254-1237804090-52825)
21) SACCAP\SACFTP_STAGINGA_GENEVA_DEV - (S-1-5-21-1590038678-1968142254-1237804090-30388)
22) SACCAP\Apps_BT_Admins_UAT - (S-1-5-21-1590038678-1968142254-1237804090-65656)
23) SACCAP\apps_pricepublisher_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-28639)
24) SACCAP\Admin_UATRobot - (S-1-5-21-1590038678-1968142254-1237804090-12135)
25) SACCAP\Admin_INTNVDEPTH03 - (S-1-5-21-1590038678-1968142254-1237804090-52586)
26) SACCAP\Admin_QTSTRAT10 - (S-1-5-21-1590038678-1968142254-1237804090-42415)
27) SACCAP\apps_reutersdata_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-32647)
28) SACCAP\apps_condensedsi_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-30671)
29) SACCAP\SharePoint_Payroll_Reader - (S-1-5-21-1590038678-1968142254-1237804090-38160)
30) SACCAP\apps_omgeo_uat_admin - (S-1-5-21-1590038678-1968142254-1237804090-28570)
31) SACCAP\Apps_RS_ProjectReporting_Browser - (S-1-5-21-1590038678-1968142254-1237804090-52753)
32) SACCAP\Apps_SACIMatch_DEV_Admin - (S-1-5-21-1590038678-1968142254-1237804090-29284)
33) SACCAP\apps_intellimatch_accountservice_dev - (S-1-5-21-1590038678-1968142254-1237804090-38152)
34) SACCAP\Apps_LANDESK_RC_Bloomberg_ENTDATA_DEV - (S-1-5-21-1590038678-1968142254-1237804090-67046)
35) SACCAP\UATWEB_intranet_RO - (S-1-5-21-1590038678-1968142254-1237804090-52518)
36) SACCAP\Apps_RS_PLReports_SANG_Browser - (S-1-5-21-1590038678-1968142254-1237804090-28424)
37) SACCAP\FS1_tech_KnowledgeBase_SQLJobs - (S-1-5-21-1590038678-1968142254-1237804090-42303)
38) SACCAP\Web_Admins - (S-1-5-21-1590038678-1968142254-1237804090-2143)
39) SACCAP\XLS_FxFwdCalculator - (S-1-5-21-1590038678-1968142254-1237804090-20128)
40) SACCAP\DTCC_Users - (S-1-5-21-1590038678-1968142254-1237804090-18660)
41) SACCAP\apps_boweb_dev_admin - (S-1-5-21-1590038678-1968142254-1237804090-29910)
42) SACCAP\InstrinsicBrokerVoteInfo - (S-1-5-21-1590038678-1968142254-1237804090-19387)
43) SACCAP\SACFTP_Murex - (S-1-5-21-1590038678-1968142254-1237804090-14859)
44) SACCAP\apps_bloombergdata_readers - (S-1-5-21-1590038678-1968142254-1237804090-29444)
45) SACCAP\Web_SACGELog - (S-1-5-21-1590038678-1968142254-1237804090-21720)
46) SACCAP\FS1_tech_qa_TestDocumentation_RO - (S-1-5-21-1590038678-1968142254-1237804090-36521)
47) SACCAP\apps_condensedsi_users_uat - (S-1-5-21-1590038678-1968142254-1237804090-30678)
48) SACCAP\apps_outgoingfeed_reader_qa - (S-1-5-21-1590038678-1968142254-1237804090-36163)
49) SACCAP\apps_boweb_prod_readonly - (S-1-5-21-1590038678-1968142254-1237804090-29905)
50) SACCAP\UATWEB_Framework_RO - (S-1-5-21-1590038678-1968142254-1237804090-52519)
51) SACCAP\Apps_HR_DeviceInventory_UAT - (S-1-5-21-1590038678-1968142254-1237804090-52831)
52) SACCAP\db_SACIMatch_DEV_admin - (S-1-5-21-1590038678-1968142254-1237804090-28479)
53) SACCAP\apps_omgeo_prod_techsupport - (S-1-5-21-1590038678-1968142254-1237804090-29495)
54) SACCAP\Admin_NVROUTER04 - (S-1-5-21-1590038678-1968142254-1237804090-52148)
55) SACCAP\apps_reutersdata_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-32652)
56) SACCAP\Admin_QTSTRAT09 - (S-1-5-21-1590038678-1968142254-1237804090-42414)
57) SACCAP\Citrix_RemoteDesktop - (S-1-5-21-1590038678-1968142254-1237804090-23726)
58) SACCAP\apps_panogenevarec_reader_dev - (S-1-5-21-1590038678-1968142254-1237804090-43335)
59) SACCAP\Admin_UATMURAPP03 - (S-1-5-21-1590038678-1968142254-1237804090-18242)
60) SACCAP\SACFTP_STAGINGA_GENEVA_UAT - (S-1-5-21-1590038678-1968142254-1237804090-30391)
61) SACCAP\apps_omgeo_prod_readonly - (S-1-5-21-1590038678-1968142254-1237804090-28569)
62) SACCAP\Admin_QTOE10 - (S-1-5-21-1590038678-1968142254-1237804090-44243)
63) SACCAP\apps_panorama_linkserver_uat - (S-1-5-21-1590038678-1968142254-1237804090-52634)
64) SACCAP\WEB_HR_JobPostings_RO - (S-1-5-21-1590038678-1968142254-1237804090-25185)
65) SACCAP\SharePoint_SAC Global Investors_Reader - (S-1-5-21-1590038678-1968142254-1237804090-21440)
66) SACCAP\Admin_NVOE02 - (S-1-5-21-1590038678-1968142254-1237804090-26375)
67) SACCAP\HardToBorrow - (S-1-5-21-1590038678-1968142254-1237804090-19476)
68) SACCAP\apps_panorama_client - (S-1-5-21-1590038678-1968142254-1237804090-52624)
69) SACCAP\MessageStatsWeb - (S-1-5-21-1590038678-1968142254-1237804090-19705)
70) SACCAP\apps_panogenevarec_reader_prod - (S-1-5-21-1590038678-1968142254-1237804090-43338)
71) SACCAP\SACFS1_Murex - (S-1-5-21-1590038678-1968142254-1237804090-18249)
72) SACCAP\apps_panorama_linkserver - (S-1-5-21-1590038678-1968142254-1237804090-52636)
73) SACCAP\apps_pricepublisher_readers_qa - (S-1-5-21-1590038678-1968142254-1237804090-28638)
74) SACCAP\RobotAdmin - (S-1-5-21-1590038678-1968142254-1237804090-7674)
75) SACCAP\Apps_RS_Logs - (S-1-5-21-1590038678-1968142254-1237804090-24035)
76) SACCAP\Apps_SACIMatch_QA_Admin - (S-1-5-21-1590038678-1968142254-1237804090-29285)
77) SACCAP\apps_panogenevarec_reader_uat - (S-1-5-21-1590038678-1968142254-1237804090-43337)
78) SACCAP\Apps_RS_Lachesis_Browser - (S-1-5-21-1590038678-1968142254-1237804090-27660)
79) SACCAP\apps_reutersdata_developers - (S-1-5-21-1590038678-1968142254-1237804090-32654)
80) SACCAP\Admin_QTOE12 - (S-1-5-21-1590038678-1968142254-1237804090-44245)
81) SACCAP\Admin_NVROUTER03 - (S-1-5-21-1590038678-1968142254-1237804090-52149)
82) SACCAP\Admin_SACRISKAPP01 - (S-1-5-21-1590038678-1968142254-1237804090-29879)
83) SACCAP\Production Changes - (S-1-5-21-1590038678-1968142254-1237804090-14785)
84) SACCAP\apps_bloombergdata_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-29449)
85) SACCAP\Apps_RS_QAReports_Browser - (S-1-5-21-1590038678-1968142254-1237804090-36364)
86) SACCAP\Pricing - (S-1-5-21-1590038678-1968142254-1237804090-7548)
87) SACCAP\apps_EDSSync_tech_development - (S-1-5-21-1590038678-1968142254-1237804090-65909)
88) SACCAP\Web_Intranet_Technology_DataDrivenReport - (S-1-5-21-1590038678-1968142254-1237804090-32079)
89) SACCAP\Admin_QTROUTER04 - (S-1-5-21-1590038678-1968142254-1237804090-52150)
90) SACCAP\Admin_JIRA01 - (S-1-5-21-1590038678-1968142254-1237804090-28301)
91) SACCAP\apps_panorama_client_denied - (S-1-5-21-1590038678-1968142254-1237804090-66724)
92) SACCAP\SACFTP_STAGINGA_GENEVA_PROD - (S-1-5-21-1590038678-1968142254-1237804090-30389)
93) SACCAP\apps_panogenevarec_reader_qa - (S-1-5-21-1590038678-1968142254-1237804090-43336)
94) SACCAP\apps_outgoingfeed_reader_prod - (S-1-5-21-1590038678-1968142254-1237804090-36165)
95) SACCAP\SACFTP_STAGINGA_INTELLIMATCH_QA2_RW - (S-1-5-21-1590038678-1968142254-1237804090-30359)
96) SACCAP\SACFTP_STAGINGA_GENEVA_QA2 - (S-1-5-21-1590038678-1968142254-1237804090-30390)
97) SACCAP\FS1_tech_EnterpriseServices_Shared - (S-1-5-21-1590038678-1968142254-1237804090-47631)
98) SACCAP\Tech_BackOffice_StrategicDevelopment - (S-1-5-21-1590038678-1968142254-1237804090-29283)
99) SACCAP\XLS_BrokerEvents - (S-1-5-21-1590038678-1968142254-1237804090-20782)
100) SACCAP\Admin_QTOE08 - (S-1-5-21-1590038678-1968142254-1237804090-31949)
101) SACCAP\apps_panorama_client_uat - (S-1-5-21-1590038678-1968142254-1237804090-52626)
102) SACCAP\apps_Trading_TradingTools_Portfolio_Holdings - (S-1-5-21-1590038678-1968142254-1237804090-48323)
103) SACCAP\Admin_UATMURAOO04 - (S-1-5-21-1590038678-1968142254-1237804090-26252)
104) SACCAP\FS1_BrokerContactSpreadsheets_ReadOnly - (S-1-5-21-1590038678-1968142254-1237804090-33436)
105) SACCAP\sacinstalls_Software_SAC_EnterpriseServices_UAT - (S-1-5-21-1590038678-1968142254-1237804090-50594)
106) SACCAP\SACBOAPP01_Logfiles - (S-1-5-21-1590038678-1968142254-1237804090-30660)
107) SACCAP\apps_outgoingfeed_reader_dev - (S-1-5-21-1590038678-1968142254-1237804090-36162)
108) SACCAP\Programmers - (S-1-5-21-1590038678-1968142254-1237804090-1310)
109) SACCAP\sacinstalls_Software_SAC_EnterpriseServices_Dev - (S-1-5-21-1590038678-1968142254-1237804090-50592)
110) SACCAP\Citrix_Tech - (S-1-5-21-1590038678-1968142254-1237804090-28147)
111) SACCAP\apps_condensedsi_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30669)
112) SACCAP\Admin_INTNVDEPTH04 - (S-1-5-21-1590038678-1968142254-1237804090-52587)
113) SACCAP\Admin_QTOE11 - (S-1-5-21-1590038678-1968142254-1237804090-44244)
114) SACCAP\apps_CyberArk_Users - (S-1-5-21-1590038678-1968142254-1237804090-64819)
115) SACCAP\Apps_RS_JIRAReports_Browser - (S-1-5-21-1590038678-1968142254-1237804090-38992)
116) SACCAP\SharePoint_BusinessContinuity_Reader - (S-1-5-21-1590038678-1968142254-1237804090-53156)
117) SACCAP\FS1_Murex_UAT3 - (S-1-5-21-1590038678-1968142254-1237804090-14854)
118) SACCAP\Admin_NVROUTER02 - (S-1-5-21-1590038678-1968142254-1237804090-63323)
119) SACCAP\Apps_SACIMatch_PROD_Admin - (S-1-5-21-1590038678-1968142254-1237804090-29286)
120) SACCAP\Apps_BT_Users_UAT - (S-1-5-21-1590038678-1968142254-1237804090-65659)
121) SACCAP\apps_condensedsi_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-30670)
122) SACCAP\Apps_CorpAct_Reader - (S-1-5-21-1590038678-1968142254-1237804090-25064)
123) SACCAP\Admin_QTOE09 - (S-1-5-21-1590038678-1968142254-1237804090-44242)
124) SACCAP\Admin_QTROUTER03 - (S-1-5-21-1590038678-1968142254-1237804090-52151)
125) SACCAP\apps_pricepublisher_readers_dev - (S-1-5-21-1590038678-1968142254-1237804090-28642)
126) SACCAP\WEB_MGMTTAB - (S-1-5-21-1590038678-1968142254-1237804090-10662)
127) SACCAP\Admin_NVROUTER01 - (S-1-5-21-1590038678-1968142254-1237804090-63322)
128) SACCAP\Devsql05_ssis - (S-1-5-21-1590038678-1968142254-1237804090-52848)
129) SACCAP\Murex_MXExtract - (S-1-5-21-1590038678-1968142254-1237804090-14993)
130) SACCAP\apps_bloombergdata_readers_uat - (S-1-5-21-1590038678-1968142254-1237804090-33716)
131) SACCAP\Apps_CorpActQA_Reader - (S-1-5-21-1590038678-1968142254-1237804090-25068)
132) SACCAP\apps_pricepublisher_admins - (S-1-5-21-1590038678-1968142254-1237804090-28631)
133) SACCAP\apps_condensedsi_users - (S-1-5-21-1590038678-1968142254-1237804090-30680)
134) SACCAP\Admin_DRFTP02 - (S-1-5-21-1590038678-1968142254-1237804090-35837)
135) SACCAP\Admin_NVOE01 - (S-1-5-21-1590038678-1968142254-1237804090-26374)
136) SACCAP\WEB_PanRef_BrokerList (RO) - (S-1-5-21-1590038678-1968142254-1237804090-29703)
137) SACCAP\Admin_UATWEB01 - (S-1-5-21-1590038678-1968142254-1237804090-11920)
138) SACCAP\FS1_Apps_ResearchAnalystToolkits_RO - (S-1-5-21-1590038678-1968142254-1237804090-47638)
139) SACCAP\Admin_UATWEB2 - (S-1-5-21-1590038678-1968142254-1237804090-22021)
140) SACCAP\Apps_RS_AppSupport_IntelliMatch_Browser - (S-1-5-21-1590038678-1968142254-1237804090-28515)
141) SACCAP\SACHKDS01_groups_hongkong_Public_RO - (S-1-5-21-1590038678-1968142254-1237804090-52953)
142) SACCAP\Tech_Dept - (S-1-5-21-1590038678-1968142254-1237804090-1405)
143) SACCAP\apps_bloombergdata_readers_dev - (S-1-5-21-1590038678-1968142254-1237804090-29452)
144) SACCAP\Citrix_MSOffice - (S-1-5-21-1590038678-1968142254-1237804090-25160)
145) SACCAP\Admin_UATBBG1 - (S-1-5-21-1590038678-1968142254-1237804090-28016)
146) SACCAP\Admin_DEVRISKAPP01 - (S-1-5-21-1590038678-1968142254-1237804090-29310)
147) SACCAP\Issue Tracker - (S-1-5-21-1590038678-1968142254-1237804090-14786)
148) SACCAP\MassSecurityLoad - (S-1-5-21-1590038678-1968142254-1237804090-20164)
149) SACCAP\apps_bloombergdata_admins_uat - (S-1-5-21-1590038678-1968142254-1237804090-33717)
150) SACCAP\apps_reutersdata_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-32653)
151) SACCAP\Tech_EnterpriseDataServices - (S-1-5-21-1590038678-1968142254-1237804090-30664)
152) SACCAP\Apps_EnterpriseLogging_Readers - (S-1-5-21-1590038678-1968142254-1237804090-29815)
153) SACCAP\apps_panorama_client_readonly - (S-1-5-21-1590038678-1968142254-1237804090-66720)
154) SACCAP\Tech_XLS_TopFirmPositions - (S-1-5-21-1590038678-1968142254-1237804090-20776)
155) SACCAP\apps_boweb_qa_poweruser - (S-1-5-21-1590038678-1968142254-1237804090-29908)
156) SACCAP\Admin_QA1PAN - (S-1-5-21-1590038678-1968142254-1237804090-10723)
157) SACCAP\Admin_DEVWEB01 - (S-1-5-21-1590038678-1968142254-1237804090-14990)
158) SACCAP\sacinstalls_Software_SAC_EnterpriseServices_QA - (S-1-5-21-1590038678-1968142254-1237804090-50593)
159) SACCAP\PortfolioHoldingByCatalyst - (S-1-5-21-1590038678-1968142254-1237804090-20086)
160) SACCAP\apps_condensedsi_users_dev - (S-1-5-21-1590038678-1968142254-1237804090-30677)
161) SACCAP\SACBOAPP02_Logfiles - (S-1-5-21-1590038678-1968142254-1237804090-30661)
162) SACCAP\apps_outgoingfeed_reader_uat - (S-1-5-21-1590038678-1968142254-1237804090-36164)
163) SACCAP\BrokerEvents - (S-1-5-21-1590038678-1968142254-1237804090-20814)
164) SACCAP\apps_reutersdata_supportusers_dev - (S-1-5-21-1590038678-1968142254-1237804090-32655)
165) SACCAP\Apps_BT_Operators_UAT - (S-1-5-21-1590038678-1968142254-1237804090-65660)
166) SACCAP\FS1_Murex_Dev - (S-1-5-21-1590038678-1968142254-1237804090-26399)
167) SACCAP\QA_TouchPad_Users - (S-1-5-21-1590038678-1968142254-1237804090-63669)
168) SACCAP\Apps_GELog_User - (S-1-5-21-1590038678-1968142254-1237804090-26277)
169) SACCAP\Apps_HR_DeviceInventory - (S-1-5-21-1590038678-1968142254-1237804090-52829)
170) SACCAP\FS1_OP'S_INTERNATIONAL_RW - (S-1-5-21-1590038678-1968142254-1237804090-42385)
171) SACCAP\SAC LP - All Employees - (S-1-5-21-1590038678-1968142254-1237804090-18199)
172) SACCAP\apps_borat_users - (S-1-5-21-1590038678-1968142254-1237804090-43069)
173) SACCAP\CS-SACDATASVCS1_Inetpub_RO - (S-1-5-21-1590038678-1968142254-1237804090-47655)
174) SACCAP\db_RiskTech_prod_poweruser - (S-1-5-21-1590038678-1968142254-1237804090-29401)
175) SACCAP\Apps_RS_Backoffice_Intellimatch_Browser - (S-1-5-21-1590038678-1968142254-1237804090-28526)
176) SACCAP\Apps_RS_ManagementFailsReports_Browser - (S-1-5-21-1590038678-1968142254-1237804090-42383)
177) SACCAP\Admin_DEVRISKAPP02 - (S-1-5-21-1590038678-1968142254-1237804090-35668)
178) SACCAP\Apps_FO_FirmPairOffPositions - (S-1-5-21-1590038678-1968142254-1237804090-63445)
179) SACCAP\apps_performanceprojection_risk_users_uat - (S-1-5-21-1590038678-1968142254-1237804090-63592)
180) SACCAP\Apps_Omgeo_Admins - (S-1-5-21-1590038678-1968142254-1237804090-29825)
181) SACCAP\apps_backofficereporting_developers_prod - (S-1-5-21-1590038678-1968142254-1237804090-65912)
182) SACCAP\Admin_DEVSECBIZ01 - (S-1-5-21-1590038678-1968142254-1237804090-50492)
183) SACCAP\apps_intellimatch_accountmaintenance_dev - (S-1-5-21-1590038678-1968142254-1237804090-32604)
184) SACCAP\apps_datamart_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-30783)
185) SACCAP\Apps_BT_Users_DEV - (S-1-5-21-1590038678-1968142254-1237804090-66742)
186) SACCAP\apps_backofficereporting_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-65915)
187) SACCAP\Program - (S-1-5-21-1590038678-1968142254-1237804090-17804)
188) SACCAP\SAC Adv Emp - Stamford - (S-1-5-21-1590038678-1968142254-1237804090-17828)
189) SACCAP\apps_intellimatch_accountmaintenance_qa - (S-1-5-21-1590038678-1968142254-1237804090-32606)
190) SACCAP\apps_datamart_publishers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30789)
191) SACCAP\Apps_SPP_PublisherQA - (S-1-5-21-1590038678-1968142254-1237804090-53085)
192) SACCAP\Admin_UATSECAPP01 - (S-1-5-21-1590038678-1968142254-1237804090-50494)
193) SACCAP\db_RiskTech_prod_readonly - (S-1-5-21-1590038678-1968142254-1237804090-29400)
194) SACCAP\apps_intellimatch_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-30636)
195) SACCAP\Apps_RS_Backoffice_Browser - (S-1-5-21-1590038678-1968142254-1237804090-28242)
196) SACCAP\Web_Apps_SACApplicationAccess_DEV - (S-1-5-21-1590038678-1968142254-1237804090-55188)
197) SACCAP\Citrix_Murex - (S-1-5-21-1590038678-1968142254-1237804090-30124)
198) SACCAP\apps_datamart_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-30782)
199) SACCAP\apps_datamart_datareaders_dev - (S-1-5-21-1590038678-1968142254-1237804090-30801)
200) SACCAP\SellsideVoiceMail - (S-1-5-21-1590038678-1968142254-1237804090-16626)
201) SACCAP\Murex_Users - (S-1-5-21-1590038678-1968142254-1237804090-12771)
202) SACCAP\apps_intellimatch_securitymaintenance_qa - (S-1-5-21-1590038678-1968142254-1237804090-32602)
203) SACCAP\Year End - Infrastructure Employees - (S-1-5-21-1590038678-1968142254-1237804090-19573)
204) SACCAP\Apps_Murex_QA_EOD_MD_IMPORT(RO) - (S-1-5-21-1590038678-1968142254-1237804090-27714)
205) SACCAP\SACFTP_STAGINGA_INTELLIMATCH_UAT_RW - (S-1-5-21-1590038678-1968142254-1237804090-30361)
206) SACCAP\apps_intellimatch_powerusers_dev - (S-1-5-21-1590038678-1968142254-1237804090-32596)
207) SACCAP\Apps_BT_SSO_Admins_DEV - (S-1-5-21-1590038678-1968142254-1237804090-66740)
208) SACCAP\SACFTP_STAGINGA_INTELLIMATCH_DEV_RW - (S-1-5-21-1590038678-1968142254-1237804090-30357)
209) SACCAP\Apps_DataDrivenReport - (S-1-5-21-1590038678-1968142254-1237804090-29981)
210) SACCAP\Tech_BloombergDataLicense - (S-1-5-21-1590038678-1968142254-1237804090-44421)
211) SACCAP\apps_datamart_datawriters_dev - (S-1-5-21-1590038678-1968142254-1237804090-30797)
212) SACCAP\apps_borat_users_uat - (S-1-5-21-1590038678-1968142254-1237804090-43071)
213) SACCAP\Apps_DeviceManager_Readers - (S-1-5-21-1590038678-1968142254-1237804090-42564)
214) SACCAP\Apps_RS_EnterpriseServices_Browser - (S-1-5-21-1590038678-1968142254-1237804090-48226)
215) SACCAP\FS1_tech_QA_WeeklyReleaseCalendar_RO - (S-1-5-21-1590038678-1968142254-1237804090-36368)
216) SACCAP\SAC Event Log Viewers - (S-1-5-21-1590038678-1968142254-1237804090-63380)
217) SACCAP\Apps_RS_UATWEB_Browser - (S-1-5-21-1590038678-1968142254-1237804090-27945)
218) SACCAP\XLOGONSCRIPT_MAP_X_SACFIAPP01_MurexProdFS-Client X - (S-1-5-21-1590038678-1968142254-1237804090-29598)
219) SACCAP\WEB_SACGeneva_CustodianMapping - (S-1-5-21-1590038678-1968142254-1237804090-47304)
220) SACCAP\FS1_Apps_MurexProd_RO - (S-1-5-21-1590038678-1968142254-1237804090-28513)
221) SACCAP\db_BrokerScorecard_prod_poweruser - (S-1-5-21-1590038678-1968142254-1237804090-32691)
222) SACCAP\Apps_RS_DEVWEB_Browser - (S-1-5-21-1590038678-1968142254-1237804090-26633)
223) SACCAP\Web_Apps_SACApplicationAccess_QA - (S-1-5-21-1590038678-1968142254-1237804090-55189)
224) SACCAP\apps_intellimatch_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-30630)
225) SACCAP\Tech - Enterprise Data Services - (S-1-5-21-1590038678-1968142254-1237804090-19588)
226) SACCAP\Admin_SACNVSQL04 - (S-1-5-21-1590038678-1968142254-1237804090-35694)
227) SACCAP\Apps_BT_Operators_DEV - (S-1-5-21-1590038678-1968142254-1237804090-66743)
228) SACCAP\Apps_FO_BorrowRateReview - (S-1-5-21-1590038678-1968142254-1237804090-55912)
229) SACCAP\apps_borat_developers - (S-1-5-21-1590038678-1968142254-1237804090-43061)
230) SACCAP\FS1_Op's_USEQUITIES_FAILS_RW - (S-1-5-21-1590038678-1968142254-1237804090-42386)
231) SACCAP\Citrix_ALLCitrixGroups - (S-1-5-21-1590038678-1968142254-1237804090-25178)
232) SACCAP\FS1_tech_QA_EnvironmentOutages_RO - (S-1-5-21-1590038678-1968142254-1237804090-36367)
233) SACCAP\Web_Apps_SACApplicationAccess_UAT - (S-1-5-21-1590038678-1968142254-1237804090-55190)
234) SACCAP\WEB_DealLog - (S-1-5-21-1590038678-1968142254-1237804090-48299)
235) SACCAP\apps_borat_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-43063)
236) SACCAP\WEB_Panorama_SecurityEditor - (S-1-5-21-1590038678-1968142254-1237804090-24793)
237) SACCAP\SharePoint_Intrinsic_Reader - (S-1-5-21-1590038678-1968142254-1237804090-21427)
238) SACCAP\Tech_XLS_IntrinsicPortfolioHoldings - (S-1-5-21-1590038678-1968142254-1237804090-19544)
239) SACCAP\apps_datamart_authors_openlink_dev - (S-1-5-21-1590038678-1968142254-1237804090-30793)
240) SACCAP\apps_datamart_admins_dev - (S-1-5-21-1590038678-1968142254-1237804090-30777)
241) SACCAP\db_BBGSEC_prod_readonly - (S-1-5-21-1590038678-1968142254-1237804090-43624)
242) SACCAP\Apps_RS_CorpActions_Browser - (S-1-5-21-1590038678-1968142254-1237804090-36581)
243) SACCAP\apps_intellimatch_securitymaintenance_dev - (S-1-5-21-1590038678-1968142254-1237804090-32600)
244) SACCAP\Admin_SACNVSQL03 - (S-1-5-21-1590038678-1968142254-1237804090-35693)
245) SACCAP\Apps_LANDesk_RC_ALLGROUPS - (S-1-5-21-1590038678-1968142254-1237804090-30027)
246) SACCAP\Technology - (S-1-5-21-1590038678-1968142254-1237804090-16661)
247) SACCAP\Tech - Central Services Data - (S-1-5-21-1590038678-1968142254-1237804090-18630)
248) SACCAP\apps_backofficereporting_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-65914)
249) SACCAP\SharePoint_Technology_Security_Reader - (S-1-5-21-1590038678-1968142254-1237804090-38716)
250) SACCAP\Apps_EnterpriseConfiguration_Readers - (S-1-5-21-1590038678-1968142254-1237804090-29365)
251) SACCAP\apps_intellimatch_accountmaintenance_uat - (S-1-5-21-1590038678-1968142254-1237804090-32605)
252) SACCAP\apps_performanceprojection_risk_users_qa - (S-1-5-21-1590038678-1968142254-1237804090-63591)
253) SACCAP\apps_performanceprojection_risk_users - (S-1-5-21-1590038678-1968142254-1237804090-63594)
254) SACCAP\FS1_Ops_International Operations - (S-1-5-21-1590038678-1968142254-1237804090-23841)
255) SACCAP\Apps_FO_StockLoanTool - (S-1-5-21-1590038678-1968142254-1237804090-54970)
256) SACCAP\apps_datamart_supportusers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30785)
257) SACCAP\apps_condensedsi_supportusers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30673)
258) SACCAP\AzMan_Reader_PROD_EntDataSvcs - (S-1-5-21-1590038678-1968142254-1237804090-65861)
259) SACCAP\sacvss01_vssdb_PostImplementation - (S-1-5-21-1590038678-1968142254-1237804090-65631)
260) SACCAP\apps_borat_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-43064)
261) SACCAP\apps_intellimatch_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30634)
262) SACCAP\apps_intellimatch_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-30635)
263) SACCAP\apps_intellimatch_supportusers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30638)
264) SACCAP\Apps_iSACUsers - (S-1-5-21-1590038678-1968142254-1237804090-65430)
265) SACCAP\apps_backofficereporting_developers_uat - (S-1-5-21-1590038678-1968142254-1237804090-65913)
266) SACCAP\Apps_BT_Admins_DEV - (S-1-5-21-1590038678-1968142254-1237804090-66739)
267) SACCAP\apps_datamart_developers_dev - (S-1-5-21-1590038678-1968142254-1237804090-30781)
268) SACCAP\Admin_DEVSECAPP01 - (S-1-5-21-1590038678-1968142254-1237804090-50491)
269) SACCAP\Apps_RS_DEVWEB_Publisher - (S-1-5-21-1590038678-1968142254-1237804090-26634)
270) SACCAP\apps_performanceprojection_risk_users_dev - (S-1-5-21-1590038678-1968142254-1237804090-63590)
271) SACCAP\apps_borat_developers_qa - (S-1-5-21-1590038678-1968142254-1237804090-43062)
272) SACCAP\Stamford Office - Te - (S-1-5-21-1590038678-1968142254-1237804090-17774)
273) SACCAP\db_VarianceSwap_prod_poweruser - (S-1-5-21-1590038678-1968142254-1237804090-29577)
274) SACCAP\apps_intellimatch_securitymaintenance_uat - (S-1-5-21-1590038678-1968142254-1237804090-32601)
275) SACCAP\FS1_tech_QA_Admin_ReleaseManagementFlowMatrix_RO - (S-1-5-21-1590038678-1968142254-1237804090-36365)
276) SACCAP\apps_datamart_developers - (S-1-5-21-1590038678-1968142254-1237804090-30784)
277) SACCAP\Tech_XLS_GrossmanHoldingsReport - (S-1-5-21-1590038678-1968142254-1237804090-19512)";

                List<string> umeshGroupsList = new List<string>();
                foreach (var line in sUmeshGroups.Split('\r'))
                {
                    string sid = line.Substring(line.LastIndexOf('(') + 1);
                    sid = sid.Substring(0, sid.Length - 1).Replace("\n","");
                    umeshGroupsList.Add(sid);

                }
                
                //string[] users = DirectoryServicesUtils.GetAllDomainUsers();


                //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                //IAzManDBUser dbUser1 = storage.GetDBUser(new SqlAzManSID(this.GetBytesFromInt32(1), true));
                //IAzManDBUser dbUser2 = storage.GetDBUser(new SqlAzManSID(this.GetBytesFromInt32(2), true));
                //AuthorizationType auth1 = storage.CheckAccess("Eidos", "DB Persone", "Accesso", dbUser1, DateTime.Now, false);
                //AuthorizationType auth2 = storage.CheckAccess("Eidos", "DB Persone", "Accesso", dbUser1, DateTime.Now, false);
                //string cs = "data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI";
                string cs = "data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI";
                var ctx = new[] { new KeyValuePair<string, object>("Value1", "111"), new KeyValuePair<string, object>("Value2", "222") };
                IAzManStorage storage = new SqlAzManStorage(cs);
                //DateTime dt = new DateTime(2009, 05, 01);
                //AuthorizationType authz = storage.CheckAccess("Eidos", "DB Persone", "Super utente senza dati retributivi", WindowsIdentity.GetCurrent(), dt, false);

                //authz = upcTest.CheckAccess("Super utente senza dati retributivi", dt);
                //MessageBox.Show(authz.ToString());
                DateTime t1, t2;
                ////return;
                t1 = DateTime.Now;
                StorageCache sc = new StorageCache(cs);
                sc.BuildStorageCache();
                //t2 = DateTime.Now;
                ////MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //t1 = DateTime.Now;
                //UserPermissionCache uupc = new UserPermissionCache(storage, "Eidos", "DB Persone", WindowsIdentity.GetCurrent(), true, true);
                t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //return;
                //t1 = DateTime.Now;
                //UserPermissionCache upcTest = new UserPermissionCache(storage, "Eidos", "DB Persone", WindowsIdentity.GetCurrent(), true, true, ctx);
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    upcTest.CheckAccess("Accesso", DateTime.Now);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                string ssid = WindowsIdentity.GetCurrent().GetUserBinarySSid();
                string[] gsid = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
                ssid = "S-1-5-21-1590038678-1968142254-1237804090-38243";
                gsid = umeshGroupsList.ToArray();

                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    sc.CheckAccess("Eidos", "DB Persone", "Gestore", ssid, gsid, DateTime.Now, false);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                sr.CacheServiceClient csc = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
                csc.Open();
                t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //var aauu = csc.CheckAccessForWindowsUsersWithoutAttributesRetrieve("ZZEntDataSvcs", "CommissionFeeTax", "Editor", ssid, gsid, DateTime.Now, false, null);
                var aauu = sc.CheckAccess("ZZEntDataSvcs", "CommissionFeeTax", "Editor", ssid, gsid, DateTime.Now, false, null);
                    //csc.GetAuthorizedItemsForWindowsUsers("Eidos", "DB Persone", ssid, gsid, DateTime.Now, null);
                //}
                t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                csc.Close();


                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    storage.CheckAccess("Eidos", "DB Persone", "Gestore", WindowsIdentity.GetCurrent(), DateTime.Now, false);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //return;

                //DateTime dt = DateTime.Now;
                //foreach (string user in users)
                //{
                //    WindowsIdentity win = new WindowsIdentity(user);
                //    sc.CheckAccess("Eidos", "DB Persone", "Gestore", win.GetUserBinarySSid(), win.GetGroupsBinarySSid(), DateTime.Now, false);
                //}
                //TimeSpan ts = DateTime.Now.Subtract(dt);
                //var seconds = ts.TotalSeconds;

                //
                //upcTest.CheckAccess("Accesso", DateTime.Now);

                List<KeyValuePair<string, string>> attributes1;
                List<KeyValuePair<string, string>> attributes2;
                List<KeyValuePair<string, string>> attributes3;

                foreach (var store in storage.Stores)
                {
                    foreach (var application in store.Value.Applications)
                    {
                        UserPermissionCache upc = new UserPermissionCache(storage, store.Value.Name, application.Value.Name, WindowsIdentity.GetCurrent(), true, false, ctx);
                        foreach (var item in application.Value.Items)
                        {
                            AuthorizationType auth1 = sc.CheckAccess(store.Value.Name, application.Value.Name, item.Value.Name, WindowsIdentity.GetCurrent().GetUserBinarySSid(), WindowsIdentity.GetCurrent().GetGroupsBinarySSid(), DateTime.Now, false, out attributes1, ctx);
                            AuthorizationType auth2 = storage.CheckAccess(store.Value.Name, application.Value.Name, item.Value.Name, WindowsIdentity.GetCurrent(), DateTime.Now, false, out attributes2, ctx);
                            AuthorizationType auth3 = upc.CheckAccess(item.Value.Name, DateTime.Now, out attributes3);
                            this.detectedDifferences(auth1, attributes1, auth2, attributes2);
                            this.detectedDifferences(auth2, attributes2, auth3, attributes3);
                            this.detectedDifferences(auth1, attributes1, auth3, attributes3);

                        }
                    }
                }
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectedDifferences(
            AuthorizationType auth1,
            List<KeyValuePair<string, string>> attrs1,
            AuthorizationType auth2,
            List<KeyValuePair<string, string>> attrs2)
        {
            if (auth1 != auth2)
                throw new Exception("Auth1 <> Auth2");
            if (attrs1.Count == attrs2.Count)
            {
                var t = (from t1 in attrs1
                         join t2 in attrs2 on new { K = t1.Key, V = t1.Value } equals new { K = t2.Key, V = t2.Value }
                         select t1).Count();
                if (t != attrs2.Count)
                {
                    throw new Exception("attrs1 <> attrs2");
                }
            }
            else
            {
                throw new Exception("attrs1 <> attrs2");
            
            }
        }

        private byte[] GetBytesFromInt32(int n)
        {
            byte[] result = BitConverter.GetBytes(n);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }

        private void callBack(IAsyncResult ar)
        {
            MessageBox.Show("callback " + (string)ar.AsyncState);
        }

        private void btnEventHandling_Click(object sender, EventArgs e)
        {
            
            //SqlAzManENS.ApplicationUpdated += new ApplicationUpdatedDelegate(SqlAzManENS_ApplicationUpdated);
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Store Stress Test");
            IAzManApplication application = store.GetApplication("Application0");
            application.ApplicationUpdated += new NetSqlAzMan.ENS.ApplicationUpdatedDelegate(application_Updated);
            application.Update("New Description");
            MessageBox.Show("Descrizione dell'Applicazione modificata !");
        }

        void SqlAzManENS_ApplicationUpdated(IAzManApplication application, string oldDescription)
        {
            MessageBox.Show("SqlAzManENS_ApplicationUpdated Application " + application.Name + " ha cambiato descrizione da " + oldDescription + " a " + application.Description);
        }

        void application_Updated(IAzManApplication application, string oldDescription)
        {
            MessageBox.Show("application_Updated Application " + application.Name + " ha cambiato descrizione da " + oldDescription + " a " + application.Description);
        }

        private void btnImportFromAzMan_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    frmImportFromAzMan frm = new frmImportFromAzMan();
            //    IAzManStorage storage = new SqlAzManStorage("data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //    frm.storage = storage;
            //    DialogResult dr = frm.ShowDialog(this);
            //    if (dr == DialogResult.OK)
            //    {
            //        MessageBox.Show("Done !");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Import From AzMan failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //IAzManStorage storage = new SqlAzManStorage("data source=eidos-egcc1\\sql2005;Initial Catalog=NetSqlAzManStore;Integrated Security = SSPI;");
            ////storage.CreateStore("Prova", "").CreateApplication("App 1", "").CreateItem("Task 1","");
            //IAzManItem storage = storage["Prova"]["App 1"]["Task 1"];

            //frmItemProperties frm = new frmItemProperties();
            //frm.Text += " - " + storage.Name;
            //frm.store = storage.Application;
            //frm.storage = storage;
            //DialogResult dr = frm.ShowDialog(this);
            ////MessageBox.Show(dr.ToString());
            
        }

        private void btnTestImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FileName = "NetSqlAzMan.xml";
            openFileDialog.Filter = "Xml files|*.xml|All files|*.*";
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = "Import from ...";
            DialogResult dr = openFileDialog.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                frmImportOptions frm = new frmImportOptions();
                IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                IAzManStore store = storage.GetStore("Eidos");
                frm.importIntoObject = store;
                frm.fileName = openFileDialog.FileName;
                frm.ShowDialog();
            }
        }

        private void btnTestDomain_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(DirectoryServicesUtils.rootDsePath);
        }

        /// <summary>
        /// Flags that indicate the scope types described by this structure. You can combine multiple scope types if all specified scopes use the same settings. 
        /// </summary>
        internal class DSOP_SCOPE_TYPE_FLAGS
        {
            internal const uint DSOP_SCOPE_TYPE_TARGET_COMPUTER = 0x00000001;
            internal const uint DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN = 0x00000002;
            internal const uint DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN = 0x00000004;
            internal const uint DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN = 0x00000008;
            internal const uint DSOP_SCOPE_TYPE_GLOBAL_CATALOG = 0x00000010;
            internal const uint DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN = 0x00000020;
            internal const uint DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN = 0x00000040;
            internal const uint DSOP_SCOPE_TYPE_WORKGROUP = 0x00000080;
            internal const uint DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE = 0x00000100;
            internal const uint DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE = 0x00000200;
        }

        /// <summary>
        /// Flags that indicate the format used to return ADsPath for objects selected from this scope. The flScope sid can also indicate the initial scope displayed in the Look in drop-down list. 
        /// </summary>
        internal class DSOP_SCOPE_INIT_INFO_FLAGS
        {
            internal const uint DSOP_SCOPE_FLAG_STARTING_SCOPE = 0x00000001;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT = 0x00000002;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP = 0x00000004;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_GC = 0x00000008;
            internal const uint DSOP_SCOPE_FLAG_WANT_SID_PATH = 0x00000010;
            internal const uint DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH = 0x00000020;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS = 0x00000040;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS = 0x00000080;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS = 0x00000100;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS = 0x00000200;
        }

        /// <summary>
        /// Filter flags to use for an up-level scope, regardless of whether it is a mixed or native mode domain. 
        /// </summary>
        internal class DSOP_FILTER_FLAGS_FLAGS
        {
            internal const uint DSOP_FILTER_INCLUDE_ADVANCED_VIEW = 0x00000001;
            internal const uint DSOP_FILTER_USERS = 0x00000002;
            internal const uint DSOP_FILTER_BUILTIN_GROUPS = 0x00000004;
            internal const uint DSOP_FILTER_WELL_KNOWN_PRINCIPALS = 0x00000008;
            internal const uint DSOP_FILTER_UNIVERSAL_GROUPS_DL = 0x00000010;
            internal const uint DSOP_FILTER_UNIVERSAL_GROUPS_SE = 0x00000020;
            internal const uint DSOP_FILTER_GLOBAL_GROUPS_DL = 0x00000040;
            internal const uint DSOP_FILTER_GLOBAL_GROUPS_SE = 0x00000080;
            internal const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL = 0x00000100;
            internal const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE = 0x00000200;
            internal const uint DSOP_FILTER_CONTACTS = 0x00000400;
            internal const uint DSOP_FILTER_COMPUTERS = 0x00000800;
        }

        /// <summary>
        /// Contains the filter flags to use for down-level scopes
        /// </summary>
        internal class DSOP_DOWNLEVEL_FLAGS
        {
            internal const uint DSOP_DOWNLEVEL_FILTER_USERS = 0x80000001;
            internal const uint DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS = 0x80000002;
            internal const uint DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS = 0x80000004;
            internal const uint DSOP_DOWNLEVEL_FILTER_COMPUTERS = 0x80000008;
            internal const uint DSOP_DOWNLEVEL_FILTER_WORLD = 0x80000010;
            internal const uint DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER = 0x80000020;
            internal const uint DSOP_DOWNLEVEL_FILTER_ANONYMOUS = 0x80000040;
            internal const uint DSOP_DOWNLEVEL_FILTER_BATCH = 0x80000080;
            internal const uint DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER = 0x80000100;
            internal const uint DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP = 0x80000200;
            internal const uint DSOP_DOWNLEVEL_FILTER_DIALUP = 0x80000400;
            internal const uint DSOP_DOWNLEVEL_FILTER_INTERACTIVE = 0x80000800;
            internal const uint DSOP_DOWNLEVEL_FILTER_NETWORK = 0x80001000;
            internal const uint DSOP_DOWNLEVEL_FILTER_SERVICE = 0x80002000;
            internal const uint DSOP_DOWNLEVEL_FILTER_SYSTEM = 0x80004000;
            internal const uint DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS = 0x80008000;
            internal const uint DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER = 0x80010000;
            internal const uint DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS = 0x80020000;
            internal const uint DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE = 0x80040000;
            internal const uint DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE = 0x80080000;
            internal const uint DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON = 0x80100000;
        }

        private void btnTestADShowDialog2_Click(object sender, EventArgs e)
        {
            //bool showLocalUsersAndGroups = true;

            //// Initialize 1st search scope			

            //uint flType =
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG |
            //    //DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_WORKGROUP;

            //if (showLocalUsersAndGroups)
            //    flType = flType | DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER;

            //uint flScope =
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH |
            //    //DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_SID_PATH |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT; // Starting !?;
            

            //uint flBothModes =
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS | 
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_WELL_KNOWN_PRINCIPALS; // +1 = advanced view, Check MSDN for the available options

            //uint flDownlevel =
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ANONYMOUS |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_BATCH |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_DIALUP |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_INTERACTIVE |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SYSTEM |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS;
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_WORLD;

            //ADObjectPickerClass cadObjectPicker = new ADObjectPickerClass();
            //cadObjectPicker.ScopeTypeFlags = flType;
            //cadObjectPicker.ScopeFlags = flScope;
            //cadObjectPicker.UplevelFilterFlags_Both = flBothModes;
            //cadObjectPicker.DownLevelFilterFlags = flDownlevel;
            //cadObjectPicker.InvokeDialog(this.Handle.ToInt32());
            //ADObjectColl authorizationType = (ADObjectColl)cadObjectPicker.ADObjectsColl;
            //for (uint j = 1; j<=authorizationType.Count; j++)
            //{
            //    int i = (int)j;
            //    ADObjectInfo info = (ADObjectInfo)authorizationType.Item(i);
            //    ADObject ad = new ADObject();
            //    ad.ADSPath = info.ADPath;
            //    ad.ClassName = info.Class;
            //    ad.Name = info.Name;
            //    ad.UPN = info.UPN;
            //    NTAccount Account = new NTAccount("Administrator");
            //    SecurityIdentifier Sid = (SecurityIdentifier)
            //      Account.Translate(typeof(SecurityIdentifier));
               
            //    string s = String.Format("Name: {0} Class: {1} ADPath: {2} Sid: {3}", info.Name, info.Class, info.ADPath, Sid.Value);
            //    MessageBox.Show(s);
            }

        private void btnCheckAccessTemplate_Click(object sender, EventArgs e)
        {
            string cs = "data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;";
            My_Application.Security.CheckAccessHelper helper = new My_Application.Security.CheckAccessHelper(cs, WindowsIdentity.GetCurrent());
            helper.OpenConnection();
            bool result = helper.CheckAccess(My_Application.Security.CheckAccessHelper.Operation.Op_1);
            helper.CloseConnection();
            //Use result for your biz
            if (result == true)
            {
                //Allow or AllowWithDelegation
            }
            else
            { 
                //Deny or Neutral
            }
        }

        private void btnGenerateCheckAccessHelper_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManApplication application = storage["Store Stress Test"]["Application0"];
            CodeCompileUnit ccu = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateCheckAccessHelperClass("MyApplication.NetSqlHelper", "CheckAccessHelper", true, true, application, NetSqlAzMan.CodeDom.Language.CSharp);
            string code = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu, NetSqlAzMan.CodeDom.Language.CSharp);
            this.textBox1.Text = code;
        }

        private void btnDBGetUsers_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = String.Empty;
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage["My Store"];

            IAzManDBUser dbu = store.GetDBUser(store.GetStoreGroup("sg1").SID);
            //IAzManDBUser[] dbUsers = store. app.GetDBUsers();
            //foreach (IAzManDBUser dbU in dbUsers)
            //{

            //    this.textBox1.Text += String.Format("Sid: {0} - Name: {1}\r\n", dbU.CustomSid.StringValue, dbU.UserName);
            //}
            ////MessageBox.Show(app.GetDBUser("andrea").CustomSid.StringValue);
            //WindowsIdentity win = WindowsIdentity.GetCurrent();
            ////app.GetItem("ResponsabileUO").CreateAuthorization(new SqlAzManSID(win.User), WhereDefined.LDAP, app.GetDBUser("a.ferendeles").CustomSid, WhereDefined.Database, AuthorizationType.AllowWithDelegation, null, null);
            ////AuthorizationType auth = storage.CheckAccess("NetSqlAzManStore", "ApplicazioneSia", "Delega", storage.GetDBUser("a.ferendeles"), DateTime.Now, true);
            ////MessageBox.Show(auth.ToString());
        }

        private void btnCheckAccessTest_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManApplication app = storage["Test"]["Test"];
            app.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), "g1", String.Empty, String.Empty, GroupType.Basic);
            app.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), "g2", String.Empty, String.Empty, GroupType.Basic);
            IAzManApplicationGroup g1 = app.GetApplicationGroup("g1");
            g1.CreateApplicationGroupMember(new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.Local, true);
            bool isMember = g1.IsInGroup(WindowsIdentity.GetCurrent()); //result is true


            return;
            //frmCheckAccessTest frm = new frmCheckAccessTest();
            //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //IAzManStore store = storage.GetStore("Italferr");
            //frm.application = store.GetApplication("CartaDeiServizi");
            //frm.ShowDialog();
        }

        private void btnIsAMemberOf_Click(object sender, EventArgs e)
        {
        }

        private void btnIHV_Click(object sender, EventArgs e)
        {
            //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //IAzManStore store = storage.GetStore("Store Stress Test");
            //NetSqlAzMan.SnapIn.Printing.ptItemAuthorizations doc = new NetSqlAzMan.SnapIn.Printing.ptItemAuthorizations();
            //doc.Applications = new IAzManApplication[] { 
            //    store.GetApplication("Application0")
            //    //,store.GetApplication("Application1"),
            //    //store.GetApplication("Application2"),
            //    //store.GetApplication("Application3"),
            //    //store.GetApplication("Application4")
            //};
            //frmPrint frm = new frmPrint();
            //frm.Document = doc;
            //frm.ShowDialog(this);

            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Eidos");
            NetSqlAzMan.SnapIn.Printing.ptEffectivePermissions doc = new NetSqlAzMan.SnapIn.Printing.ptEffectivePermissions();
            doc.Applications = new IAzManApplication[] { 
                store.GetApplication("DB Persone")
                //,store.GetApplication("Application1"),
                //store.GetApplication("Application2"),
                //store.GetApplication("Application3"),
                //store.GetApplication("Application4")
            };
            frmPrint frm = new frmPrint();
            frm.Document = doc;
            frm.ShowDialog(this);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Store Stress Test");
            foreach (KeyValuePair<string, bool> kvp in store.GetManagers())
            {
                MessageBox.Show(String.Format("Manager: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in store.GetUsers())
            {
                MessageBox.Show(String.Format("User: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in store.GetReaders())
            {
                MessageBox.Show(String.Format("Reader: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            IAzManApplication app = store["Application1"];
            foreach (KeyValuePair<string, bool> kvp in app.GetManagers())
            {
                MessageBox.Show(String.Format("Manager: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in app.GetUsers())
            {
                MessageBox.Show(String.Format("User: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in app.GetReaders())
            {
                MessageBox.Show(String.Format("Reader: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
        }

        private void btnCacheTest_Click(object sender, EventArgs e)
        {
            DateTime dtStart = DateTime.Now;
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            NetSqlAzMan.Cache.UserPermissionCache userPermissionCache = new NetSqlAzMan.Cache.UserPermissionCache(storage, "Italferr", "CartaDeiServizi", WindowsIdentity.GetCurrent(), true, true);
            AuthorizationType auth = userPermissionCache.CheckAccess("My Operation", DateTime.Now);
            MessageBox.Show(((TimeSpan)(DateTime.Now-dtStart)).TotalMilliseconds.ToString());
        }

        private void btnCheckStoreAccess_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            foreach (IAzManStore store in storage.GetStores())
            {
                //MessageBox.Show(String.Format("Store: {0} - Access: {1}", store.Name, store.CheckStoreAccess(WindowsIdentity.GetCurrent(), DateTime.Now)));
                store.CheckStoreAccess(WindowsIdentity.GetCurrent(), DateTime.Now);
                foreach (IAzManApplication application in store.GetApplications())
                {
                    //MessageBox.Show(String.Format("Application: {0} - Access: {1}", application.Name, application.CheckApplicationAccess(WindowsIdentity.GetCurrent(), DateTime.Now)));
                    application.CheckApplicationAccess(WindowsIdentity.GetCurrent(), DateTime.Now);
                }
            }
        }

        private void btnCacheServiceTest_Click(object sender, EventArgs e)
        {
            List<TimeSpan> times = new List<TimeSpan>();
            List<System.Threading.Thread> threads = new List<System.Threading.Thread>();
            int max = 100;
            for (int i = 0; i < max; i++)
            {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(
                () =>
                {
                    int k = i;
                    Random r = new Random();
                    //System.Threading.Thread.Sleep(r.Next(300));
                    DateTime now = DateTime.Now;
                    sr.CacheServiceClient c = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
                    c.Open();
                    KeyValuePair<string, string>[] attributes;
                    string user = WindowsIdentity.GetCurrent().GetUserBinarySSid();
                    string[] groups = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
                    AuthorizationType auth = c.CheckAccessForWindowsUsersWithAttributesRetrieve(out attributes, "Italferr", "CartaDeiServizi", "Visualizza Dettagli Richiesta", user, groups, DateTime.Now, false, null);
                    c.Close();
                    TimeSpan ts = DateTime.Now.Subtract(now);
                    if (auth != AuthorizationType.Allow)
                    {
                        throw new Exception("Error");
                    }
                    times.Add(ts);
                }));
                threads.Add(t);
                t.Start();
            }
            foreach (System.Threading.Thread t in threads)
            {
                t.Join();
            }
            this.textBox1.Text = String.Empty;
            int cc = 0;
            foreach (TimeSpan ts in times)
            {
                this.textBox1.Text += String.Format("{0}) {1}\r\n",++cc, ts);
            }
            MessageBox.Show("Done");
            
            //DateTime now = DateTime.Now;
            //NetSqlAzMan.Cache.StorageCache sc = new StorageCache("data source=.;initial catalog=NetSqlAzManStorage;user id=sa;password=");
            //sc.BuildStorageCache("Italferr", "CartaDeiServizi");
            //WindowsIdentity wid = WindowsIdentity.GetCurrent();
            //string user = wid.GetUserBinarySSid(); //using NetSqlAzMan.Cache needed
            //string[] groups = wid.GetGroupsBinarySSid(); //using NetSqlAzMan.Cache needed
            //AuthorizationType au = sc.CheckAccess("Italferr", "CartaDeiServizi", "Approvatore Sistemi Informativi", user, groups, DateTime.Now, false);
            //MessageBox.Show(au.ToString());
            //return;
            
            //int max = 100;


            //sr.CacheServiceClient client = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
            //for (int i = 0; i < max; i++)
            //{
            //    KeyValuePair<string, string>[] attributes;
            //    auth = client.CheckAccessForWindowsUsersWithAttributesRetrieve(out attributes, "Italferr", "CartaDeiServizi", "Visualizza Dettagli Richiesta", user, groups, DateTime.Now, false, null);
            //}
            //client.Close();
            //MessageBox.Show(auth.ToString());
            //TimeSpan ts = DateTime.Now.Subtract(now);
            //MessageBox.Show(String.Format("Total: {0} - Single: {1}", ts.TotalMilliseconds, ts.TotalMilliseconds / (double)max));

            //NetSqlAzMan.Cache.StorageCache sc = new NetSqlAzMan.Cache.StorageCache("data source=.;initial catalog=NetSqlAzManStorage;user id=sa;password=");
            //sc.BuildStorageCache();
            //WindowsIdentity wid = WindowsIdentity.GetCurrent();
            //List<KeyValuePair<string, string>> attributes;
            ////AuthorizationType auth = sc.CheckAccess("ItalferR", "CartadeiServizi", "Visualizza dettagli Richiesta ", wid.User, wid.Groups, DateTime.Now, false, out attributes);
            //DateTime now = DateTime.Now;
            //int max = 10000;
            //string user = wid.GetUserBinarySSid();
            //string[] groups = wid.GetGroupsBinarySSid();
            //for (int i = 0; i < max; i++)
            //{
            //    AuthorizationType auth = sc.CheckAccess("Italferr", "CartaDeiServizi", "visualizza dettagli richiesta", user, groups, DateTime.Now, false, out attributes);
            //}
            //TimeSpan ts = DateTime.Now.Subtract(now);
            //MessageBox.Show(String.Format("Total: {0} - Single: {1}",ts.TotalMilliseconds, ts.TotalMilliseconds/(double)max));
        }

        /// <summary>
        /// Create an Authorization Delegate
        /// </summary>
        private void AddDBUserToRole(string dbUserName, string roleName)
        {
            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=password";
            //Create an instance of SqlAzManStorage class
            using (IAzManStorage storage = new SqlAzManStorage(sqlConnectionString))
            {
                storage.OpenConnection();
                IAzManStore mystore = storage.GetStore("My Store"); //or storage["My Store"]
                IAzManApplication myapp = mystore.GetApplication("My Application");
                IAzManItem myRole = myapp.GetItem(roleName);
                //Retrieve DB user identity
                IAzManDBUser dbUser = storage.GetDBUser(dbUserName);
                //Add DB "My Db User" to "My Role" role.
                IAzManAuthorization auth = myRole.CreateAuthorization(new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.LDAP, dbUser.CustomSid, WhereDefined.Database, AuthorizationType.Allow, null, null);
                //Optional: add authorization attribute
                //auth.CreateAttribute("attribute key", "attribute value");
                storage.CloseConnection();
            }
        }

        private void btnSerializeUserPermissionCache_Click(object sender, EventArgs e)
        {
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=";
            StorageCache sc = new StorageCache(sqlConnectionString);
            sc.BuildStorageCache();

            BinaryFormatter xSer = new BinaryFormatter();
            FileStream fs = File.Create("c:\\ser.xml");
            xSer.Serialize(fs, sc);
            fs.Close();

            fs = File.Open("c:\\ser.xml", FileMode.Open);
            StorageCache sc2 = (StorageCache)xSer.Deserialize(fs);
            fs.Close();
            AuthorizationType result = sc2.CheckAccess("Italferr", "CartaDeiServizi", "Visualizza Richiesta RAC", WindowsIdentity.GetCurrent().GetUserBinarySSid(), DateTime.Now, false);
            MessageBox.Show(result.ToString());

            //string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=";
            //IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            //UserPermissionCache upc = new UserPermissionCache(storage, "Italferr", "CartaDeiServizi", WindowsIdentity.GetCurrent(), true, true);

            //BinaryFormatter xSer = new BinaryFormatter();
            //FileStream fs = File.Create("c:\\ser.xml");
            //xSer.Serialize(fs, upc);
            //fs.Close();

            //fs = File.Open("c:\\ser.xml", FileMode.Open);
            //UserPermissionCache upc2 = (UserPermissionCache)xSer.Deserialize(fs);
            //fs.Close();
            //AuthorizationType result = upc2.CheckAccess("Visualizza Richiesta RAC", DateTime.Now);
            //MessageBox.Show(result.ToString());

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;user id=sa;password=");
            IAzManApplication app = storage.GetStore("Eidos").GetApplication("Feedback");

            frmExportOptions frm = new frmExportOptions();

            frmExport frmwait = new frmExport();
            frmwait.ShowDialog(null, "c:\\netsqlazman.xml", new IAzManExport[] { app }, true, false, true, app.Store.Storage);
        }

        private void btnCreateItemsFromAFolder_Click(object sender, EventArgs e)
        {
            using (IAzManStorage storage = new SqlAzManStorage("Data Source=(local);Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI;"))
            {
                storage.OpenConnection();
                storage.BeginTransaction();
                var a = storage["Eidos"]["DB Persone"]["Gestore"].GetMembers();

                

            }
            this.CreateItemsFromAFolder(
                "Data Source=(local);Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI;",
                "My Store",
                "My Application",
                @"D:\Documenti\EIDOS\ICP\EIDOS.ApplicazioniAziendali\EIDOS.ApplicazioniAziendali.DBPersone.Web",
                "*.aspx",
                ItemType.Task);
        }

        public void CreateItemsFromAFolder(
            string storageConnectionString, 
            string storeName,
            string applicationName,
            string folderPath, 
            string searchPattern,
            ItemType itemType)
        {
            using (IAzManStorage storage = new SqlAzManStorage(storageConnectionString))
            {
                storage.OpenConnection();
                storage.BeginTransaction();
                try
                {
                    IAzManApplication app = storage.GetStore(storeName).GetApplication(applicationName);
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    foreach (FileInfo fi in di.GetFiles(searchPattern))
                    {
                        //Use some recursive function to get subfolder files
                        app.CreateItem(fi.Name, String.Empty, itemType);
                    }
                    storage.CommitTransaction();
                }
                catch
                {
                    storage.RollBackTransaction();
                }
            }
        }

        private void btnStorageCacheAuthorizedItems_Click(object sender, EventArgs e)
        {
            string cs = "data source=.;Initial Catalog=NetSqlAzManStorage;user id=testuser;password=;";
            var ctx = new[] { new KeyValuePair<string, object>("Value1", "111"), new KeyValuePair<string, object>("Value2", "222") };
            IAzManStorage storage = new SqlAzManStorage(cs);
            IAzManApplication app = storage["Eidos"]["DB Persone"];
            var res = app.GetItem("Gestore");
            MessageBox.Show(res.Members.Count.ToString());
            //string ssid = WindowsIdentity.GetCurrent().GetUserBinarySSid();
            //string[] gsid = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
            //DateTime t1, t2;

            //StorageCache sc = new StorageCache(cs);
            //sc.BuildStorageCache("Eidos");
            //t1 = DateTime.Now;
            //for (int i = 0; i < 1000; i++)
            //{
            //    AuthorizedItem[] result = sc.GetAuthorizedItems("Eidos", "DB Persone", ssid, gsid, DateTime.Now, ctx);
            //}
            //t2 = DateTime.Now;
            //double ms = t2.Subtract(t1).TotalMilliseconds;
            //MessageBox.Show(String.Format("Done in {0} ms", ms));
        }

        /// <summary>
        /// Handles the Click event of the btnCreateALotOfItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCreateALotOfItems_Click(object sender, EventArgs e)
        {
            string cs = "data source=.\\sql2005;initial catalog=NetSqlAzManStorage;Integrated Security=SSPI;";
            IAzManStorage storage = new SqlAzManStorage(cs);
            storage.OpenConnection();
            storage.BeginTransaction();
            IAzManStore store = storage.CreateStore("Test2", String.Empty);
            IAzManApplication app = store.CreateApplication("Test", String.Empty);
            storage.ENS.AuthorizationCreated+= new AuthorizationCreatedDelegate(ens_AuthorizationCreated);
            
            //Create 1 MLN Items
            for (int r = 0; r < 100; r++)
            {
                IAzManItem role = app.CreateItem("Role " + r.ToString(), "", ItemType.Role);

                IAzManAuthorization auth = role.CreateAuthorization(new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.Local,
                    new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.Local, AuthorizationType.Allow, null, null);
                Debug.WriteLine("Role "+ r.ToString()); 
                auth.CreateAttribute("key", "value");
                for (int t = 0; t < 100; t++)
                {
                    IAzManItem task = app.CreateItem("Task " + t.ToString() + " of Role " + r.ToString(), "", ItemType.Task);
                    role.AddMember(task);
                    for (int o = 0; o < 100; o++)
                    {
                        IAzManItem op = app.CreateItem("Operation " + o.ToString() + " of Task " + t.ToString() + " of Role " + r.ToString() , "", ItemType.Operation);
                        task.AddMember(op);
                    }
                }   
            }
            storage.CommitTransaction();
            storage.CloseConnection();
        }

        void ens_AuthorizationCreated(IAzManItem item, IAzManAuthorization authorizationCreated)
        {
            MessageBox.Show("created");
        }
    }
}