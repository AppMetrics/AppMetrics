#addin Cake.Coveralls
#addin Cake.ReSharperReports

#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=ReSharperReports"
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
#tool "nuget:?package=coveralls.io"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target                      = Argument("target", "Default");
var configuration               = HasArgument("Configuration") ? Argument<string>("Configuration") :
                                  EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") : "Release";
var skipOpenCover               = Argument("SkipCoverage", false);
var skipReSharperCodeInspect    = Argument("SkipCodeInspect", false) || !IsRunningOnWindows();
var preReleaseSuffix            = HasArgument("PreReleaseSuffix") ? Argument<string>("PreReleaseSuffix") :
	                              (AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag) ? null :
                                  EnvironmentVariable("PreReleaseSuffix") != null ? EnvironmentVariable("PreReleaseSuffix") : "ci";
var buildNumber                 = HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
                                  AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
                                  TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber :
                                  EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) : 0;

//////////////////////////////////////////////////////////////////////
// DEFINE FILES & DIRECTORIES
//////////////////////////////////////////////////////////////////////
var packDirs                    = new [] { Directory("./src/App.Metrics"), Directory("./src/App.Metrics.Concurrency"), Directory("./src/App.Metrics.Extensions.Middleware"), Directory("./src/App.Metrics.Extensions.Mvc"), Directory("./src/App.Metrics.Formatters.Json") };
var artifactsDir                = (DirectoryPath) Directory("./artifacts");
var testResultsDir              = (DirectoryPath) artifactsDir.Combine("test-results");
var coverageResultsDir          = (DirectoryPath) artifactsDir.Combine("coverage");
var reSharperReportsDir         = (DirectoryPath) artifactsDir.Combine("resharper-reports");
var testCoverageOutputFilePath  = coverageResultsDir.CombineWithFilePath("OpenCover.xml");
var packagesDir                 = artifactsDir.Combine("packages");
var resharperSettings			= "./AppMetrics.sln.DotSettings";
var inspectCodeXml				= string.Format("{0}/inspectCode.xml", reSharperReportsDir);
var inspectCodeHtml				= string.Format("{0}/inspectCode.html", reSharperReportsDir);
var solutionFile				= "./AppMetrics.sln";

//////////////////////////////////////////////////////////////////////
// DEFINE PARAMS
//////////////////////////////////////////////////////////////////////
var coverallsToken              = Context.EnvironmentVariable("COVERALLS_REPO_TOKEN");
var openCoverFilter				= "+[App.Metrics*]* -[xunit.*]* -[*.Facts]*";
var openCoverExcludeFile        = "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs";
var excludeFromCoverage			= "*.AppMetricsExcludeFromCodeCoverage*";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);    
});

Task("Restore")
    .IsDependentOn("Clean")    
    .Does(() =>
{
    DotNetCoreRestore("./", new DotNetCoreRestoreSettings
    {        
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    });
});

Task("Build")    
    .IsDependentOn("Restore")
    .Does(() =>
{
    var projects = GetFiles("./**/project.json");
    
    foreach(var project in projects)
    {
        DotNetCoreBuild(project.GetDirectory().FullPath, new DotNetCoreBuildSettings {
            Configuration = configuration
        });
    }    
});

Task("Pack")
    .IsDependentOn("Restore")    
    .IsDependentOn("Clean")
    .Does(() =>
{
    string versionSuffix = null;
    if (!string.IsNullOrEmpty(preReleaseSuffix))
    {
        versionSuffix = preReleaseSuffix + "-" + buildNumber.ToString("D4");
    }
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = packagesDir,
        VersionSuffix = versionSuffix
    };
    
    foreach(var packDir in packDirs)
    {
        DotNetCorePack(packDir, settings);
    }    
});

Task("RunInspectCode")
	.WithCriteria(() => !skipReSharperCodeInspect)
    .Does(() =>
{
	InspectCode(solutionFile, new InspectCodeSettings { SolutionWideAnalysis = true, Profile = resharperSettings, OutputFile = inspectCodeXml });
    ReSharperReports(inspectCodeXml, inspectCodeHtml);
});

Task("RunTests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./test/**/project.json");

    CreateDirectory(testResultsDir);
    CreateDirectory(coverageResultsDir);

    Context.Information("Found " + projects.Count() + " projects");

    foreach (var project in projects)
    {
		var folderName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(project.ToString())).Name;
        
		if (IsRunningOnWindows())
        {			
			Action<ICakeContext> testAction = tool => {

                    tool.DotNetCoreTest(project.GetDirectory().FullPath, new DotNetCoreTestSettings {
                        Configuration = configuration,
                        NoBuild = true,
                        Verbose = false,
                        ArgumentCustomization = args =>
                            args.Append("-xml").Append(testResultsDir.CombineWithFilePath(folderName) + ".xml")
                    });
                };

                if (!skipOpenCover) {
                    OpenCover(testAction,
                        testCoverageOutputFilePath,
                        new OpenCoverSettings { 
							ReturnTargetCodeOffset = 1,
							SkipAutoProps = true,
							Register = "user",
							OldStyle = true,
							MergeOutput = true,
							ArgumentCustomization = args => args.Append(@"-safemode:off")
                        }
                        .WithFilter(openCoverFilter)
                        .ExcludeByAttribute(excludeFromCoverage)
                        .ExcludeByFile(openCoverExcludeFile));
                } 
                else 
                {
                    testAction(Context);
                }
        }
        else if (!folderName.Contains("Net452"))
        {
            var settings = new DotNetCoreTestSettings
            {
                Configuration = configuration, 
                Framework = "netcoreapp1.1"
            };

            DotNetCoreTest(project.GetDirectory().FullPath, settings);
        }
    }    
});

Task("HtmlCoverageReport")    
    .WithCriteria(() => FileExists(testCoverageOutputFilePath))
    .WithCriteria(() => BuildSystem.IsLocalBuild)
    .IsDependentOn("RunTests")
    .Does(() => 
{
    ReportGenerator(testCoverageOutputFilePath, coverageResultsDir);
});

Task("PublishCoverage")    
    .WithCriteria(() => FileExists(testCoverageOutputFilePath))
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .WithCriteria(() => !string.IsNullOrEmpty(coverallsToken))
    .IsDependentOn("RunTests")
    .Does(() => 
{
    CoverallsIo(testCoverageOutputFilePath, new CoverallsIoSettings()
    {
        RepoToken = coverallsToken
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")	
    .IsDependentOn("Build")
    .IsDependentOn("RunTests")
    .IsDependentOn("Pack")
	.IsDependentOn("RunInspectCode")	
    .IsDependentOn("HtmlCoverageReport");

Task("AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("RunTests")
    .IsDependentOn("Pack")
	.IsDependentOn("RunInspectCode")	
    .IsDependentOn("PublishCoverage");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);