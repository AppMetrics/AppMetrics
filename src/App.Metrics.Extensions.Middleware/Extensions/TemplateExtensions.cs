// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing.Template
{
    // ReSharper restore CheckNamespace
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
    }
}