#tool "nuget:https://api.nuget.org/v3/index.json?package=GitVersion.CommandLine"

var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");
var sign            = Argument<bool>("sign", false);

// call e.g. .\build.ps1 --release=1.0.0 --pre=beta1
// call e.g. .\build.ps1 --release=1.0.0
 var versionOverride = Argument<string>("release", "");
 var suffixOverride  = Argument<string>("pre", "");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var packPaths        = new [] {
					                Directory("./src/App.Metrics.AppNetCore"),
									Directory("./src/App.Metrics.Extensions.Configuration"),
									Directory("./src/App.Metrics.Extensions.DependencyInjection"),
									Directory("./src/App.Metrics.Extensions.Hosting"),
									Directory("./src/App.Metrics.Health.AppNetCore"),
									Directory("./src/App.Metrics.Health.Extensions.Configuration"),
									Directory("./src/App.Metrics.Health.Extensions.DependencyInjection"),
									Directory("./src/App.Metrics.Health.Extensions.Hosting"),
									Directory("./src/App.Metrics.Extensions.HealthChecks")
						};
var buildArtifacts      = Directory("./artifacts");

var isAppVeyor          = AppVeyor.IsRunningOnAppVeyor;
var isVsts              = TFBuild.IsRunningOnVSTS;
var isWindows           = IsRunningOnWindows();

///////////////////////////////////////////////////////////////////////////////
// Setup
///////////////////////////////////////////////////////////////////////////////
DotNetCoreMSBuildSettings msBuildSettings;
VersionInfo versions;

class VersionInfo
{
    public string AssemblyVersion { get; set; }
    public string VersionSuffix { get; set; }
    public string FileVersion { get; set; }
    public string InformationalVersion { get; set; }
    public string BranchName { get; set; }
    public string PreReleaseLabel { get; set; }
}

Setup(context =>
{
    // only calculate versions if on Windows
    // due to problems with GitVersion in the current setup - but also since Windows is our release platform anyways
    if (isWindows)
    {
        var gitVersions = Context.GitVersion();
        
        versions = new VersionInfo
        {
            InformationalVersion = gitVersions.InformationalVersion,
            BranchName = gitVersions.BranchName,
            PreReleaseLabel = gitVersions.PreReleaseLabel
        };

        // explicit version has been passed in as argument
        if (!string.IsNullOrEmpty(versionOverride))
        {
            versions.AssemblyVersion = versionOverride;
            versions.FileVersion = versionOverride;

            if (!string.IsNullOrEmpty(suffixOverride))
            {
                versions.VersionSuffix = suffixOverride;
            }
        }
        else
        {
            versions.AssemblyVersion = gitVersions.AssemblySemVer;
            versions.FileVersion = gitVersions.AssemblySemVer;

            if (!string.IsNullOrEmpty(versions.PreReleaseLabel))
            {
                versions.VersionSuffix = gitVersions.PreReleaseLabel + gitVersions.CommitsSinceVersionSourcePadded;      
            }
            
        }

        Information("branch            : " + versions.BranchName);
        Information("pre-release label : " + versions.PreReleaseLabel);
        Information("version           : " + versions.AssemblyVersion);
        Information("version suffix    : " + versions.VersionSuffix);
        Information("informational     : " + versions.InformationalVersion);
        
        msBuildSettings = GetMSBuildSettings();
    }
    else
    {
        Information("Skipping version calculation because not on Windows.");
        msBuildSettings = null;
    }
});

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
});

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings 
    {
        Configuration = configuration,
        MSBuildSettings = msBuildSettings
    };

    // building projects
    if (!isWindows)
    {
        Information("Not running on Windows - building source for netstandard2.0");
        settings.Framework = "netstandard2.0";
    }

    var projects = GetFiles("./src/**/*.csproj");
    foreach(var project in projects)
	{
	    DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    }

    // building tests
    if (!isWindows)
    {
        Information("Not running on Windows - building tests for netcoreapp2.1");
        settings.Framework = "netcoreapp2.1";
    }

    var tests = GetFiles("./test/**/*.csproj");
    foreach(var test in tests)
	{
	    DotNetCoreBuild(test.GetDirectory().FullPath, settings);
    }

    // authenticode sign assemblies
    if (sign)
    {
        var dlls = GetFiles("./src/bin/release/**/AppMetrics*.dll");
        foreach(var dll in dlls)
        {
            SignFile(dll.FullPath);
        }
    }
});

///////////////////////////////////////////////////////////////////////////////
// Test
///////////////////////////////////////////////////////////////////////////////
Task("Test")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    if (!isWindows)
    {
        Information("Not running on Windows - skipping tests for .NET Framework");
        settings.Framework = "netcoreapp2.1";
    }

    var projects = GetFiles("./test/**/*.csproj");
    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath, settings);
    }
});

///////////////////////////////////////////////////////////////////////////////
// Pack
///////////////////////////////////////////////////////////////////////////////
Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .Does(() =>
{
    if (SkipPack()) return;

    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        MSBuildSettings = msBuildSettings,
        OutputDirectory = buildArtifacts + Directory("packages"),
        NoBuild = true
    };

    foreach(var packPath in packPaths)
    {
        DotNetCorePack(packPath, settings);
    }

    if (sign)
    {
        var packages = GetFiles("./artifacts/**/*.nupkg");
        foreach(var package in packages)
        {
            SignFile(package.FullPath);
        }
    }
});


///////////////////////////////////////////////////////////////////////////////
// Helpers
///////////////////////////////////////////////////////////////////////////////
private bool SkipPack()
{
    if (!isWindows)
    {
        Information("Skipping pack because not on Windows.");
        return true;
    }
    
    if (String.IsNullOrEmpty(versions.PreReleaseLabel) && versions.BranchName == "dev") 
    {
        Information("Skipping pack of release version, because on dev.");
        return true;
    }

    if (versions.PreReleaseLabel == "PullRequest")
    {
        Information("Skipping pack for pull requests.");
        return true;
    }

    return false;
}

private DotNetCoreMSBuildSettings GetMSBuildSettings()
{
    var settings = new DotNetCoreMSBuildSettings();

    settings.WithProperty("AssemblyVersion", versions.AssemblyVersion);
    settings.WithProperty("VersionPrefix", versions.AssemblyVersion);
    settings.WithProperty("FileVersion", versions.FileVersion);
    settings.WithProperty("InformationalVersion", versions.InformationalVersion);
    
    if (!String.IsNullOrEmpty(versions.VersionSuffix))
    {
        settings.WithProperty("VersionSuffix", versions.VersionSuffix);
    }

    return settings;
}

private void SignFile(string fileName)
{
    var signClientConfig = EnvironmentVariable("SignClientConfig") ?? "";
    var signClientSecret = EnvironmentVariable("SignClientSecret") ?? "";

    if (signClientConfig == "")
    {
        throw new Exception("SignClientConfig environment variable is missing. Aborting.");
    }

    if (signClientSecret == "")
    {
        throw new Exception("SignClientSecret environment variable is missing. Aborting.");
    }

    Information("  Signing " + fileName);

    var success = StartProcess("./tools/signclient", new ProcessSettings {
        Arguments = new ProcessArgumentBuilder()
            .Append("sign")
            .Append($"-c {signClientConfig}")
            .Append($"-i {fileName}")
            .Append($"-s {signClientSecret}")
            .Append(@"-n 'AppMetrics'")
        });

    if (success == 0)
    {
        Information("  success.");
    }
    else
    {
        throw new Exception("Error signing " + fileName);
    }
    
}

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("Pack");

RunTarget(target);