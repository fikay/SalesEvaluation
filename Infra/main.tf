terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "4.40.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
  resource_provider_registrations = "none"
}

module "virtual_machine_module"{
  source = "./modules/virtual_machine_module"
  resource_group_name = var.resource_group_name
}