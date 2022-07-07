param([string]$connection)
dotnet ef dbcontext scaffold $connection Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Context --context KX13Context --data-annotations --force #--use-database-names 

# Remove connection string
$workDir = $MyInvocation.MyCommand.Path
$workDir = [System.IO.Path]::GetDirectoryName($workDir)
$contextPath = "$workDir\Context\KX13Context.cs"
$contextText = [System.IO.File]::ReadAllText($contextPath)
$result = $contextText -replace '(?i-mn).*if \(\!optionsBuilder\.IsConfigured\)[\s\S]*?\}','';
$result = $result -replace '(?i-mn).*protected override void OnConfiguring[\s\S]*?\}','';
[System.IO.File]::WriteAllText($contextPath, $result)

# ./genModel.ps1 -connection "Server=.;Database=KX13_20220324;Integrated Security=True"