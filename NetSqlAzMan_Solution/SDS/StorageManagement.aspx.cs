using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

public partial class StorageManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCreateStore_Click(object sender, EventArgs e)
    {
        IAzManStorage storage = new SqlAzManStorage(ConfigurationManager.ConnectionStrings["NetSqlAzManStorage"].ConnectionString);
        storage.OpenConnection();
        try
        {
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
            IAzManStore store = storage.CreateStore("Store Created Programmatically", "store description");
            for (int i = 0; i < 10; i++)
            {
                IAzManApplication app = store.CreateApplication("App " + i.ToString(), "application description");
                IAzManItem prevItem = null;
                for (int j = 0; j < 10; j++)
                {
                    IAzManItem item = app.CreateItem("Item " + j.ToString(), "item description", ItemType.Operation);
                    if (prevItem!=null)
                        item.AddMember(prevItem);
                    prevItem = item;
                }
            }
            storage.CommitTransaction();
        }
        catch
        {
            storage.RollBackTransaction();
            throw;
        }
        finally
        {
            storage.CloseConnection();
        }
    }
    protected void btnDeleteStore_Click(object sender, EventArgs e)
    {
        IAzManStorage storage = new SqlAzManStorage(ConfigurationManager.ConnectionStrings["NetSqlAzManStorage"].ConnectionString);
        storage["Store Created Programmatically"].Delete();
    }
    protected void btnPickUpItemsCount_Click(object sender, EventArgs e)
    {
        IAzManStorage storage = new SqlAzManStorage(ConfigurationManager.ConnectionStrings["NetSqlAzManStorage"].ConnectionString);
        this.txtItemsCount.Text = storage["Store Created Programmatically"]["App 1"].GetItems().Length.ToString();
    }
}
