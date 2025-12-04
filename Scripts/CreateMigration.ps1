dotnet clean

$MigrationName = $args[0]


dotnet ef migrations add $($MigrationName) -s WebAPi -p Infrastructure