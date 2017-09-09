[CmdletBinding()]
param (   
    [Parameter(Mandatory=$True)]
    [string]$apikey
)
$ErrorActionPreference = 'Stop'
cd dist
choco push --source "https://chocolatey.org" -k="'$apikey'"
cd ..
