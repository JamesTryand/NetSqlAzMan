@echo off
@echo Starting .NET Sql Authorization Manager Cache Service
SET PATH=%systemroot%\SysWOW64;%systemroot%\system32;%PATH%
NET START NetSqlAzManCacheService
pause