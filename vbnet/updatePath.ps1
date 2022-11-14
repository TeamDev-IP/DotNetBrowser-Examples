(Get-ChildItem -Include "*.vbproj" -Recurse) | foreach {(Get-Content $_) | foreach { $_.Replace("..\..\packages", "..\..\..\packages")} | Set-Content $_}
