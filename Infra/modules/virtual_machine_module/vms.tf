resource "azurerm_linux_virtual_machine" "sales_evaluation_bastion_vm" {
  name                  = "sales-evaluation-bastion-vm"
  resource_group_name   = data.azurerm_resource_group.existingResourceGroup.name
  location              = data.azurerm_resource_group.existingResourceGroup.location
  size                  = "Standard_DS1_v2"
  admin_username        = var.admin_username
  admin_password        = var.admin_password
  network_interface_ids = [azurerm_network_interface.sales_evaluation_bastion_nic.id]


  admin_ssh_key {
    username   = var.admin_username
    public_key = azurerm_ssh_public_key.bastion_public_key.public_key
  }
  
  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "0001-com-ubuntu-server-jammy"
    sku       = "22_04-lts-gen2"
    version   = "latest"
  }

  tags = {
    name = "sales_evaluation_bastion_vm"
    user = "VirtualMachineModule"
  }
  
}