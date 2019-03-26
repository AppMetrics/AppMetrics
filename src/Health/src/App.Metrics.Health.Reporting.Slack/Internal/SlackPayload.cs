// <copyright file="SlackPayload.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health.Reporting.Slack.Internal
{
    internal class SlackPayload
    {
        public SlackPayload()
        {
            Attachments = new List<SlackAttachment>();
        }

        public string Text { get; set; }

        public string Channel { get; set; }

        public string UserName { get; set; }

        public string IconEmoji { get; set; }

        public List<SlackAttachment> Attachments { get; set; }
    }
}