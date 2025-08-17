output "resource_group_id" {
  description = "value of the resource group id"
    value       = data.azurerm_resource_group.existingResourceGroup.id
}