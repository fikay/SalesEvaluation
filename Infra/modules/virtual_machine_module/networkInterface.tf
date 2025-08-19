resource "azurerm_network_interface" "sales_evaluation_bastion_nic" {
  name                = "bastion-nic"
  location            = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.sales_evaluation_bastion_subnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id  = azurerm_public_ip.bastion_vm_public_ip.id
  }

    tags = {
        name = "sales_evaluation_bastion_nic"
        user = "VirtualMachineModule"
    }
}


resource "azurerm_network_interface" "private_subnet_nics" {
    for_each = var.private_vms_nic_names
    name                = each.value.nic_name
    location            = data.azurerm_resource_group.existingResourceGroup.location
    resource_group_name = data.azurerm_resource_group.existingResourceGroup.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.sales_evaluation_private_subnet.id
    private_ip_address_allocation = "Dynamic"
  }

    tags = {
        name = each.value.nic_name
        user = "VirtualMachineModule"
    }
}

