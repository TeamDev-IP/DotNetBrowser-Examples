param([String]$oldVersion = "1.21.3", [String]$newVersion = "1.21.4", [String]$oldVersionFull = "1.21.3.0", [String]$newVersionFull = "1.21.4.0")
$oldVersionProjectName = "DotNetBrowser." + $oldVersion;
$newVersionProjectName = "DotNetBrowser." + $newVersion;

(Get-ChildItem -Include "*.csproj" -Recurse) | foreach {(Get-Content $_) | foreach { $_ -replace "Version=$($oldVersionFull)", "Version=$($newVersionFull)"} | Set-Content $_}

(Get-ChildItem -Include "*.csproj" -Recurse) | foreach {(Get-Content $_) | foreach { $_ -replace $oldVersionProjectName, $newVersionProjectName} | Set-Content $_}

(Get-ChildItem -Include "packages.config" -Recurse) | foreach {(Get-Content $_) | foreach { $_ -replace "version=`"$($oldVersion)", "version=`"$($newVersion)"} | Set-Content $_}
