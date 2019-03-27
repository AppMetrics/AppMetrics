// <copyright file="SlackHealthAlerterBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Reporting.Slack;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class SlackHealthAlerterBuilderExtensions
    {
        public static IHealthBuilder ToSlack(
            this IHealthReportingBuilder healthReportingBuilder,
            Action<SlackHealthAlertOptions> optionsSetup)
        {
            var options = new SlackHealthAlertOptions();

            optionsSetup(options);

            healthReportingBuilder.Using(new SlackIncomingWebHookHealthAlerter(options));

            return healthReportingBuilder.Builder;
        }

        public static IHealthBuilder ToSlack(
            this IHealthReportingBuilder healthReportingBuilder,
            SlackHealthAlertOptions options)
        {
            healthReportingBuilder.Using(new SlackIncomingWebHookHealthAlerter(options));

            return healthReportingBuilder.Builder;
        }
    }
}