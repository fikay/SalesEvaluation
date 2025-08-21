# output "subscription_id" {
#   description = "The Subscription ID for the Azure Account"
#     value       = var.subscription_id
# }

output "resource_group_id" {
  description = "value of the resource group id"
    value       = module.virtual_machine_module.resource_group_id
}

output "resource_group_name" {
  description = "The name of the resource group in which resources are created"
    value       = var.resource_group_name
}

output "nat_gateway_ip_address" {
  description = "value of the NAT gateway IP address"
  value       = module.virtual_machine_module.nat_gateway_ip_address
}


output "bastion_ip_address" {
  description = "value of the bastion subnet IP address"
  value       = module.virtual_machine_module.bastion_ipadress
}

output "bastion_public_ip_address" {
  description = "The public IP address for the Bastion VM"
  value       = module.virtual_machine_module.bastion_public_ip_address
}