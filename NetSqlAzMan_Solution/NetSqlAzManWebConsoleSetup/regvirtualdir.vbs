Dim customActionData
Dim virtualSite
Dim virtualDir
Dim oShell
Dim cmd

customActionData = Session.Property("CustomActionData")
virtualSite = Mid(customActionData, 1, InStr(customActionData, "|")-1)
virtualSite = Right(virtualSite, 1)
virtualDir = Mid(customActionData, InStr(customActionData, "|")+1)
cmd = "%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -s W3SVC/" & virtualSite & "/Root/" & virtualDir
set oShell = CreateObject("WScript.Shell") 
oShell.Run cmd, 0, true
set oShell = Nothing
