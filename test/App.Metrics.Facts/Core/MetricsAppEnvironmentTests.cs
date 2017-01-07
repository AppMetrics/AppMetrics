using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.PlatformAbstractions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsAppEnvironmentTests
    {
        [Fact]
        public void can_get_required_env_params()
        {
            var assemblyName = Assembly.GetEntryAssembly().GetName();

            var appEnv = new MetricsAppEnvironment(PlatformServices.Default.Application);

            appEnv.ApplicationName.Should().Be(assemblyName.Name);
            appEnv.ApplicationVersion.Should().Be(assemblyName.Version.ToString());
            appEnv.RuntimeFramework.Should().Be(PlatformServices.Default.Application.RuntimeFramework.Identifier);
            appEnv.RuntimeFrameworkVersion.Should().Be(PlatformServices.Default.Application.RuntimeFramework.Version.ToString());
        }
    }
}