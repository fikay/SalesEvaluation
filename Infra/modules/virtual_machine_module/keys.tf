resource "azurerm_ssh_public_key" "bastion_public_key" {
  name                = "bastion_public_key"
  resource_group_name = data.azurerm_resource_group.existingResourceGroup.name
  location            = data.azurerm_resource_group.existingResourceGroup.location
  public_key          = file("${path.module}/../../keys/bastionKey.pub")
}