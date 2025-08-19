#!/bin/bash

#Update the Linux system
sudo apt-get update -y
sudo apt-get upgrade -y

#Install .NET SDK
sudo apt-get install -y dotnet-sdk-8.0

#Install the runtime also 
sudo apt-get install -y aspnetcore-runtime-8.0


#Install sql server adn set up the database 
curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list | sudo tee /etc/apt/sources.list.d/mssql-server-2022.list
sudo apt-get update
sudo apt-get install -y mssql-server

#Install sql server command line tools
curl https://packages.microsoft.com/keys/microsoft.asc | sudo tee /etc/apt/trusted.gpg.d/microsoft.asc
curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list | sudo tee /etc/apt/sources.list.d/mssql-release.list
sudo apt-get update
sudo apt-get install mssql-tools18 unixodbc-dev

echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' | sudo tee /etc/profile.d/mssql-tools.sh
sudo chmod +x /etc/profile.d/mssql-tools.sh
source /etc/profile.d/mssql-tools.sh


