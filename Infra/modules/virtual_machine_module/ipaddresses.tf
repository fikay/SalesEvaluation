resource "azurerm_public_ip" "ngateway_public_ip" {
  name = "${azurerm_nat_gateway.ngateway.name}-public-ip"
  location = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
  allocation_method = "Static"
  sku = "Standard"

  tags = {
    name = "sales_evaluation_nat_gateway_public_ip"
    user = "VirtualMachineModule"
  }
}


resource "azurerm_public_ip" "bastion_vm_public_ip" {
  name = "bastion-public-ip"
  location = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
  allocation_method = "Static"
  sku = "Standard"

  tags = {
    name = "sales_evaluation_bastion_vm_public_ip"
    user = "VirtualMachineModule"
  }
}