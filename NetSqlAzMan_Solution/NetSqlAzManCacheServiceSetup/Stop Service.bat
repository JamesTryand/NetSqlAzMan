@echo off
@echo Stopping .NET Sql Authorization Manager Cache Service
SET PATH=%systemroot%\SysWOW64;%systemroot%\system32;%PATH%
NET STOP NetSqlAzManCacheService
pause