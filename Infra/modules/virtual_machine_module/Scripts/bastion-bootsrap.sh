#!/bin/bash
set -e

# Update the Linux system
sudo apt-get update -y
sudo apt-get upgrade -y

# Install .NET SDK and runtime
sudo apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0

# Add Microsoft repo keys for SQL Server
curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | sudo tee /usr/share/keyrings/microsoft-prod.gpg > /dev/null

# Add SQL Server repository
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/22.04/mssql-server-2022 jammy main" | sudo tee /etc/apt/sources.list.d/mssql-server-2022.list

sudo apt-get update -y

# Install SQL Server non-interactively
export ACCEPT_EULA=Y
export DEBIAN_FRONTEND=noninteractive
sudo apt-get install -y mssql-server

# Configure SQL Server (non-interactive)
sudo MSSQL_PID=Developer SA_PASSWORD="${sa_password}" ACCEPT_EULA=Y /opt/mssql/bin/mssql-conf setup
sudo systemctl enable mssql-server
sudo systemctl start mssql-server

# Install SQL Server tools
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/22.04/prod jammy main" | sudo tee /etc/apt/sources.list.d/msprod.list
sudo apt-get update -y
sudo DEBIAN_FRONTEND=noninteractive ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev

# Add sqlcmd to PATH
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
export PATH="$PATH:/opt/mssql-tools18/bin"
source ~/.bashrc

# Create database, login, and user
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${sa_password}" -C <<SQL
CREATE DATABASE azureDeveloper;
GO
CREATE LOGIN AzureDev WITH PASSWORD = '${db_user_password}';
GO
USE azureDeveloper;
GO
CREATE USER AzureDev FOR LOGIN AzureDev;
GO
ALTER ROLE db_owner ADD MEMBER AzureDev;
GO
SQL
