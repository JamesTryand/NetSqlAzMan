using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;

namespace NetSqlAzMan.SnapIn
{
    internal class ImageIndexes
    {
        internal const int NetSqlAzManImgIdx = 3;
        internal const int StoresImgIdx = 1;
        internal const int StoreImgIdx = 6;
        internal const int StoreGroupsImgIdx = 1;
        internal const int StoreGroupBasicImgIdx = 7;
        internal const int StoreGroupLDAPImgIdx = 9;
        internal const int ApplicationImgIdx = 0;
        internal const int WindowsUserImgIdx = 10;
        internal const int WindowsGroupImgIdx = 8;
        internal const int ApplicationGroupsImgIdx = 1;
        internal const int ApplicationGroupBasicImgIdx = 7;
        internal const int ApplicationGroupLDAPImgIdx = 9;
        internal const int ItemsImgIdx = 1;
        internal const int ItemImgIdx = 4;
        internal const int AuthorizationsImgIdx = 1;
        internal const int AuthorizationImgIdx = 2;
        internal const int AuthorizationAttributeImgIdx = 10;
        internal const int SidNotFoundImgIdx = 5;
        internal const int RoleImgIdx = 12;
        internal const int TaskImgIdx = 13;
        internal const int OperationImgIdx = 14;
        internal const int HierarchyImgIdx = 15;
        internal const int mnuConnectionSettingsImgIdx = 16;
        internal const int DatabaseUserImgIdx = 17;

        internal static void LoadImages(MMC.SnapInImageList smallImages, MMC.SnapInImageList largeImages)
        {
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Application_16x16); //0
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Folder_16x16); //1
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.ItemAuthorization_16x16); //2
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.NetSqlAzMan_16x16); //3
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Operation_16x16); //4
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.SIDNotFound_16x16); //5
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Store_16x16); //6
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.StoreApplicationGroup_16x16); //7
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsBasicGroup_16x16); //8
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsQueryLDAPGroup_16x16); //9
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsUser_16x16); //10
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.AuthorizationAttribute_16x16); //11
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Role_16x16); //12
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Task_16x16); //13
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Operation_16x16); //14
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Hierarchy_16x16); //15
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Options_16x16); //16
            smallImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.DBUser_16x16); //17

            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Application_32x32); //0
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Folder_32x32); //1
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.ItemAuthorization_32x32); //2
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.NetSqlAzMan_32x32); //3
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Operation_32x32); //4
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.SIDNotFound_32x32); //5
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Store_32x32); //6
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.StoreApplicationGroup_32x32); //7
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsBasicGroup_32x32); //8
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsQueryLDAPGroup_32x32); //9
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.WindowsUser_32x32); //10
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.AuthorizationAttribute_32x32); //11
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Role_32x32); //12
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Task_32x32); //13
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Operation_32x32); //14
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Hierarchy_32x32); //15
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.Options_32x32); //16
            largeImages.Add(NetSqlAzMan.SnapIn.Properties.Resources.DBUser_32x32); //17
        }
    }
}
