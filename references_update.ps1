Get-ChildItem *packages.config -Recurse | foreach {$_.Delete()}

Get-ChildItem -Include *.csproj -Recurse | foreach { (Get-Content $_ | Where-Object {$_ -notmatch "packages.config" }) | Set-Content $_ }

Get-ChildItem -Include *.csproj -Recurse | foreach { (Get-Content $_) | foreach { 
    $_ -replace 'packages.*\DotNetBrowser.dll', '..\Library\DotNetBrowser.dll'
} | 
set-content $_ }

Get-ChildItem -Include *.csproj -Recurse | foreach { (Get-Content $_) | foreach { 
    $_ -replace 'packages.*\DotNetBrowser.Chromium32.dll', '..\Library\DotNetBrowser.Chromium32.dll'
} | 
set-content $_ }

Get-ChildItem -Include *.csproj -Recurse | foreach { (Get-Content $_) | foreach { 
    $_ -replace 'packages.*\DotNetBrowser.Chromium64.dll', '..\Library\DotNetBrowser.Chromium64.dll'
} | 
set-content $_ }