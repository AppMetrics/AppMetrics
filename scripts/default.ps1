properties {
    $configuration = $configuration
    $runtimeVersion = "0.0.0"
}

Include ".\core\utils.ps1"

$projectFileName = "project.json"
$solutionRoot = (get-item $PSScriptRoot).parent.fullname
$jsonlib= "$solutionRoot\packages\Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"
$artifactsRoot = "$solutionRoot\artifacts"
$artifactsBuildRoot = "$artifactsRoot\build"
$artifactsTestRoot = "$artifactsRoot\test"
$artifactsPackagesRoot = "$artifactsRoot\packages"
$srcRoot = "$solutionRoot\src"
$testsRoot = "$solutionRoot\test"
$globalFilePath = "$solutionRoot\global.json"
$appProjects = Get-ChildItem "$srcRoot\**\$projectFileName" | foreach { $_.FullName }
$testProjects = Get-ChildItem "$testsRoot\**\$projectFileName" | foreach { $_.FullName }
$packableProjectDirectories = @("$srcRoot\App.Metrics", "$srcRoot\App.Metrics.Concurrency", "$srcRoot\App.Metrics.Extensions.Middleware", "$srcRoot\App.Metrics.Formatters.Json", "$srcRoot\App.Metrics.Extensions.Reporting.TextFile","$srcRoot\App.Metrics.Extensions.Reporting.Console","$srcRoot\App.Metrics.Extensions.Reporting.InfluxDB")

task default -depends PatchProject, TestParams, Setup, Build, RunTests, Pack

task TestParams { 
	Assert ($configuration -ne $null) '$configuration should not be null'
}

task Setup {
    if((test-path $globalFilePath) -eq $false) {
        throw "global.json doesn't exists. globalFilePath: $globalFilePath"
    }
	
	$globalSettingsObj = (get-content $globalFilePath) -join "`n" | ConvertFrom-Json
	$hasSdk = $globalSettingsObj | Get-Member | 
		where { $_.MemberType -eq "NoteProperty" } | 
		Test-Any { $_.Name -eq "sdk" }
		
	if($hasSdk) {
		$hasVersion = $globalSettingsObj.sdk | Get-Member | 
			where { $_.MemberType -eq "NoteProperty" } | 
			Test-Any { $_.Name -eq "version" }
			
		if($hasVersion) {
            $script:runtimeVersion = $globalSettingsObj.sdk.version;
			Write-Host("Using: $script:runtimeVersion")
		}
		else {
			throw "global.json doesn't contain sdk version."
		}
	}
	
}

task Build -depends Restore, Clean {
	
	$appProjects | foreach {
		dotnet build "$_" --configuration $configuration
    }
    
    $testProjects | foreach {
		dotnet build "$_" --configuration $configuration
    }
}

task RunTests -depends Restore, Clean {
	$testProjects | foreach {
		Write-Output "Running tests for '$_'"
		dotnet test  "$_"  
	}
}

task PatchProject {
    if (Test-Path Env:\APPVEYOR_BUILD_NUMBER) {
        $buildNumber = [int]$Env:APPVEYOR_BUILD_NUMBER
        $paddedBuildNumber = $buildnumber.ToString().PadLeft(5,'0')
        Write-Host "Using AppVeyor build number"
        Write-Host $paddedBuildNumber
        
        [Reflection.Assembly]::LoadFile($jsonlib)
        
        $packableProjectDirectories | foreach {
            Write-Host "Patching project.json"
            
            $json = (Get-Content "$_\project.json" | Out-String)
            $config = [Newtonsoft.Json.Linq.JObject]::Parse($json)
            $version = $config.Item("version").ToString()
            $config.Item("version") = New-Object -TypeName Newtonsoft.Json.Linq.JValue -ArgumentList "$version-build$paddedBuildNumber"

            $config.ToString() | Out-File "$_\project.json"
            
            $after = (Get-Content "$_\project.json" | Out-String)
            Write-Host $after
        }
    }
}

task Pack -depends Restore, Clean {
	$packableProjectDirectories | foreach {
		dotnet pack "$_" --configuration $configuration -o "$artifactsPackagesRoot"
	}
}

task Restore {
	@($srcRoot, $testsRoot) | foreach {
        Write-Output "Restoring for '$_'"
        dotnet restore "$_"
    }
}

task Clean {
    $directories = $(Get-ChildItem "$solutionRoot\artifacts*"),`
        $(Get-ChildItem "$solutionRoot\**\**\bin")
		
    $directories | foreach ($_) { Remove-Item $_.FullName -Force -Recurse }
}