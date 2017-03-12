// <copyright file="TemplateExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
namespace Microsoft.AspNetCore.Routing.Template
{
    internal static class TemplateExtensions
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
            string action) =>
            string.Join(
                      "/",
                      templateRoute.ParsedTemplate.Segments
                                   .Select(s => s.ToTemplateSegmentString()))
                  .Replace("{controller}", controller)
                  .Replace("{action}", action).ToLower();
        // ReSharper restore CheckNamespace
        // ReSharper restore MemberCanBePrivate.Global
    }
}