dotnet clean

$MigrationName = $args[0]


dotnet ef migrations add $($MigrationName) -s src/WebAPi -p src/Infrastructure