Dim oShell
Set oShell = CreateObject("WScript.Shell")
oShell.Run "NET START NetSqlAzManCacheService", 0, true
