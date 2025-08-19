variable "resource_group_name" {
  description = "The name of the resource group in which resources are created"
  type        = string
}

variable "nat_gateway_ip" {
  description = "The public IP address for the NAT gateway"
  type        = string
  default     = null
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

variable "private_vms_nic_names" {
  default = {
  front_end = {
    vm_name = "frontendvm"
    nic_name = "front_end_nic"
  }
  back_end = {
    vm_name = "backendvm"
    nic_name = "back_end_nic"
  }
  }
}