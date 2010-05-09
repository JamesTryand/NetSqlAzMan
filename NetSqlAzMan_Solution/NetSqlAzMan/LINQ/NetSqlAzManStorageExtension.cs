using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace NetSqlAzMan.LINQ
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NetSqlAzManStorageDataContext
    {
        private static Dictionary<KeyValuePair<string, string>, int?> dbUsersCheckSum = new Dictionary<KeyValuePair<string, string>, int?>();
        /// <summary>
        /// Gets the DB users ex.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="dBUserSid">The d B user sid.</param>
        /// <param name="dBUserName">Name of the d B user.</param>
        /// <returns></returns>
        public DataTable GetDBUsersEx(string storeName, string applicationName, byte[] dBUserSid, string dBUserName)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.netsqlazman_GetDBUsers(@StoreName, @ApplicationName, @DBUserSID, @DBUserName)", (SqlConnection)this.Connection);
            SqlParameter pStoreName = new SqlParameter("@StoreName", SqlDbType.NVarChar, 255);
            SqlParameter pApplicationName = new SqlParameter("@ApplicationName", SqlDbType.NVarChar, 255);
            SqlParameter pDBUserSID = new SqlParameter("@DBUserSID", SqlDbType.VarBinary, 85);
            SqlParameter pDBUserName = new SqlParameter("@DBUserName", SqlDbType.NVarChar, 255);
            pStoreName.Value = !String.IsNullOrEmpty(storeName) ? (object)storeName : DBNull.Value;
            pApplicationName.Value = !String.IsNullOrEmpty(applicationName) ? (object)applicationName : DBNull.Value;
            pDBUserSID.Value = dBUserSid != null ? (object)dBUserSid : DBNull.Value;
            pDBUserName.Value = !String.IsNullOrEmpty(dBUserName) ? (object)dBUserName : DBNull.Value;
            da.SelectCommand.Parameters.Add(pStoreName);
            da.SelectCommand.Parameters.Add(pApplicationName);
            da.SelectCommand.Parameters.Add(pDBUserSID);
            da.SelectCommand.Parameters.Add(pDBUserName);
            DataTable result = new DataTable("DBUsers");
            da.SelectCommand.Transaction = this.Transaction as SqlTransaction;
            da.Fill(result);
            return result;
        }

        partial void OnCreated()
        {
            this.ObjectTrackingEnabled = true;
        }

    }
}
