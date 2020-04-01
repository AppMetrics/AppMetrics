Get-ChildItem .\ -Include bin,obj -Recurse | Where { $_.fullname -notlike "*node_modules*" } | foreach ($_) { remove-item $_.fullname -Force -Recurse }
dotnet build-server shutdown