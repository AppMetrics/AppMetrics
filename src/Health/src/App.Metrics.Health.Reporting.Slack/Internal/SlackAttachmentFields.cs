// <copyright file="SlackAttachmentFields.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health.Reporting.Slack.Internal
{
    internal class SlackAttachmentFields
    {
        public string Title { get; set; }

        public string Value { get; set; }

        public bool Short { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbUrl { get; set; }

        public string Footer { get; set; }

        public string FooterIcon { get; set; }
    }
}