variable "subscription_id" {
  description = "The Subscription ID for the Azure Account"
  type        = string
}


variable "resource_group_name" {
  description = "The name of the resource group in which resources are created"
  type        = string
}

variable "admin_username" {
  description = "The administrator username for the virtual machine"
  type        = string
}

variable "admin_password" {
  description = "The administrator password for the virtual machine"
  type        = string
  sensitive   = true
}

variable "db_password" {
  description = "The password for the database"
  type        = string
  sensitive   = true
}