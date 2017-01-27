properties {
    $configuration = $configuration
    $runtimeVersion = "0.0.0"
}

Include ".\core\utils.ps1"

$projectFileName = "project.json"
$solutionRoot = (get-item $PSScriptRoot).parent.fullname
$jsonlib = "$solutionRoot\packages\Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"
$codeCoverage = "$solutionRoot\packages\OpenCover*\tools\OpenCover.Console.exe"
$coveralls = "$solutionRoot\packages\coveralls.net*\tools\csmacnz.coveralls.exe"
$artifactsRoot = "$solutionRoot\artifacts"
$artifactsBuildRoot = "$artifactsRoot\build"
$artifactsTestRoot = "$artifactsRoot\test"
$artifactsPackagesRoot = "$artifactsRoot\packages"
$artifactsCodeCoverageRoot = "$artifactsRoot\coverage"
$srcRoot = "$solutionRoot\src"
$testsRoot = "$solutionRoot\test"
$globalFilePath = "$solutionRoot\global.json"
$appProjects = Get-ChildItem "$srcRoot\**\$projectFileName" | foreach { $_.FullName }
$testProjects = Get-ChildItem "$testsRoot\**\$projectFileName" | foreach { $_.FullName }
$packableProjectDirectories = @("$srcRoot\App.Metrics",								
								"$srcRoot\App.Metrics.Concurrency", 
								"$srcRoot\App.Metrics.Extensions.Middleware", 
								"$srcRoot\App.Metrics.Extensions.Mvc",
								"$srcRoot\App.Metrics.Formatters.Json")

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
	$script:buildSuccess = $true
	$appProjects | foreach {
		try {
			exec { & dotnet build "$_" --configuration $configuration }
		}
		catch {
			$script:buildSuccess = $false
			continue			
		}		
    }
		
	if ($script:buildSuccess) {
		$testProjects | foreach {
			dotnet build "$_" --configuration $configuration
		}   
	}	
}

task RunTests -depends Restore, Clean {
	
	if ($script:buildSuccess) {
		New-Item $artifactsCodeCoverageRoot -type directory -force	
		$success = $true
		
		$testProjects | foreach {
			Write-Output "Running tests for '$_'"	
			try {
				exec { & $codeCoverage "-target:C:\Program Files\dotnet\dotnet.exe" -targetargs:" test -c Release $_" -mergeoutput -hideskipped:All -output:"$artifactsCodeCoverageRoot\coverage.xml" -oldStyle -filter:"+[App.Metrics*]* -[xunit.*]* -[*.Facts]*" -excludebyattribute:"*.AppMetricsExcludeFromCodeCoverage*" -excludebyfile:"*\*Designer.cs;*\*.g.cs;*\*.g.i.cs" -register:user -skipautoprops -safemode:off -returntargetcode }
			}
			catch {
				$script:buildSuccess = $false
			}					
		}
			
		if (-not $script:buildSuccess) {
			throw 'tests failed'
		}
		
		if (-not (Test-Path env:COVERALLS_REPO_TOKEN))
		{
			Write-Output "Skipping code coverage publish"		
		}
		else 
		{		
			Write-Output "Publishing code coverage"
			exec { & $coveralls --opencover -i "$artifactsCodeCoverageRoot\coverage.xml" --repoToken $env:COVERALLS_REPO_TOKEN --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_JOB_ID }
		}
	} 
	else {
		Write-Output "Skipping tests since build failed"		
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
	if ($script:buildSuccess) {
		$packableProjectDirectories | foreach {
			dotnet pack "$_" --configuration $configuration -o "$artifactsPackagesRoot"
		}
	}
	else {
		Write-Output "Skipping Pack since build failed"	
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
