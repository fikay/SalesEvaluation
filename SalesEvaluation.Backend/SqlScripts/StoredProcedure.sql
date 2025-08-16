IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'FILES')
	EXEC('CREATE SCHEMA FILES');
GO

-- This script creates a stored procedure to insert file details into the FilesDetails table.
IF OBJECT_ID('FILES.INSERTFILE') IS NOT NULL
	DROP PROCEDURE FILES.INSERTFILE;
GO

IF OBJECT_ID('dbo.INSERTFILE') IS NOT NULL
    DROP PROCEDURE dbo.INSERTFILE;
GO

CREATE PROCEDURE FILES.INSERTFILE
    @FileName NVARCHAR(255),
    @FileType NVARCHAR(50),
    @UploadedBy NVARCHAR(255),
    @UserId UNIQUEIDENTIFIER,
    @Content VARBINARY(MAX)
AS
BEGIN
    INSERT INTO FILES.FilesDetails (FileName, FileType, UploadedBy, UploadedAt, UserId, Content)
    VALUES (@FileName, @FileType, @UploadedBy, SYSUTCDATETIME(), @UserId, @Content);
END;
GO
