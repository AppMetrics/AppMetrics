// <copyright file="TemplatePartExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing.Template
// ReSharper restore CheckNamespace
{
    public static class TemplatePartExtensions
    {
        public static string ToTemplatePartString(this TemplatePart templatePart)
        {
            if (templatePart.IsParameter)
            {
                return
                    ("{" + (templatePart.IsCatchAll ? "*" : string.Empty) + templatePart.Name + (templatePart.IsOptional ? "?" : string.Empty) + "}").
                    ToLower();
            }

            return templatePart.Text.ToLower();
        }

        public static string ToTemplateSegmentString(this TemplateSegment templateSegment) =>
            string.Join(string.Empty, templateSegment.Parts.Select(ToTemplatePartString));

        public static string ToTemplateString(
            this Route templateRoute,
            string controller,
            string action,
            string version) =>
            string.Join(
                      "/",
                      templateRoute.ParsedTemplate.Segments
                                   .Select(s => s.ToTemplateSegmentString()))
                  .Replace("{controller}", controller).ToLower()
                  .Replace("{action}", action).ToLower()
                  .Replace("{version}", version).ToLower();
    }
}