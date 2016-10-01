using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using App.Metrics.MetricData;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Utils
{
    public static class AppEnvironment
    {
        //TODO: AH - Inject Logger
        private static readonly ILogger log = new LoggerFactory().CreateLogger(typeof(AppEnvironment));

        public static IEnumerable<EnvironmentEntry> Current
        {
            get
            {
                yield return new EnvironmentEntry("MachineName", Environment.MachineName);
                yield return new EnvironmentEntry("ProcessName", SafeGetString(() => Process.GetCurrentProcess().ProcessName));
                yield return new EnvironmentEntry("OS", RuntimeEnvironment.OperatingSystem);
                yield return new EnvironmentEntry("OSVersion", RuntimeEnvironment.OperatingSystemVersion);
                yield return new EnvironmentEntry("CPUCount", Environment.ProcessorCount.ToString());
                yield return new EnvironmentEntry("HostName", SafeGetString(Dns.GetHostName));
                yield return new EnvironmentEntry("IPAddress", SafeGetString(GetIpAddress));
                yield return new EnvironmentEntry("LocalTime", Clock.FormatTimestamp(DateTime.Now));


                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                {
                    var entryAssemblyName = entryAssembly.GetName();
                    yield return new EnvironmentEntry("EntryAssemblyName", entryAssemblyName.Name);
                    yield return new EnvironmentEntry("EntryAssemblyVersion", entryAssemblyName.Version.ToString());
                }
            }
        }

        /// <summary>
        ///     Try to resolve Asp site name without compile-time linking System.Web assembly.
        /// </summary>
        /// <returns>Site name if able to identify</returns>
        public static string ResolveAspSiteName()
        {
            const string UnknownName = "UnknownSiteName";
            try
            {
                var runtimeType = Type.GetType(
                    "System.Web.HttpRuntime, System.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, true);
                if (runtimeType != null)
                {
                    var result = runtimeType.GetProperty("AppDomainAppVirtualPath").GetValue(null) as string;
                    if (result != null)
                    {
                        result = result.Trim('/');
                        if (result != String.Empty)
                        {
                            return result;
                        }

                        log.LogDebug("HttpRuntime.AppDomainAppVirtualPath is empty, trying AppDomainAppId");

                        result = runtimeType.GetProperty("AppDomainAppId").GetValue(null) as string;
                        if (result != null)
                        {
                            result = result.Trim('/');
                            if (result != String.Empty)
                            {
                                return result;
                            }
                        }
                        else
                        {
                            log.LogWarning("Unable to find property System.Web.HttpRuntime.AppDomainAppId to resolve AspSiteName");
                        }

                        log.LogWarning("HttpRuntime.AppDomainAppId is also empty, giving up trying to find site name");
                    }
                    else
                    {
                        log.LogWarning("Unable to find property System.Web.HttpRuntime.AppDomainAppVirtualPath to resolve AspSiteName");
                    }
                }
                else
                {
                    log.LogWarning("Unable to find type System.Web.HttpRuntime to resolve $Env.AppDomainAppVirtualPath$ macro");
                }
            }
            catch (Exception e)
            {
                log.LogWarning("Unable to find type System.Web.HttpRuntime to resolve AspSiteName $Env.AppDomainAppVirtualPath$ macro", e);
            }

            return UnknownName;
        }

        private static string GetIpAddress()
        {
            var hostName = SafeGetString(Dns.GetHostName);
            try
            {
                //TODO AH -: Make async
                var ipAddress = Dns.GetHostEntryAsync(hostName).GetAwaiter().GetResult()
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                if (ipAddress != null)
                {
                    return ipAddress.ToString();
                }
                return string.Empty;
            }
            catch (SocketException x)
            {
                if (x.SocketErrorCode == SocketError.HostNotFound)
                {
                    log.LogWarning("Unable to resolve hostname " + hostName);
                    return string.Empty;
                }
                throw;
            }
        }

        private static string SafeGetString(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception x)
            {
                MetricsErrorHandler.Handle(x, "Error retrieving environment value");
                return string.Empty;
            }
        }
    }
}