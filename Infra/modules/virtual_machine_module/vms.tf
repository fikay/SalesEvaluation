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

  custom_data = base64encode( templatefile("${path.module}/Scripts/bastion-bootsrap.sh", {
    sa_password = var.admin_password
    db_user_password = var.db_password
  }))
   # SSH connection for provisioners
  connection {
    type        = "ssh"
    host        =azurerm_public_ip.bastion_vm_public_ip.ip_address
    user        = var.admin_username
    private_key = file("${path.module}/../../keys/bastionKey")
    timeout     = "2m"
  }

  provisioner "file" {
    source      = "${path.module}/../../keys/bastionKey"
    destination = "/home/${var.admin_username}/.ssh/bastionKey"
    
  }

  provisioner "remote-exec" {
    inline = [ 
         "chmod 600 /home/${var.admin_username}/.ssh/bastionKey",
         "chown ${var.admin_username}:${var.admin_username} /home/${var.admin_username}/.ssh/bastionKey"
     ]
  }

  tags = {
    name = "sales_evaluation_bastion_vm"
    user = "VirtualMachineModule"
  }
  
}


resource "azurerm_linux_virtual_machine" "sales_evaluation_app_vms" {
for_each = var.private_vms_nic_names
  name                  = each.value.vm_name
  resource_group_name   = data.azurerm_resource_group.existingResourceGroup.name
  location              = data.azurerm_resource_group.existingResourceGroup.location
  size                  = "Standard_DS1_v2"
  admin_username        = var.admin_username
  admin_password        = var.admin_password
  network_interface_ids = [azurerm_network_interface.private_subnet_nics[each.key].id]


  admin_ssh_key {
    username   = var.admin_username
    public_key = azurerm_ssh_public_key.bastion_public_key.public_key
  }
  
  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  custom_data = filebase64("${path.module}/Scripts/bastion-bootsrap.sh")
  source_image_reference {
    publisher = "Canonical"
    offer     = "0001-com-ubuntu-server-jammy"
    sku       = "22_04-lts-gen2"
    version   = "latest"
  }

  tags = {
    name = each.value.vm_name
    user = "VirtualMachineModule"
  }
  
}