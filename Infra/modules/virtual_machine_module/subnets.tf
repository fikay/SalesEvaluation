resource "azurerm_subnet" "sales_evaluation_public_subnet" {
  name                 = "sales_evaluation_subnet"
  resource_group_name  = data.azurerm_resource_group.existingResourceGroup.name
  virtual_network_name = azurerm_virtual_network.sales_evaluation_vnet.name
  address_prefixes = ["10.0.0.0/24"]
}

resource "azurerm_subnet" "sales_evaluation_private_subnet" {
  name                 = "sales_evaluation_private_subnet"
  resource_group_name  = data.azurerm_resource_group.existingResourceGroup.name
  virtual_network_name = azurerm_virtual_network.sales_evaluation_vnet.name
  address_prefixes = ["10.0.1.0/24"]
  default_outbound_access_enabled = false
}

resource "azurerm_subnet" "sales_evaluation_bastion_subnet" {
  name                 = "sales_evaluation_public_subnet"
  resource_group_name  = data.azurerm_resource_group.existingResourceGroup.name
  virtual_network_name = azurerm_virtual_network.sales_evaluation_vnet.name
  address_prefixes = ["10.0.2.0/24"]
}