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


