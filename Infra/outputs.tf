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