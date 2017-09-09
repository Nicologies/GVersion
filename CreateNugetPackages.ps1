$ErrorActionPreference = 'Stop'
Remove-Item -Force ./dist/GVersion*nupkg -ErrorAction Ignore
nuget pack ./GVersion/GVersion.nuspec -OutputDirectory dist
