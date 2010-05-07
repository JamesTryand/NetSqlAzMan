
ALTER DATABASE [$(DatabaseName)]
ADD LOG FILE 
(
    NAME = [PrimaryLogFileName]
    , FILENAME = N'$(DefaultDataPath)$(DatabaseName)_log.ldf'
    
) 

