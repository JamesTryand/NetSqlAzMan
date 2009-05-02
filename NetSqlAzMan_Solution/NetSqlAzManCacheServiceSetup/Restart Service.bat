@echo off

@echo Stopping .NET Sql Authorization Manager Cache Service
NET STOP NetSqlAzManCacheService

@echo Starting .NET Sql Authorization Manager Cache Service
NET START NetSqlAzManCacheService
pause