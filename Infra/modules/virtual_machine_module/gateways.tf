resource "azurerm_nat_gateway" "ngateway" {
  name                  = "sales_evaluation_nat_gateway"
  location              = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name   = data.azurerm_resource_group.existingResourceGroup.name
  sku_name              = "Standard"
  idle_timeout_in_minutes = 10
    
    tags = {
        name = "sales_evaluation_nat_gateway"
        user = "VirtualMachineModule"
    }

}


resource "azurerm_public_ip" "ngateway_public_ip" {
  name = "${azurerm_nat_gateway.ngateway.name}-public-ip"
  location = data.azurerm_resource_group.existingResourceGroup.location
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
  allocation_method = "Static"
  sku = "Standard"
}



resource "azurerm_nat_gateway_public_ip_association" "ip-ng_public_ip_association" {
  nat_gateway_id       = azurerm_nat_gateway.ngateway.id
  public_ip_address_id = azurerm_public_ip.ngateway_public_ip.id
}