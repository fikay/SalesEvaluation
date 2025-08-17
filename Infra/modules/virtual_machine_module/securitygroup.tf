resource "azurerm_network_security_group" "sales_evaluation_public_nsg" {
  name                = "sales_evaluation_public_nsg"
  location            = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name

  security_rule {
    name                       = "AllowHTTP"
    priority                   = 100
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_ranges    = ["80", "443"]
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
}

resource "azurerm_network_security_group" "sales_evaluation_bastion_nsg"{
    name                = "sales_evaluation_bastion_nsg"
    location            = data.azurerm_resource_group.existingResourceGroup.location
    resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
    
    security_rule {
        name                       = "AllowBastionAccess"
        priority                   = 100
        direction                  = "Inbound"
        access                     = "Allow"
        protocol                   = "Tcp"
        source_port_range          = "*"
        destination_port_ranges    = ["22", "3389"]
        source_address_prefix      = "*"
        destination_address_prefix = "*"
    }

    security_rule {
        name                       = "DenyAllInbound"
        priority                   = 200
        direction                  = "Inbound"
        access                     = "Deny"
        protocol                   = "*"
        source_port_range          = "*"
        destination_port_range     = "*"
        source_address_prefix      = "*"
        destination_address_prefix = "*"
    }
}

resource "azurerm_network_security_group" "sales_evaluation_private_nsg" {
  name                = "sales_evaluation_private_nsg"
  location            = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name

 security_rule {
    name                       = "AllowPublicSubnetAccessthroughHTTPS"
    priority                   = 100
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "*"
    source_port_range          = "*"
    destination_port_ranges    = ["22", "3389"]
    source_address_prefix      = azurerm_subnet.sales_evaluation_public_subnet.address_prefixes[0]
    destination_address_prefix = "*"
  }

  security_rule {
    name = "AllowSSH"
    priority = 200
    direction = "Inbound"
    access = "Allow"
    protocol = "Tcp"
    source_port_range = "*"
    destination_port_ranges = ["22"]
    source_address_prefix = azurerm_subnet.sales_evaluation_bastion_subnet.address_prefixes[0]
    destination_address_prefix = "*"
  }


  security_rule {
    name                       = "DenyAllInbound"
    priority                   = 300
    direction                  = "Inbound"
    access                     = "Deny"
    protocol                   = "*"
    source_port_range          = "*"
    destination_port_range     = "*"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
  
}


### Associate NSGs with Subnets
 resource "azurerm_subnet_network_security_group_association" "public_subnet_nsg_association"{
  subnet_id = azurerm_subnet.sales_evaluation_public_subnet.id
  network_security_group_id = azurerm_network_security_group.sales_evaluation_public_nsg.id
 }

resource "azurerm_subnet_network_security_group_association" "private_subnet_nsg_association" {
  subnet_id = azurerm_subnet.sales_evaluation_private_subnet.id
  network_security_group_id =azurerm_network_security_group.sales_evaluation_private_nsg.id
}

resource "azurerm_subnet_network_security_group_association" "bastion_subnet_nsg_association" {
  subnet_id = azurerm_subnet.sales_evaluation_bastion_subnet.id
  network_security_group_id = azurerm_network_security_group.sales_evaluation_bastion_nsg.id
}