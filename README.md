# SalesEvaluation

This project is a learning project which is used for Azure dvelopment. This project entails steps on how to deploy projects to azure through various means such as:<br> 
    - Deploying on a virtual Machine<br>
    - Deploying on an azure Web app<br>
    - Deploying using Docker<br>
    - Deploying using Kubernetes.

This project will also be using **terraform** for the provisioning of the cloud infrastructure.

# General Resources
- [GitHub ReadMe Styling](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#styling-text) 

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
            -  Linking it to this GithHub Repository (This is done by choosing the version control Workflow).
 2. Next I configured terrfaorm cloud for single sign on with Microsoft Entra ID.


 #### RoadBlocks
    Azure playground in Pluralsight has restrictions in azure which prevents me fro mcreating a managaed identity and as result I could not link to terraform cloud. So for now I will be performing most actions locally but these processes should still work in an actual azure environment.


### Local Development Steps
1. Logged on to the playground using azure cli command **Az Login**. This prompts an interactive login.

2. In the root of my application code, I created an Infra folder which I created my terrform main.tf file. For basic terraform commnands used: <br>
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
    - [ ] Define VNet address space (e.g., `10.0.0.0/16`)
    - [ ] Define subnets with non-overlapping IP ranges:
    - [ ] Public subnet for user-facing resources
    - [ ] Private subnet for core applications
    - [ ] Decide routing and access rules for public vs private subnet

    ## 2. Create VNet and Subnets
    - [ ] Create the virtual network
    - [ ] Create the public subnet
    - [ ] Create the private subnet
    - [ ] Ensure address ranges do not overlap
    - [ ] Ensure DDOS protection for the Vnet

    ## 3. Implement Network Security Groups (NSGs)
    - [ ] Create NSG for public subnet
    - [ ] Allow inbound traffic from internet (HTTP/HTTPS)
    - [ ] Allow restricted admin access (SSH/RDP)
    - [ ] Create NSG for private subnet
    - [ ] Block inbound internet traffic
    - [ ] Allow traffic only from public subnet
    - [ ] Associate NSGs with respective subnets

    ## 4. Set Up Secure Internet Access
    - [ ] Deploy NAT Gateway or Azure Firewall for outbound traffic from private subnet
    - [ ] Restrict outbound traffic to required destinations

    ## 5. Deploy Public-Facing Resource
    - [ ] Deploy VM, Application Gateway, or Load Balancer in public subnet
    - [ ] Configure it to forward requests to private subnet

    ## 6. Configure Internal Routing
    - [ ] Ensure public subnet can communicate with private subnet
    - [ ] Enforce private subnet access restrictions

    ## 7. Apply Security Enhancements
    - [ ] Enable DDoS protection for public subnet
    - [ ] Use private endpoints for databases or storage in private subnet
    - [ ] Enable logging and monitoring for network activity
    - [ ] Regularly update VMs and applications for security patches

    ## 8. Test the Setup
    - [ ] Verify users can reach public subnet services
    - [ ] Verify private subnet resources are inaccessible directly from the internet
    - [ ] Test public → private communication is working securely

