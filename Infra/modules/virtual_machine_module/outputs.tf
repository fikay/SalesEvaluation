output "resource_group_id" {
  description = "value of the resource group id"
    value       = data.azurerm_resource_group.existingResourceGroup.id
}

output "nat_gateway_ip_address" {
  description = "The public IP address for the NAT gateway"
  value       = azurerm_public_ip.ngateway_public_ip.ip_address
}

output "bastion_ipadress" {
  description = "value of the bastion subnet IP address"
  value       = azurerm_subnet.sales_evaluation_bastion_subnet.address_prefixes[0]
}
