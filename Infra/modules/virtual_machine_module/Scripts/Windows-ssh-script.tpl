add-content -path c:/users/derek/.ssh/config -value @'

Host $(hostname)
    hostname $(hostname)
    User $(user)
    IdentityFile $(IdentityFile)

'