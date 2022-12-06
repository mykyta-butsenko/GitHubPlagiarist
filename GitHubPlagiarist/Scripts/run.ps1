Foreach ($configFile in Get-ChildItem -Path $PSScriptRoot\Configuration\*.json)
{
    Start-Process -FilePath $PSScriptRoot\Application\GitHubSearch.exe -ArgumentList $configFile
}