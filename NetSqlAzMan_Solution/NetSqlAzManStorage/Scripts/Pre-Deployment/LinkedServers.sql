/** ADD ADSI LINKED SERVER PROVIDER **/
    -- CHECK IF SERVER ALREADY EXISTS
    if not exists (select * from master.dbo.sysservers where srvname = 'ADSI')
    begin
     exec sp_addlinkedserver 'ADSI', 'Active Directory Service Interfaces', 'ADSDSOObject', 'adsdatasource'
     /** REMEMBER: change security context credentials for this linked server to allow ADSI provider to estabilish a connection with your DOMAIN **/
    end
GO