@echo off
@echo PreEmptive DotFuscator Post-Deployment

@echo Making Assemblies Strong Name (Delay Sign) 
"%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn" -R "NetSqlAzMan.dll" "NetSqlAzMan.pfx"
"%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn" -R "NetSqlAzMan.SnapIn.dll" "NetSqlAzMan.SnapIn.pfx"
"%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn" -R "NetSqlAzManCacheService.exe" "NetSqlAzManCacheService.pfx"
"%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn" -R "NetSqlAzManCacheServiceInvalidateUtility.exe" "NetSqlAzManCacheServiceInvalidateUtility.pfx"
"%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn" -R "NetSqlAzManWebConsole.dll" "NetSqlAzManWebConsole.pfx"

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

pause

