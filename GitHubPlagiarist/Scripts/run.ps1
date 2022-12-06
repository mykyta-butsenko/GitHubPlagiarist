Foreach ($configFile in Get-ChildItem -Path $PSScriptRoot\Configuration\*.json)
{
    Start-Process -FilePath $PSScriptRoot\Application\GitHubPlagiarist.exe -ArgumentList $configFile
}