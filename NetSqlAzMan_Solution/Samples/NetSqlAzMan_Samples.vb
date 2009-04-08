'  -------------------
'  * NetSqlAzMan Samples
'  * -------------------
'  * Andrea Ferendeles
'  * aferende@hotmail.com
'  * http://netsqlazman.codeplex.com
'  * 
'  * 1) Install first NetSqlAzMan.msi
'  * 2) Create a New Sql database: "NetSqlAzManStorage"
'  * 3) Execute Sql Script (installation folder) on Me sql database
'  * 4) Launch NetSqlAzMan Console: start - run - "netsqlazman.msc"
'  * 5) Create a Store called "My Store"
'  * 6) Under "My Store", create an Application called "My Application"
'  * 7) Under "My Application" - Item Definitions, create an Operation called "My Operation"
'  * 8) Under Item Authorization - "My Operation", assign yourself (Windows Account) a set an "Allow" permission
'  * 
'  */



Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Principal
'*************************************************
'TODO: Add a reference to NetSqlAzMan.dll assembly
'*************************************************
Imports NetSqlAzMan
Imports NetSqlAzMan.Interfaces
'*************************************************

Namespace NetSqlAzMan_VBNET_Samples
    ' <summary>
    ' NetSqlAzMan VB.NET Samples
    ' </summary>
    Partial Public Class NetSqlAzMan_Samples
        ' <summary>
        ' Check Access from your Application [FOR Windows Users ONLY].
        ' </summary>
        Private Sub CheckAccessPermissionsForWindowsUsers(ByVal userIdentity As WindowsIdentity, ByVal useCache As Boolean)
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers

            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            'To Pass current user identity:
            'WindowsIdentity.GetCurrent() -> for Windows Applications 
            'this.Request.LogonUserIdentity -> for ASP.NET Applications
            Dim attributes As System.Collections.Generic.List(Of KeyValuePair(Of String, String)) = Nothing
            Dim auth As AuthorizationType
            If (useCache) Then
                'Build the cache only one time per session/application/user
                Dim cache As New NetSqlAzMan.Cache.UserPermissionCache(storage, "My Store", "My Application", userIdentity, True, True)
                'Then Check Access
                auth = cache.CheckAccess("My Operation", DateTime.Now, attributes)
            Else
                auth = storage.CheckAccess("My Store", "My Application", "My Operation", userIdentity, DateTime.Now, True, attributes)
            End If

            Select Case auth
                Case AuthorizationType.AllowWithDelegation
                    'Yes, I can ... and I can delegate
                    Exit Sub
                Case AuthorizationType.Allow
                    'Yes, I can
                    Exit Sub
                Case AuthorizationType.Neutral
                Case AuthorizationType.Deny
                    'No, I cannot
                    Exit Sub
            End Select
            'Do something with attributes
        End Sub

        ' <summary>
        ' Check Access from your Application [FOR Windows Users ONLY].
        ' </summary>
        Private Sub CheckAccessPermissionsForDBUsers(ByVal dbUserName As String)
            ' REMBER: 
            ' Modify dbo.GetDBUsers Table-Function to customize DB User list.
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers
            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            'Retrieve DB User identity from dbo.GetDBUsers Table-Function
            Dim dbUser As IAzManDBUser = storage.GetDBUser(dbUserName)
            Dim auth As AuthorizationType = storage.CheckAccess("My Store", "My Application", "My Operation", dbUser, DateTime.Now, True)
            Select Case auth
                Case AuthorizationType.AllowWithDelegation
                    'Yes, I can ... and I can delegate
                    Exit Sub
                Case AuthorizationType.Allow
                    'Yes, I can
                    Exit Sub
                Case AuthorizationType.Neutral
                Case AuthorizationType.Deny
                    'No, I cannot
                    Exit Sub
            End Select
        End Sub

        ' <summary>
        ' Navigate through NetSqlAzMan DOM (Document Object Model)
        ' </summary>
        Private Sub NetSqlAzMan_DOM_Sample()
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers

            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            Dim mystore As IAzManStore = storage.GetStore("My Store")  'or storage["My Store"]
            Dim myapp As IAzManApplication = mystore.GetApplication("My Application")
            Dim myop As IAzManItem = myapp.GetItem("My Operation")
            Dim auths() As IAzManAuthorization = myop.GetAuthorizations()
            Dim auth As IAzManAuthorization
            For Each auth In auths
                Dim attrs() As IAzManAttribute(Of IAzManAuthorization) = auth.GetAttributes()
                Dim attr As IAzManAttribute(Of IAzManAuthorization)
                For Each attr In attrs
                    Dim attrKey As String = attr.Key
                    Dim attrValue As String = attr.Value
                    'do something
                Next
            Next
        End Sub

        ' <summary>
        ' Create an Authorization Delegate
        ' </summary>
        Private Sub CreateDelegate()
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Users

            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            Dim mystore As IAzManStore = storage.GetStore("My Store")  'or storage["My Store"]
            Dim myapp As IAzManApplication = mystore.GetApplication("My Application")
            Dim myop As IAzManItem = myapp.GetItem("My Operation")
            'Retrieve current user identity (delegating user)
            Dim userIdentity As WindowsIdentity = WindowsIdentity.GetCurrent()  'for Windows Applications 
            'WindowsIdentity userIdentity = this.Request.LogonUserIdentity; //for ASP.NET Applications
            'Retrieve delegate user Login
            Dim delegateUserLogin As NTAccount = New NTAccount("DOMAIN", "delegateuseraccount")
            'Retrieve delegate user SID
            Dim delegateSID As SecurityIdentifier = CType(delegateUserLogin.Translate(GetType(SecurityIdentifier)), SecurityIdentifier)
            Dim delegateNetSqlAzManSID As IAzManSid = New SqlAzManSID(delegateSID)
            'Estabilish delegate authorization (only Allow or Deny)
            Dim delegateAuthorization As RestrictedAuthorizationType = RestrictedAuthorizationType.Allow
            'Create delegate
            Dim del As IAzManAuthorization = myop.CreateDelegateAuthorization(userIdentity, delegateNetSqlAzManSID, delegateAuthorization, New DateTime(2006, 1, 1, 0, 0, 0), New DateTime(2006, 12, 31, 23, 59, 59))
            'Set custom Attribute on Authorization Delegate
            del.CreateAttribute("MyCustomInfoKey", "MyCustomInfoValue")
        End Sub

        ' <summary>
        ' Remove Authorization Delegate
        ' </summary>
        Private Sub RemoveDelegate()
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Users

            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            Dim mystore As IAzManStore = storage.GetStore("My Store")  'or storage["My Store"]
            Dim myapp As IAzManApplication = mystore.GetApplication("My Application")
            Dim myop As IAzManItem = myapp.GetItem("My Operation")
            'Retrieve current user identity (delegating user)
            Dim userIdentity As WindowsIdentity = WindowsIdentity.GetCurrent()  'for Windows Applications 
            'WindowsIdentity userIdentity = this.Request.LogonUserIdentity; //for ASP.NET Applications
            'Retrieve delegate user Login
            Dim delegateUserLogin As NTAccount = New NTAccount("DOMAIN", "delegateuseraccount")
            'Retrieve delegate user SID
            Dim delegateSID As SecurityIdentifier = CType(delegateUserLogin.Translate(GetType(SecurityIdentifier)), SecurityIdentifier)
            Dim delegateNetSqlAzManSID As IAzManSid = New SqlAzManSID(delegateSID)
            'Estabilish delegate authorization (only Allow or Deny)
            Dim delegateAuthorization As RestrictedAuthorizationType = RestrictedAuthorizationType.Allow
            'Remove delegate and all custom attributes
            myop.DeleteDelegateAuthorization(userIdentity, delegateNetSqlAzManSID, delegateAuthorization)
        End Sub
        ' <summary>
        ' Create a Full Storage through .NET code
        ' </summary>
        Private Sub CreateFullStorage()
            ' USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Administrators

            'Sql Storage connection string
            Dim sqlConnectionString As String = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password"
            'Create an instance of SqlAzManStorage class
            Dim storage As IAzManStorage = New SqlAzManStorage(sqlConnectionString)
            'Open Storage Connection
            storage.OpenConnection()
            'Begin a new Transaction
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted)
            'Create a new Store
            Dim NewStore As IAzManStore = storage.CreateStore("My Store", "Store description")
            'Create a new Basic StoreGroup
            Dim NewStoreGroup As IAzManStoreGroup = NewStore.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), "My Store Group", "Store Group Description", String.Empty, GroupType.Basic)
            'Retrieve current user SID
            Dim mySid As IAzManSid = New SqlAzManSID(WindowsIdentity.GetCurrent().User)
            'Add myself as member of "My Store Group"
            Dim storeGroupMember As IAzManStoreGroupMember = NewStoreGroup.CreateStoreGroupMember(mySid, WhereDefined.Local, True)
            'Create a new Application
            Dim NewApp As IAzManApplication = NewStore.CreateApplication("New Application", "Application description")
            'Create a new Role
            Dim NewRole As IAzManItem = NewApp.CreateItem("New Role", "Role description", ItemType.Role)
            'Create a new Task
            Dim NewTask As IAzManItem = NewApp.CreateItem("New Task", "Task description", ItemType.Task)
            'Create a new Operation
            Dim NewOp As IAzManItem = NewApp.CreateItem("New Operation", "Operation description", ItemType.Operation)
            'Add "New Operation" as a member of "New Task"
            NewTask.AddMember(NewOp)
            'Add "New Task" as a member of "New Role"
            NewRole.AddMember(NewTask)
            'Create an authorization for myself on "New Role"
            Dim auth As IAzManAuthorization = NewRole.CreateAuthorization(mySid, WhereDefined.Local, mySid, WhereDefined.Local, AuthorizationType.AllowWithDelegation, New Nullable(Of DateTime)(), New Nullable(Of DateTime)())
            'Create a custom attribute
            Dim attr As IAzManAttribute(Of IAzManAuthorization) = auth.CreateAttribute("New Key", "New Value")
            'Commit transaction
            storage.CommitTransaction()
            'Close connection
            storage.CloseConnection()
        End Sub
    End Class
End Namespace

