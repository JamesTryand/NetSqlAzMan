@echo off
@echo PreEmptive DotFuscator Post-Deployment

@echo Making Assemblies Strong Name (Delay Sign) 
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn" -R "NetSqlAzMan.dll" "NetSqlAzMan.pfx"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn" -R "NetSqlAzMan.SnapIn.dll" "NetSqlAzMan.SnapIn.pfx"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn" -R "NetSqlAzManCacheService.exe" "NetSqlAzManCacheService.pfx"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn" -R "NetSqlAzManCacheServiceInvalidateUtility.exe" "NetSqlAzManCacheServiceInvalidateUtility.pfx"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn" -R "NetSqlAzManWebConsole.dll" "NetSqlAzManWebConsole.pfx"

@echo Copying Strong Name Assemblies to source directories
copy netsqlazman.dll ..\NetSqlAzMan\bin\Debug\ /Y
copy netsqlazman.dll ..\NetSqlAzMan\bin\Release\ /Y

copy netsqlazman.dll ..\NetSqlAzMan.SnapIn\bin\Debug\ /Y
copy netsqlazman.dll ..\NetSqlAzMan.SnapIn\bin\Release\ /Y


copy netsqlazman.snapin.dll ..\NetSqlAzMan.SnapIn\bin\Debug\ /Y
copy netsqlazman.snapin.dll ..\NetSqlAzMan.SnapIn\bin\Release\ /Y

copy netsqlazman.dll ..\NetSqlAzManCacheService\bin\Debug\ /Y
copy netsqlazman.dll ..\NetSqlAzManCacheService\bin\Release\ /Y

copy NetSqlAzManCacheService.exe ..\NetSqlAzManCacheService\bin\Debug\ /Y
copy NetSqlAzManCacheService.exe ..\NetSqlAzManCacheService\bin\Release\ /Y

copy NetSqlAzManCacheServiceInvalidateUtility.exe ..\NetSqlAzManCacheServiceInvalidateUtility\bin\Debug\ /Y
copy NetSqlAzManCacheServiceInvalidateUtility.exe ..\NetSqlAzManCacheServiceInvalidateUtility\bin\Release\ /Y

copy netsqlazman.dll ..\NetSqlAzManWebConsole\bin\ /Y

copy NetSqlAzManWebConsole.dll ..\NetSqlAzManWebConsole\bin\ /Y

@Echo Remember to Fixing NetSqlAzManSetup_x64.msi
@echo "D:\Documenti\Visual Studio 2005\Projects\NetSqlAzMan_Solution\NetSqlAzManSetup_x64_Fix64BitMSIFile\bin\Release\NetSqlAzManSetup_x64_Fix64BitMSIFile.exe"

pause

