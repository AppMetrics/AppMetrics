// <copyright file="SlackAttachment.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health.Reporting.Slack.Internal
{
    internal class SlackAttachment
    {
        public SlackAttachment()
        {
            Fields = new List<SlackAttachmentFields>();
        }

        public string Fallback { get; set; }

        public string Color { get; set; }

        public string PreText { get; set; }

        public string AuthorName { get; set; }

        public string AuthorLink { get; set; }

        public string AuthorIcon { get; set; }

        public string Title { get; set; }

        public string TitleLink { get; set; }

        public string Text { get; set; }

        public double Ts { get; set; }

        public List<SlackAttachmentFields> Fields { get; set; }
    }
}