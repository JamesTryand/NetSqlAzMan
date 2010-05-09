using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Security;

namespace NetSqlAzMan_RoleProviderWebTest
{
    public partial class RoleProviderMultiThreadStressTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread t = new Thread(
                    new ThreadStart(
                        delegate()
                        {
                            try
                            {
                                bool isInRole = Roles.IsUserInRole("Andrea");
                                var dbUser = ((NetSqlAzMan.Providers.NetSqlAzManRoleProvider)Roles.Provider).Storage.GetDBUser("Andrea");
                                Debug.WriteLine(String.Format("dbUser: {0}", dbUser.UserName));
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Error: {0}", ex.Message);
                            }
                        }));
                t.Start();
            }
        }
    }
}
