data "azurerm_resource_group" "existingResourceGroup" {
  name = var.resource_group_name
}

resource "azurerm_virtual_network" "sales_evaluation_vnet" {
  name = "sales_evaluation_vnet"
  location = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
  address_space = ["10.0.0.0/16"]

  tags = {
    name = "sales_evaluation_vnet"
    user = "VirtualMachineModule"
  }
}


# resource "azurerm_network_ddos_protection_plan" "ddos_protection" {
#   name                = "${data.azurerm_resource_group.existingResourceGroup.name}-ddos-protection-plan"
#   location            = data.azurerm_resource_group.existingResourceGroup.location
#   resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
# }