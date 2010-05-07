
ALTER DATABASE [$(DatabaseName)]
ADD FILE 
(
    NAME = [PrimaryFileName]
    , FILENAME = N'$(DefaultDataPath)$(DatabaseName).mdf'
    
) TO FILEGROUP [PRIMARY]

