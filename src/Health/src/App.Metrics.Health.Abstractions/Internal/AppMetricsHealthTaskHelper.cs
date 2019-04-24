// <copyright file="AppMetricsHealthTaskHelper.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

#if !NETSTANDARD1_6

using System.Threading.Tasks;

namespace App.Metrics.Health.Internal
{
    public static class AppMetricsHealthTaskHelper
    {
        private static Task _completedTask;

        private struct VoidTaskResult { }

        public static Task CompletedTask()
        {
            return _completedTask ?? (_completedTask = Task.FromResult(default(VoidTaskResult)));
        }
    }
}
#endif