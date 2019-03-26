$CakeVersion = "0.30.0"
$DotNetVersion = "2.2.104";
$DotNetInstallerUri = "https://dot.net/v1/dotnet-install.ps1";

# Make sure tools folder exists
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
$ToolPath = Join-Path $PSScriptRoot "tools"
if (!(Test-Path $ToolPath)) {
    Write-Verbose "Creating tools directory..."
    New-Item -Path $ToolPath -Type directory | out-null
}

# Attempt to set highest encryption available for SecurityProtocol.
# PowerShell will not set this by default (until maybe .NET 4.6.x). This
# will typically produce a message for PowerShell v2 (just an info
# message though)
try {
    # Set TLS 1.2 (3072), then TLS 1.1 (768), then TLS 1.0 (192), finally SSL 3.0 (48)
    # Use integers because the enumeration values for TLS 1.2 and TLS 1.1 won't
    # exist in .NET 4.0, even though they are addressable if .NET 4.5+ is
    # installed (.NET 4.5 is an in-place upgrade).
    [System.Net.ServicePointManager]::SecurityProtocol = 3072 -bor 768 -bor 192 -bor 48
  } catch {
    Write-Output 'Unable to set PowerShell to use TLS 1.2 and TLS 1.1 due to old .NET Framework installed. If you see underlying connection closed or trust errors, you may need to upgrade to .NET Framework 4.5+ and PowerShell v3'
  }

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################
$sdk = $args[0]

# only install sdk if -sdk was used
if ($sdk -eq "-sdk")
{
    Write-Output("Using .NET SDK v" + $DotNetVersion)

    Function Remove-PathVariable([string]$VariableToRemove)
    {
        $path = [Environment]::GetEnvironmentVariable("PATH", "User")
        if ($path -ne $null)
        {
            $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
            [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
        }

        $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
        if ($path -ne $null)
        {
            $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
            [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
        }
    }

    # Get .NET Core CLI path if installed.
    $FoundDotNetCliVersion = $null;
    if (Get-Command dotnet -ErrorAction SilentlyContinue) {
        $FoundDotNetCliVersion = dotnet --version;
    }

    if($FoundDotNetCliVersion -ne $DotNetVersion) {
        $InstallPath = Join-Path $PSScriptRoot ".dotnet"
        if (!(Test-Path $InstallPath)) {
            mkdir -Force $InstallPath | Out-Null;
        }
        (New-Object System.Net.WebClient).DownloadFile($DotNetInstallerUri, "$InstallPath\dotnet-install.ps1");
        & $InstallPath\dotnet-install.ps1 -Channel $DotNetChannel -Version $DotNetVersion -InstallDir $InstallPath;

        Remove-PathVariable "$InstallPath"
        $env:PATH = "$InstallPath;$env:PATH"
    }

    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
    $env:DOTNET_CLI_TELEMETRY_OPTOUT=1
}
else {
    Write-Output("Using global .NET SDK")
}

###########################################################################
# INSTALL CAKE
###########################################################################

# Make sure Cake has been installed.
$CakePath = Join-Path $ToolPath ".store\cake.tool\$CakeVersion"
$CakeExePath = (Get-ChildItem -Path $ToolPath -Filter "dotnet-cake*" -File| ForEach-Object FullName | Select-Object -First 1)

if ((!(Test-Path -Path $CakePath -PathType Container)) -or (!(Test-Path $CakeExePath -PathType Leaf))) {
    & dotnet tool install --tool-path $ToolPath --version $CakeVersion Cake.Tool
    if ($LASTEXITCODE -ne 0)
    {
        'Failed to install cake'
        exit 1
    }
    $CakeExePath = (Get-ChildItem -Path $ToolPath -Filter "dotnet-cake*" -File| ForEach-Object FullName | Select-Object -First 1)
}

###########################################################################
# INSTALL SignTool
###########################################################################

# Make sure Cake has been installed.
$SignClientPath = Join-Path $ToolPath ".store\SignClient"
$SignClientExePath = (Get-ChildItem -Path $ToolPath -Filter "SignClient*" -File| ForEach-Object FullName | Select-Object -First 1)

if ((!(Test-Path -Path $SignClientPath -PathType Container)) -or (!(Test-Path $SignClientExePath -PathType Leaf))) {
    & dotnet tool install --tool-path $ToolPath SignClient
    if ($LASTEXITCODE -ne 0)
    {
        'Failed to install SignClient'
        exit 1
    }
    $SignClientExePath = (Get-ChildItem -Path $ToolPath -Filter "signtool*" -File| ForEach-Object FullName | Select-Object -First 1)
}

###########################################################################
# RUN BUILD SCRIPT
###########################################################################
& "$CakeExePath" ./build.cake --bootstrap
if ($LASTEXITCODE -eq 0)
{
    & "$CakeExePath" ./build.cake $args
}
exit $LASTEXITCODE