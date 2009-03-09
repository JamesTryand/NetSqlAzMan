Imports NetSqlAzMan.Interfaces

Public Class Form1
    Dim chk As New My_Application.Security.CheckAccessHelper("Data source=.;initial catalog=NetSqlAzManStorage;Integrated Security=SSPI;", System.Security.Principal.WindowsIdentity.GetCurrent())

    Public Function a() As Boolean
        Dim dic As New Dictionary(Of String, Object)
        dic.Add("k1", "v1")
        Dim aa(0) As KeyValuePair(Of String, Object)
        Dim i As Integer = 0
        For Each kv As KeyValuePair(Of String, Object) In dic
            aa(i) = kv
            i += 1
        Next

        Return chk.CheckAccess(My_Application.Security.CheckAccessHelper.Role.My_Role, aa)
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        MessageBox.Show(MyClass.a())
    End Sub
End Class
