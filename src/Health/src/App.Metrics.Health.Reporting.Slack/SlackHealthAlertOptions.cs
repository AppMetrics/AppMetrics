// <copyright file="SlackHealthAlertOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Reporting.Slack
{
    public class SlackHealthAlertOptions
    {
        public bool AlertOnDegradedChecks { get; set; } = true;

        public string Channel { get; set; }

        public string EmojiIcon { get; set; }

        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets the health status reporting interval.
        /// </summary>
        /// <remarks>
        ///     If not set reporting interval will be set to the <see cref="HealthConstants.Reporting.DefaultReportInterval" />.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting health status.
        /// </value>
        public TimeSpan ReportInterval { get; set; }

        /// <summary>
        ///     Gets or sets the number of report runs before re-alerting checks that have re-failed. If set to 0, failed checks will only be reported once until healthy again.
        /// </summary>
        /// <remarks>
        ///     If not set number of runs will be set to the <see cref="HealthConstants.Reporting.DefaultNumberOfRunsBeforeReAlerting" />.
        /// </remarks>
        public int RunsBeforeReportExistingFailures { get; set; } = HealthConstants.Reporting.DefaultNumberOfRunsBeforeReAlerting;

        public string Username { get; set; }

        public string WebhookUrl { get; set; }
    }
}