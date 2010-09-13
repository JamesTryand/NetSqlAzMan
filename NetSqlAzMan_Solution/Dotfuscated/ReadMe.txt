DEPLOY MEMO
- Re-Compile all in Debug mode
- Update NetSqlAzMan_Reference.chm with Sandcastle Help File Builder
- Update NetSqlAzManStorage.chm if storage change

1) Compile in "Release" mode
2) Run Dotfuscator Software Service - Run
3) Run Deploy.bat (enter passwords)
4) Compile in "Release" mode again
5) Fix NetSqlAzManSetup_x64.msi with:
"D:\Documenti\Visual Studio 2005\Projects\NetSqlAzMan_Solution\NetSqlAzManSetup_x64_Fix64BitMSIFile\bin\Release\NetSqlAzManSetup_x64_Fix64BitMSIFile.exe" 