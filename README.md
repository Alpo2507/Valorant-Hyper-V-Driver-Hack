# Valorant-Hyper-V-Driver
Valorant-Hyper-V-Driver module provides a collection of Hyper-V check Plugins, which serve to monitor Hyper-V services, VMM Agents, Clusters and much more. It is especially designed to be used under Hyper-V environments.Script to speed up or even automate the process of creating your VM's on Hyper-V. Quick run or even automate your dll on hyper v. more features such as Windows Sandbox automation are planned
Hyper-V’s export operation requires that the computer account in Active Directory have access to the location where the exports are being stored. I recommend creating an Active Directory group for the Hyper-V hosts and then giving the group the required ‘Full Control’ file and share permissions. When a NAS, such as a QNAP device is intended to be used as an export location, Hyper-V will not be able to complete the operation as the computer account will not have access to the share on the NAS. To copy all the files necessary for a complete backup, the VM must be in an offline state for the operation to be completed


## Usage

### Install packer from Chocolatey

```cmd
choco install packer --version=1.7.10 -y
```

### Install vagrant from Chocolatey

```cmd
choco install vagrant --version=2.2.19 -y
```

### Use account with Administrator privileges for Hyper-V


![Untitled](https://user-images.githubusercontent.com/99544239/153879416-0f95a71f-149b-4710-91a3-4b1040e39681.png)
