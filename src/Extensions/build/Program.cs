using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace build
{
    class Program
    {
        private const string Prefix = "App.Metrics.Extensions";
        private const bool RequireTests = true;
        private const string ArtifactsDir = "artifacts";
        private const string Build = "build";
        private const string Test = "test";
        private const string Pack = "pack";
        
        static void Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);

            CleanArtifacts();

            app.OnExecute(() =>
            {
                Target(Build, () => 
                {
                    var solution = Directory.GetFiles(".", "*.sln", SearchOption.TopDirectoryOnly).First();

                    Run("dotnet", $"build {solution} -c Release", echoPrefix: Prefix);
                });

                Target(Test, DependsOn(Build), () => 
                {
                    try
                    {
                        var tests = Directory.GetFiles("./test", "*.csproj", SearchOption.AllDirectories);

                        foreach (var test in tests)
                        {
                            Run("dotnet", $"test {test} -c Release --no-build", echoPrefix: Prefix);
                        }    
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        if (RequireTests)
                        {
                            throw new Exception($"No tests found: {ex.Message}");
                        };
                    }
                });
                
                Target(Pack, DependsOn(Build), () => 
                {
                    var projects = Directory.GetFiles("./src", "*.csproj", SearchOption.AllDirectories);

                    foreach(var project in projects)
                    {
                        Run("dotnet", $"pack {project} -c Release -o ./{ArtifactsDir} --no-build", echoPrefix: Prefix);
                        
                        CopyArtifacts();
                    }
                });


                Target("default", DependsOn(Test, Pack));
                RunTargetsAndExit(app.RemainingArguments, logPrefix: Prefix);
            });

            app.Execute(args);
        }

        private static void CopyArtifacts()
        {
            var files = Directory.GetFiles($"./{ArtifactsDir}");

            foreach (string s in files)
            {
                var fileName = Path.GetFileName(s);
                var destFile = Path.Combine("../../nuget", fileName);
                File.Copy(s, destFile, true);
            }
        }

        private static void CleanArtifacts()
        {
            Directory.CreateDirectory($"./{ArtifactsDir}");

            foreach (var file in Directory.GetFiles($"./{ArtifactsDir}"))
            {
                File.Delete(file);
            }
        }
    }
}