Import-Module "../bin/Debug/netstandard2.0/publish/Posh-SSH.psd1"

$pickle = Get-SSHTrustedHost 

$pickle | ft

exit

$username = "admin"
$password = "Minions12345"
$securePassword = $password | ConvertTo-SecureString -asPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential($username,$securePassword)

New-SSHSession -ComputerName "10.100.5.3" -Credential $creds -AcceptKey
