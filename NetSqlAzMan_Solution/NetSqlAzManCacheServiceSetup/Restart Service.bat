@echo off
SET PATH=%systemroot%\SysWOW64;%systemroot%\system32;%PATH%
@echo Restarting .NET Sql Authorization Manager Cache Service
NET STOP NetSqlAzManCacheService
NET START NetSqlAzManCacheService
pause