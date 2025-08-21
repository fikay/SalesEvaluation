-- Create a new database
CREATE DATABASE azureDeveloper;
GO

-- Create a SQL Server login
CREATE LOGIN AzureDev WITH PASSWORD = 'DevelopingSolutionsOneAtATime';
GO

-- Create a database user for that login in the new database
USE azureDeveloper;
GO
CREATE USER AzureDev FOR LOGIN AzureDev;
GO

-- Grant full access to the database
ALTER ROLE db_owner ADD MEMBER AzureDev;
GO
