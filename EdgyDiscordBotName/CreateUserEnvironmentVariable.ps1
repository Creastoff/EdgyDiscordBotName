function Set-EnvironmentVariable
{
  param
  (
    [Parameter(Mandatory=$true)]
    [String]
    $Name,
    
    [Parameter(Mandatory=$true)]
    [String]
    $Value,
    
    [Parameter(Mandatory=$true)]
    [EnvironmentVariableTarget]
    $Target
  )
  [System.Environment]::SetEnvironmentVariable($Name, $Value, $Target)
}

$variableName = "DiscordBotApiKey";
$token = "tokenhere";
$target = "User";

Set-EnvironmentVariable -Name $variableName -Value $token -Target $target;