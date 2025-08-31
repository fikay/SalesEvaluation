#!/bin/bash
set -e

# Update the Linux system
sudo apt-get update -y
sudo apt-get upgrade -y

# Install .NET SDK and runtime
sudo apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0




#Install GithHub CLI
(type -p wget >/dev/null || (sudo apt update && sudo apt install wget -y)) \
	&& sudo mkdir -p -m 755 /etc/apt/keyrings \
	&& out=$(mktemp) && wget -nv -O$out https://cli.github.com/packages/githubcli-archive-keyring.gpg \
	&& cat $out | sudo tee /etc/apt/keyrings/githubcli-archive-keyring.gpg > /dev/null \
	&& sudo chmod go+r /etc/apt/keyrings/githubcli-archive-keyring.gpg \
	&& sudo mkdir -p -m 755 /etc/apt/sources.list.d \
	&& echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | sudo tee /etc/apt/sources.list.d/github-cli.list > /dev/null \
	&& sudo apt update \
	&& sudo apt install gh -y


# Configure GitHub CLI
# export GITHUB_TOKEN=${github_token}
export HOME=/home/fikayofaks
export DOTNET_CLI_HOME=$HOME
# Clone the repository into www directory and publish the application
if [ "${dest}" == "backend" ];then
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


sudo mkdir /var/www
sudo chown $USER:$USER /var/www
cd /var/www
git clone https://${github_token}@github.com/fikay/SalesEvaluation.git
cd SalesEvaluation/SalesEvaluation.Backend
# Restore dependencies
dotnet restore
# Publish the application
sudo mkdir -p /var/www/sales-evaluation-${dest}

#Get the appsettings file from /home directory and copy sqlscripts to the www directory
sudo cp /home/fikayofaks/appsettings.Development.json /var/www/sales-evaluation-${dest}/appsettings.Development.json
sudo cp -r /var/www/SalesEvaluation/SalesEvaluation.Backend/SqlScripts /var/www/sales-evaluation-${dest}/SqlScripts
# Publish the application to the specified output directory
dotnet publish -c Release -o /var/www/sales-evaluation-${dest}

#Create a systemd service for the .NET application
sudo tee /etc/systemd/system/sales-evaluation-${dest}.service <<EOF
[Unit]
Description=Sales Evaluation .NET Web ${dest}
After=network.target

[Service]
WorkingDirectory=/var/www/sales-evaluation-${dest}
ExecStart=/usr/bin/dotnet /var/www/sales-evaluation-${dest}/SalesEvaluation.Backend.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=sales-evaluation-${dest}
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Development
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_CLI_TELEMETRY_OPTOUT=1
Environment=DOTNET_CLI_HOME=/home/fikayofaks
Environment=HOME=/home/fikayofaks
Environment=DOTNET_ROOT=/usr/share/dotnet


[Install]
WantedBy=multi-user.target
EOF
fi

if [ "${dest}" == "frontend" ];then
sudo mkdir /var/www
sudo chown $USER:$USER /var/www
cd /var/www
git clone https://${github_token}@github.com/fikay/SalesEvaluation.git
cd SalesEvaluation/SalesEvaluation
# Restore dependencies
dotnet restore
# Publish the application
sudo mkdir -p /var/www/sales-evaluation-${dest}

#Get the appsettings file from /home directory and copy sqlscripts to the www directory
sudo cp /home/fikayofaks/appsettings.Development.json /var/www/sales-evaluation-${dest}/appsettings.Development.json
# sudo cp -r /var/www/SalesEvaluation/SalesEvaluation.Backend/SqlScripts /var/www/sales-evaluation-API/SqlScripts
# Publish the application to the specified output directory
dotnet publish -c Release -o /var/www/sales-evaluation-${dest}

sudo tee /etc/systemd/system/sales-evaluation-${dest}.service <<EOF
[Unit]
Description=Sales Evaluation .NET Web ${dest}
After=network.target

[Service]
WorkingDirectory=/var/www/sales-evaluation-${dest}
ExecStart=/usr/bin/dotnet /var/www/sales-evaluation-${dest}/SalesEvaluation.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=sales-evaluation-${dest}
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Development
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_CLI_TELEMETRY_OPTOUT=1
Environment=DOTNET_CLI_HOME=/home/fikayofaks
Environment=HOME=/home/fikayofaks
Environment=DOTNET_ROOT=/usr/share/dotnet


[Install]
WantedBy=multi-user.target
EOF

fi

# Reload systemd to recognize the new service
sudo systemctl daemon-reload
# Start and enable the service to run on boot
sudo systemctl start sales-evaluation-${dest}
sudo systemctl enable sales-evaluation-${dest}
# Install Nginx
sudo apt-get install -y nginx


# Configure Nginx to serve the application
sudo tee /etc/nginx/sites-available/sales-evaluation-${dest} <<EOF
server {
    listen 80;
    server_name localhost;

    location / {
        proxy_pass http://localhost:5000; # Adjust the port if your app runs on a different port
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
    }
}
EOF
# Enable the Nginx configuration
sudo ln -s /etc/nginx/sites-available/sales-evaluation-${dest} /etc/nginx/sites-enabled/
# Test the Nginx configuration
sudo nginx -t
# Restart Nginx to apply the changes
sudo systemctl restart nginx
# Enable Nginx to start on boot
sudo systemctl enable nginx

# Install and configure Certbot for SSL (optional, if you want to secure the site)
# sudo apt-get install -y certbot python3-certbot-nginx






# 


#port