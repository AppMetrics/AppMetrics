using App.Metrics.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.PlatformAbstractions;
using Xunit;
#if NET452
using System.Reflection;
#endif

// ReSharper disable CheckNamespace
namespace App.Metrics.Facts.Core
{
    // ReSharper restore CheckNamespace

    public class MetricsAppEnvironmentTests
    {
#if NET452
        [Fact(Skip = "broken after vs2017 upgrade")]
#else
        [Fact]
#endif
        public void can_get_required_env_params()
        {
#if NET452
            var assemblyName = Assembly.GetEntryAssembly().GetName();
            var expectedAppName = assemblyName.Name;
            var expectedAppVersion = assemblyName.Version.ToString();
            var appEnv = new MetricsAppEnvironment(PlatformServices.Default.Application, assemblyName);
#else
            var expectedAppName = PlatformServices.Default.Application.ApplicationName;
            var expectedAppVersion = PlatformServices.Default.Application.ApplicationVersion;
            var appEnv = new MetricsAppEnvironment(PlatformServices.Default.Application);
#endif

            appEnv.ApplicationName.Should().Be(expectedAppName);
            appEnv.ApplicationVersion.Should().Be(expectedAppVersion);
            appEnv.RuntimeFramework.Should().Be(PlatformServices.Default.Application.RuntimeFramework.Identifier);
            appEnv.RuntimeFrameworkVersion.Should().Be(PlatformServices.Default.Application.RuntimeFramework.Version.ToString());
        }
    }
}