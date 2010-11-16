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
            for (int i = 0; i < 1; i++)
            {
                //ThreadPool.QueueUserWorkItem(
                //    new WaitCallback(
                //////Thread t = new Thread(
                //////    new ThreadStart(
                //        delegate(object o)
                //        {
                            try
                            {
                                NetSqlAzMan.Providers.NetSqlAzManRoleProvider provider = ((NetSqlAzMan.Providers.NetSqlAzManRoleProvider)Roles.Provider);
                                var dbUser = provider.GetApplication().GetDBUser("Arianna");
                                string randomRoleName = String.Format("Random Role {0}", Guid.NewGuid().ToString());
                                provider.CreateRole(randomRoleName);
                                provider.AddUsersToRoles(new[] { "EIDOS-NBAFR\\Andrea" }, new[] { randomRoleName });
                                provider.InvalidateCache(true);
                                bool isInRole = provider.IsUserInRole("EIDOS-NBAFR\\Andrea", randomRoleName); //Roles.IsUserInRole(randomRoleName);

                                Debug.WriteLine(String.Format("isInRole: {0}", isInRole.ToString()));
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Error: {0}", ex.Message);
                            }
                        //}));
                //t.Start();
            }
        }
    }
}
