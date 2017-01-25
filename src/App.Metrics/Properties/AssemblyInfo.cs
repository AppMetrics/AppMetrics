// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyProduct("App.Metrics")]
[assembly: AssemblyCompany("Allan Hardy.")]
[assembly: NeutralResourcesLanguage("en-us")]
[assembly: AssemblyCopyright("© Allan Hardy. All rights reserved.")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("7ad4f2aa-a152-4092-b79f-2648cbedffbc")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Middleware")]
[assembly: InternalsVisibleTo("App.Metrics.Formatters.Json")]
[assembly: InternalsVisibleTo("App.Metrics.Facts")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Middleware.Integration.Facts")]
[assembly: InternalsVisibleTo("App.Metrics.Benchmarks")]
[assembly: InternalsVisibleTo("App.Metrics.Reservoir.Benchmarks")]
[assembly: InternalsVisibleTo("App.Metrics.Sampling.Facts")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Reporting.InfluxDB")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Reporting.TextFile")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Reporting.Console")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Mvc")]
[assembly: InternalsVisibleTo("App.Metrics.Extensions.Reporting.InfluxDB.Facts")]
[assembly: InternalsVisibleTo("App.Metrics.Net452.Facts")]