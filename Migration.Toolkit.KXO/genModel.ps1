param([string]$connection)
dotnet ef dbcontext scaffold $connection Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Context --context KxoContext --data-annotations --force # --use-database-names

# Remove connection string
$workDir = $MyInvocation.MyCommand.Path
$workDir = [System.IO.Path]::GetDirectoryName($workDir)
$contextPath = "$workDir\Context\KXOContext.cs"
$contextText = [System.IO.File]::ReadAllText($contextPath)
$result = $contextText -replace '(?i-mn).*if \(\!optionsBuilder\.IsConfigured\)[\s\S]*?\}','';
$result = $result -replace '(?i-mn).*protected override void OnConfiguring[\s\S]*?\}','';
[System.IO.File]::WriteAllText($contextPath, $result)