Imports System
Imports System.Security.Principal
Imports System.IO
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Collections.Generic
Imports System.Text
Imports NetSqlAzMan
Imports NetSqlAzMan.Interfaces

Namespace MyApplication.BizRules
    Public NotInheritable Class BizRule : Implements IAzManBizRule

        Public Sub New()
        End Sub

        Public Overloads Function Execute(ByVal contextParameters As Hashtable, ByVal identity As IAzManSid, ByVal ownerItem As IAzManItem, ByRef ForcedCheckAccessResult As AuthorizationType) As Boolean _
            Implements IAzManBizRule.Execute
            Return True
        End Function
    End Class
End Namespace
