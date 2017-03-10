#addin Cake.Coveralls
#addin Cake.ReSharperReports
#addin Cake.Incubator
#addin Cake.Compression

#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=ReSharperReports"
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
#tool "nuget:?package=coveralls.io"
#tool "nuget:?package=SharpZipLib"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target                      = Argument("target", "Default");
var configuration               = HasArgument("BuildConfiguration") ? Argument<string>("BuildConfiguration") :
                                  EnvironmentVariable("BuildConfiguration") != null ? EnvironmentVariable("BuildConfiguration") : "Release";
var skipCoverage                = HasArgument("SkipCoverage") ? Argument<bool>("SkipCoverage") :
                                  EnvironmentVariable("SkipCoverage") != null ? bool.Parse(EnvironmentVariable("SkipCoverage")) : false;
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
var testCoverageOutputFilePath  = coverageResultsDir.CombineWithFilePath("coverage.xml");
var htmlCoverageReport			= coverageResultsDir.FullPath + "/coverage.html";
var mergedCoverageSnapshots		= coverageResultsDir.FullPath + "/coverage.dcvr";
var xmlCoverageReport			= coverageResultsDir.FullPath + "/coverage.xml";
var packagesDir                 = artifactsDir.Combine("packages");
var resharperSettings			= "./AppMetrics.sln.DotSettings";
var inspectCodeXml				= string.Format("{0}/inspectCode.xml", reSharperReportsDir);
var inspectCodeHtml				= string.Format("{0}/inspectCode.html", reSharperReportsDir);
var solutionFile				= "./AppMetrics.sln";
var solution					= ParseSolution(new FilePath(solutionFile));

//////////////////////////////////////////////////////////////////////
// DEFINE PARAMS
//////////////////////////////////////////////////////////////////////
var coverallsToken              = Context.EnvironmentVariable("COVERALLS_REPO_TOKEN");
var coverIncludeFilter			= "+:App.Metrics*";
var coverExcludeFilter			= "-:*.Facts";
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
	var projects = solution.GetProjects();
    
    foreach(var project in projects)
    {		
        DotNetCoreBuild(project.Path.ToString(), new DotNetCoreBuildSettings {
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
    var projects = GetFiles("./test/**/*.csproj");

    CreateDirectory(testResultsDir);
    CreateDirectory(coverageResultsDir);

    Context.Information("Found " + projects.Count() + " projects");

    foreach (var project in projects)
    {		
		var folderName = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(project.ToString())).Name;				
        
		if (IsRunningOnWindows())
        {			
			Action<ICakeContext> testAction = tool => {

                    tool.DotNetCoreTest(project.ToString(), new DotNetCoreTestSettings {
                        Configuration = configuration,
                        NoBuild = true,
                        Verbose = false,
                        ArgumentCustomization = args =>
                            args.Append("--logger:trx")
                    });					
                };

                if (!skipCoverage) {                    
					var dotCoverSettings = new DotCoverCoverSettings {
								ArgumentCustomization = args => args.Append(@"/HideAutoProperties")
									.Append(@"/AttributeFilters=" + excludeFromCoverage)
									.Append(@"/ReturnTargetExitCode")								
						  };
					
					dotCoverSettings.WithFilter(coverIncludeFilter).WithFilter(coverExcludeFilter);

					var coverageFile = coverageResultsDir.FullPath + folderName + ".dcvr";					

					DotCoverCover(testAction,
						  new FilePath(coverageFile), dotCoverSettings);	

					MoveFiles(coverageFile, coverageResultsDir);
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

	if(!skipCoverage && IsRunningOnWindows()) 
	{
		var snapshots = GetFiles(coverageResultsDir.FullPath + "/*.dcvr");		

		if (snapshots != null && snapshots.Any()) 
		{
			DotCoverMerge(snapshots,
				new FilePath(mergedCoverageSnapshots), null);			

			DotCoverReport(
				mergedCoverageSnapshots,
				xmlCoverageReport,
				new DotCoverReportSettings {
				  ReportType = DotCoverReportType.XML
			});
		}
	}
});

Task("HtmlCoverageReport")    
    .WithCriteria(() => FileExists(testCoverageOutputFilePath))    
    .IsDependentOn("RunTests")
    .Does(() => 
{
    if(!skipCoverage && IsRunningOnWindows()) 
	{
		DotCoverReport(
				mergedCoverageSnapshots,
				htmlCoverageReport,
				new DotCoverReportSettings {
				  ReportType = DotCoverReportType.HTML
			});
	}
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
	.IsDependentOn("HtmlCoverageReport")
	.IsDependentOn("RunInspectCode")	
    .IsDependentOn("PublishCoverage");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);