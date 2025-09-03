# SalesEvaluation

This project is a learning project for 360Rides, which is also used for Azure development. This project entails steps on how to deploy projects to Azure through various means such as:<br> 
    - Deploying on a virtual Machine<br>
    - Deploying on an Azure Web app<br>
    - Deploying using Docker<br>
    - Deploying using Kubernetes.

This project will also be using **terraform** for the provisioning of the cloud infrastructure.

# General Resources
- [GitHub ReadMe Styling](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#styling-text) 

- [Azure resource articles](https://github.com/MicrosoftDocs/azure-docs/tree/main/articles)
# WEEKS

- [Week 1](#week-1---seting-up-terraform-cloud-and-test-deploying-a-virtual-machine)



 ## Week 1 - Seting up Terraform cloud and Test deploying a virtual machine

 ### Resources Used In This Section
-  [Terraform Video Used For Learning](https://www.youtube.com/watch?v=V53AHWun17s)
-  [Terraform Entra Id SSO](https://learn.microsoft.com/en-us/entra/identity/saas-apps/terraform-cloud-tutorial).
- [Terraform Language Docs](https://developer.hashicorp.com/terraform/language)
- [Terraform Azure Docs](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs#features-2)



This portion started off with creating: <br>
 1. A terrform cloud account
    - Creating an Organization
        - Creating a workspace
            -  Linking it to this GitHub Repository (This is done by choosing the version control Workflow).
 2. Next, I configured Terraform Cloud for single sign-on with Microsoft Entra ID.


 #### RoadBlocks
    Azure playground in Pluralsight has restrictions in azure which prevents me fro mcreating a managaed identity, and as resul,t I could not link to terraform cloud. So for now I will be performing most actions locally but these processes should still work in an actual azure environment.


### Local Development Steps
1. Logged on to the playground using the Azure CLI command **Az Login**. This prompts an interactive login.

2. In the root of my application code, I created an Infra folder where I created my Terraform main.tf file. For basic Terraform commands used: <br>
    - **terraform fmt** - For fromatting the code.
    - **terraform init** - For Initializing terraform. On running this, you should see a lock file and terraform folder.
    - **terraform plan** - For planning the infrastructure deployment.
    - **terraform apply** - For deploying the Infastructure.
    - **terraform state list** - List our terraform state
3. I created a module folder in the Infra folder as I will need different modules. The first module created is the Virtual_Machine module. This wil host all terraform files pertinent to deploying a virtual machine in Azure and also deploying the application to azure.<BR>
N:B In trying to plan just a test for the deployment, I ran into another restriction of the sandbox. This restriction prevented automatic registrationof azure providers and as a result the planning always failed. The work arounnd for this wa to manually register the providers and also 
set **resource_provider_registrations = "none"** in the terraform provider block.

**authorization failed: registering resource provider "Microsoft.AVS": unexpected status 403 (403 Forbidden) with error**

    **Steps to Manually register a Provider**<br>
        - Run the command:  **az provider list --query "[].{Namespace:namespace,RegistrationState:registrationState}"  --output table** to see all provider
        - Run **az provider register --namespace Micorosft.AVS**
**We also do not have permissions to create our own resource group in Azure Playground** as a resource I switched my terrfaorm code to use the existing resource group.

4. Once able to get the details of the existing resource group, I then went ahead to creating a Virtual network.<br>
     # Tasks for the Virtual network:<br>
     ## 1. Plan Your Network
    - [x] Define VNet address space (e.g., `10.0.0.0/16`)
    - [x] Define subnets with non-overlapping IP ranges:
    - [x] Public subnet for user-facing resources
    - [x] Private subnet for core applications
    - [x] Decide routing and access rules for public vs private subnet

    ## 2. Create VNet and Subnets
    - [x] Create the virtual network
    - [x] Create the public subnet
    - [x] Create the private subnet
    - [x] Ensure address ranges do not overlap
    - [ ] Ensure DDOS protection for the Vnet

    ## 3. Implement Network Security Groups (NSGs)
    - [x] Create NSG for public subnet
    - [x] Allow inbound traffic from internet (HTTP/HTTPS)
    - [x] Allow restricted admin access (SSH/RDP)
    - [x] Create NSG for private subnet
    - [x] Block inbound internet traffic
    - [x] Allow traffic only from public subnet
    - [x] Associate NSGs with respective subnets

    ## 4. Set Up Secure Internet Access
    - [x] Deploy NAT Gateway or Azure Firewall for outbound traffic from private subnet
    - [x] Restrict outbound traffic to required destinations
    
    # Day 2

    Notes from day 2:
        - When running the custom data script, it uses the root user, so when I ssh'ed into it and using my profile, I could not get some commands to work as they were only installed for the root user.In order to get it to work I changed the cript to install the tool globally for all users.

        Also, when you ssh into a VM and you by chance reconfigure it and it has the same IP address, you need to delete the previous one from the .ssh folder.

        In the Bastion VM after installling sql server and trying to set it up, It required a password for my user which had not been set so to switch to the root I used **sudo -i**  Switch back to my user using **su -fikayofaks**

        To Login to the sql server using the cli sqlcmd -s localhost -U sa -P 'yourpassword' -C -l 30 -q "select @@version" (last part is for the trusrtserver cert , -q tells it to kick you out immediayely after).

        Running dotnet inside the project folder using the launch setting aspnetcore environment. Using the dll you have to set it in the ~/bash .


        **Running from the folder itself**
        dotnet publish -c Release -o /var/www/backend/published
         kill -SIGTERM 7010
        ASPNETCORE_ENVIRONMENT=Development dotnet /var/www/backend/published/SalesEvaluation.Backend.dll --urls "http://0.0.0.0:5000" &

        ASPNETCORE_ENVIRONMENT=Development dotnet SalesEvaluation.Backend.dll


        Port Forwarding SSH -L 5000:10.0.2.4:5000 fikayofaks@172.190.47.173

        End Of Day  - sucessfully ran the backend on the bastion and port forwaded from my local host to see the Swagger UI

    # Day 3
        Today I am more keen on why my bootstrap script did not run to completion as sqlserver and slqcmd was not installed.
        
        1.  First steps check if the custom data script ran to completion. This can be done checking the cloud init logs in the folder /var/log
         -  nano /var/log/cloud-init-output.log
            -  Found the issue was due to it requiring interaction and aborted when It couldn't get any interaction.

     After figuring that out here are the new commands I included in the script 
     ```
        curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
        curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list | sudo tee /etc/apt/sources.list.d/mssql-server-2022.list
        sudo apt-get update -y
        sudo apt-get install -y mssql-server


        #Switch to root user and change user password
        sudo -i 
        ${var.admin_username}:${var.admin_password} | chpasswd

        #Switch back to the original user
        su ${var.admin_username} 

        sudo MSSQL_PID=Developer SA_PASSWORD="${var.admin_password}" ACCEPT_EULA=Y /opt/mssql/bin/mssql-conf setup

        systemctl status mssql-serveR
     ```
    ## 5. Deploy Public-Facing Resource
    - [ ] Deploy VM, Application Gateway, or Load Balancer in public subnet.
    This step invovled creating a VM in the bastion subnet and creating a public key to enable ssh into the VM using **ssh-keygen**.<br>
        **Commands Used**<br>
            - ssh-keygen -t rsa<br>
            - ssh -i C:\Users\fikay\source\repos\SalesEvaluation\Infra\keys\bastionKey fikayofaks@40.112.232.46 <br> (to ssh into the VM)<br>

        In other to get the SSH key unto the Bastion VM, I had to use the provisioner file and remote-exec block. The file block moves a file from my local directory into the VM. In order to do that, it needs a way to connect to the VM through ssh which leads to the connection block.
    - [ ] Configure it to forward requests to private subnet

    ## 6. Configure Internal Routing
    - [x] Ensure public subnet can communicate with private subnet
    - [x] Enforce private subnet access restrictions

    ## 7. Apply Security Enhancements
    - [ ] Enable DDoS protection for public subnet
    - [ ] Use private endpoints for databases or storage in private subnet
    - [ ] Enable logging and monitoring for network activity
    - [ ] Regularly update VMs and applications for security patches

    ## 8. Test the Setup
    - [ ] Verify users can reach public subnet services
    - [ ] Verify private subnet resources are inaccessible directly from the internet
    - [ ] Test public → private communication is working securely

